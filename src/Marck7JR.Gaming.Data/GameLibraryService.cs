using Marck7JR.Core.Extensions;
using Marck7JR.Gaming.Data.Contracts;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Data
{
    public abstract class GameLibraryService<T> : IGameLibraryService<T, GameApplication>
        where T : GameLibrary
    {
        private readonly T _library;

        public GameLibraryService(T? library)
        {
            _library = library ?? throw new ArgumentNullException(nameof(library));
        }

        public GameLibraryService(IGameLibraryFactory gameLibraryFactory)
        {
            _library = gameLibraryFactory.GetGameLibrary<T>() ?? throw new NullReferenceException();
        }

        public async Task BuildLibraryAsync()
        {
            ConcurrentStack<Func<T, IAsyncEnumerable<GameApplication>>> concurrentStack = new();

            GetApplicationsOfflineAsync.IfNotNull(action => concurrentStack.Push(action));
            GetApplicationsOnlineAsync.IfNotNull(action => concurrentStack.Push(action));

            while (concurrentStack.TryPop(out Func<T, IAsyncEnumerable<GameApplication>> result))
            {
                var applications = await result(_library).ToListAsync();

                applications.AsParallel().ForAll(application =>
                {
                    if (application.AppId.IsNotNullOrEmpty())
                    {
                        this[application.AppId!] = application;
                    }
                });
            }
        }

        public IEnumerator<KeyValuePair<string, GameApplication>> GetEnumerator() => _library.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _library.GetEnumerator();

        public virtual Func<Task<T>>? GetLibraryAsync => () => BuildLibraryAsync().ContinueWith((task) =>
        {
            if (task is Task { IsCompleted: true, Status: TaskStatus.RanToCompletion })
            {
                // TODO: Need to implement raising events
            }

            return _library;
        });
        public virtual Func<T, IAsyncEnumerable<GameApplication>>? GetApplicationsOfflineAsync { get; }
        public virtual Func<T, IAsyncEnumerable<GameApplication>>? GetApplicationsOnlineAsync { get; }
        public bool IsAvailable => _library.IsAvailable;
        public GameApplication this[string appId] { get => _library[appId]; set => _library[appId] = value; }
        public GameApplication this[int index] { get => _library[index]; }
    }
}
