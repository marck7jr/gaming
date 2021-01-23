using Marck7JR.Gaming.Data.Contracts;
using Microsoft.Win32;
using System.ComponentModel;

namespace Marck7JR.Gaming.Data.Ubisoft
{
    [Description("Ubisoft Connect")]
    [GameLibraryProtocol(GameLibraryProtocolKind.Install, "uplay://install/{0}")]
    [GameLibraryProtocol(GameLibraryProtocolKind.Run, "uplay://launch/{0}")]
    [GameLibraryProtocol(GameLibraryProtocolKind.Uninstall, "uplay://uninstall/{0}")]
    public class UbisoftLibrary : GameLibrary
    {
        private const string UbisoftRegistryKeyName = @"Software\Ubisoft\Launcher";

        protected override void InitializeComponent()
        {
            var baseRegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            RegistryKey = baseRegistryKey.OpenSubKey(UbisoftRegistryKeyName);

            if (RegistryKey is null)
            {
                baseRegistryKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                RegistryKey = baseRegistryKey.OpenSubKey(UbisoftRegistryKeyName);
            }
        }
    }
}
