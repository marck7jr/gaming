using Marck7JR.Gaming.Data.Contracts;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Marck7JR.Gaming.Data
{
    public abstract class GameLibrary : IGameLibrary<GameApplication>
    {
        public GameLibrary()
        {
            InitializeComponent();
        }

        public IDictionary<string, GameApplication> Applications { get; } = new Dictionary<string, GameApplication>();
        public string? DisplayName => GetType().GetCustomAttribute<DescriptionAttribute>().Description;
        public virtual bool IsAvailable => RegistryKey is not null;
        public RegistryKey? RegistryKey { get; protected set; }
        public IEnumerable<IGameLibraryProtocol> Protocols => GetType().GetCustomAttributes<GameLibraryProtocolAttribute>();
        protected abstract void InitializeComponent();
    }
}
