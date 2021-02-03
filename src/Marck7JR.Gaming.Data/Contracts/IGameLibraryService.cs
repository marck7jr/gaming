using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Data.Contracts
{
    public interface IGameLibraryService
    {
        public bool IsAvailable { get; }
        public Task BuildLibraryAsync();
    }

    public interface IGameLibraryService<T, U> : IGameLibraryService
        where T : IGameLibrary<U>
        where U : IGameApplication
    {
        public Func<Task<T>>? GetLibraryAsync { get; }
        public Func<T, IAsyncEnumerable<U>>? GetApplicationsOfflineAsync { get; }
        public Func<T, IAsyncEnumerable<U>>? GetApplicationsOnlineAsync { get; }
    }
}
