using Microsoft.Win32;
using System.Collections.Generic;

namespace Marck7JR.Gaming.Data.Contracts
{
    public interface IGameLibrary
    {
        public string? DisplayName { get; }
        public bool IsAvailable { get; }
        public RegistryKey? RegistryKey { get; }
    }

    public interface IGameLibrary<T> : IGameLibrary, IEnumerable<KeyValuePair<string, T>>
        where T : IGameApplication
    {
        public IDictionary<string, T> Applications { get; }
        public T this[int index] { get; }
        public T this[string appId] { get; set; }
    }
}
