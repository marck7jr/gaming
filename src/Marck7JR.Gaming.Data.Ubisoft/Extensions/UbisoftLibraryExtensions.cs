using Marck7JR.Core.Extensions;
using Marck7JR.Gaming.Data.Ubisoft.Infrastructure;
using Marck7JR.Gaming.Data.Ubisoft.Messages;
using System.Collections.Generic;
using System.IO;

namespace Marck7JR.Gaming.Data.Ubisoft.Extensions
{
    internal static class UbisoftLibraryExtensions
    {
        internal static IEnumerable<UbisoftApplicationManifest>? GetManifests(this UbisoftLibrary library)
        {
            if (library.IsAvailable && library.RegistryKey?.GetValue("InstallDir") is string installDir)
            {
                var path = Path.Combine(installDir, "cache", "configuration", "configurations");

                if (File.Exists(path))
                {
                    var bytes = File.ReadAllBytes(path);
                    var applicationCollection = UbisoftApplicationCollection.Parser.ParseFrom(bytes);

                    foreach (var application in applicationCollection.Games)
                    {
                        if (!application.Info.IsNullOrEmpty())
                        {
                            var manifest = application.Info.FromYaml<UbisoftApplicationManifest>();

                            if (manifest is not null && string.IsNullOrEmpty(manifest.root?.third_party_platform?.name))
                            {
                                manifest.uplay_id = application.Id;
                                manifest.install_id = application.Installid;

                                yield return manifest;
                            }
                        }
                    }
                }
            }
        }
    }
}
