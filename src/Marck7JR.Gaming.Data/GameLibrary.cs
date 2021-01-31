using Marck7JR.Gaming.Data.Contracts;
using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Marck7JR.Gaming.Data
{
    public abstract class GameLibrary : IGameLibrary<GameApplication>
    {
        public GameLibrary()
        {
            InitializeComponent();
        }

        public GameApplication this[int index] { get => Applications.ElementAt(index).Value; }
        public GameApplication this[string appId] { get => Applications[appId]; set => Applications[appId] = value; }

        public IDictionary<string, GameApplication> Applications { get; } = new Dictionary<string, GameApplication>();
        public string? DisplayName => GetType().GetCustomAttribute<DescriptionAttribute>().Description;
        public virtual bool IsAvailable => RegistryKey is not null;
        public RegistryKey? RegistryKey { get; protected set; }

        public IEnumerator<KeyValuePair<string, GameApplication>> GetEnumerator() => Applications.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Applications.GetEnumerator();
        protected abstract void InitializeComponent();
    }
}
