using Marck7JR.Gaming.Data.Contracts;
using Microsoft.Win32;
using System.ComponentModel;

namespace Marck7JR.Gaming.Data.Steam
{
    [Description("Steam")]
    [GameLibraryProtocol(GameLibraryProtocolKind.Install, "steam://install/{0}")]
    [GameLibraryProtocol(GameLibraryProtocolKind.Run, "steam://rungameid/{0}")]
    [GameLibraryProtocol(GameLibraryProtocolKind.Purchase, "steam://purchase/{0}")]
    [GameLibraryProtocol(GameLibraryProtocolKind.Uninstall, "steam://uninstall/{0}")]
    public class SteamLibrary : GameLibrary
    {
        private const string SteamRegistryKeyName = @"Software\Valve\Steam";

        protected override void InitializeComponent()
        {
            var baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
            RegistryKey = baseKey.OpenSubKey(SteamRegistryKeyName);

            if (RegistryKey is null)
            {
                baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                RegistryKey = baseKey.OpenSubKey(SteamRegistryKeyName);
            }
        }
    }
}
