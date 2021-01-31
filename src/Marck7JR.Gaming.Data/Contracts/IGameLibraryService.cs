using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Data.Contracts
{
    public interface IGameLibraryService
    {
        public bool IsAvailable { get; }
    }

    public interface IGameLibraryService<T, U> : IGameLibraryService, IEnumerable<KeyValuePair<string, U>>
        where T : IGameLibrary<U>
        where U : IGameApplication
    {
        public Func<Task<T>>? BuildLibraryAsync { get; }
        public Func<T, IAsyncEnumerable<U>>? GetApplicationsOfflineAsync { get; }
        public Func<T, IAsyncEnumerable<U>>? GetApplicationsOnlineAsync { get; }
        public U this[int index] { get; }
        public U this[string appId] { get; set; }
    }
}
