using Marck7JR.Gaming.Data.Contracts;
using Marck7JR.Gaming.Data.Ubisoft.Extensions;
using Marck7JR.Gaming.Data.Ubisoft.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Data.Ubisoft
{
    public sealed class UbisoftLibraryService : GameLibraryService<UbisoftLibrary>
    {
        public UbisoftLibraryService(UbisoftLibrary library) : base(library)
        {

        }

        private async IAsyncEnumerable<GameApplication> GetGameApplicationsOfflineAsync(UbisoftLibrary library)
        {
            if (library is IGameLibrary { IsAvailable: true } && library.GetManifests() is IEnumerable<UbisoftApplicationManifest> applicationManifests)
            {
                using var installsKey = library.RegistryKey!.OpenSubKey("Installs");
                var installsNames = installsKey?.GetSubKeyNames();

                var manifests = applicationManifests
                    .Where(manifest => !manifest.root.is_ulc && manifest.root.start_game is not null)
                    .GroupBy(manifest => manifest.install_id)
                    .Select(group => group.Last());

                await Task.CompletedTask;

                foreach (var manifest in manifests)
                {
                    yield return new(library, manifest)
                    {
                        AppId = $"{manifest.install_id}",
                        IsInstalled = installsNames.Any(__ => __.Equals($"{manifest.install_id}", StringComparison.InvariantCultureIgnoreCase)),
                        DisplayName = manifest.GetManifestName() ?? throw new ArgumentNullException(nameof(GameApplication.DisplayName)),
                    };
                }
            }
        }

        public override Func<UbisoftLibrary, IAsyncEnumerable<GameApplication>>? GetApplicationsOfflineAsync => GetGameApplicationsOfflineAsync;
    }
}
