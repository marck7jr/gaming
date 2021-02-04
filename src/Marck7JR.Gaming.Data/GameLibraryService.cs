using Marck7JR.Core.Extensions;
using Marck7JR.Gaming.Data.Contracts;
using System;
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
                        lock (_library.Applications)
                        {
                            _library.Applications[application.AppId!] = application;
                        }
                    }
                });
            }
        }

        public virtual Func<Task<T>>? GetLibraryAsync => () => BuildLibraryAsync().ContinueWith((task) =>
        {
            task.ConfigureAwait(true).GetAwaiter().OnCompleted(() =>
            {
                // TODO: Need to implement raising events
            });

            task.Wait();

            return _library;
        });
        public virtual Func<T, IAsyncEnumerable<GameApplication>>? GetApplicationsOfflineAsync { get; }
        public virtual Func<T, IAsyncEnumerable<GameApplication>>? GetApplicationsOnlineAsync { get; }
        public bool IsAvailable => _library.IsAvailable;
    }
}
