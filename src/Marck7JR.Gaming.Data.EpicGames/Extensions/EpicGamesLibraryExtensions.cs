using Marck7JR.Core.Extensions;
using Marck7JR.Gaming.Data.Contracts;
using Marck7JR.Gaming.Data.EpicGames.Infrastructure;
using System.Collections.Generic;
using System.IO;

namespace Marck7JR.Gaming.Data.EpicGames.Extensions
{
    internal static class EpicGamesLibraryExtensions
    {
        internal static IEnumerable<EpicGamesApplicationManifest>? GetManifests(this EpicGamesLibrary library)
        {
            if (library is IGameLibrary { IsAvailable: true } && library.RegistryKey?.GetValue("AppDataPath") is string appDataPath)
            {
                var path = Path.Combine(appDataPath, "Manifests");

                if (Directory.Exists(path))
                {
                    var files = Directory.EnumerateFiles(path);

                    foreach (var file in files)
                    {
                        var text = File.ReadAllText(file);

                        if (text.FromJson<EpicGamesApplicationManifest>() is EpicGamesApplicationManifest manifest)
                        {
                            yield return manifest;
                        }
                    }
                }
            }
        }
    }
}
