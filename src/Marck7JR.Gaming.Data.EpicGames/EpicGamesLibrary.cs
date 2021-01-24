using Marck7JR.Gaming.Data.Contracts;
using Microsoft.Win32;
using System.ComponentModel;

namespace Marck7JR.Gaming.Data.EpicGames
{
    [Description("Epic Games Launcher")]
    [GameLibraryProtocol(GameLibraryProtocolKind.Install, "com.epicgames.launcher://apps/{0}?action=launch")]
    [GameLibraryProtocol(GameLibraryProtocolKind.Run, "com.epicgames.launcher://apps/{0}?action=launch&silent=true")]
    public class EpicGamesLibrary : GameLibrary
    {
        private const string EpicGamesRegistryKeyName = @"Software\Epic Games\EpicGamesLauncher";

        protected override void InitializeComponent()
        {
            var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            RegistryKey = baseKey.OpenSubKey(EpicGamesRegistryKeyName);

            if (RegistryKey is null)
            {
                baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey = baseKey.OpenSubKey(EpicGamesRegistryKeyName);
            }
        }
    }
}
