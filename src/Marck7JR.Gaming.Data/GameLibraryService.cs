﻿using Marck7JR.Core.Extensions;
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

        public GameLibraryService(T library)
        {
            _library = library;
        }

        private async Task<T> BuildGameLibraryAsync()
        {
            ConcurrentQueue<Func<T, IAsyncEnumerable<GameApplication>>> concurrentQueue = new();

            GetApplicationsOfflineAsync.IfNotNull(action => concurrentQueue.Enqueue(action));
            GetApplicationsOnlineAsync.IfNotNull(action => concurrentQueue.Enqueue(action));

            while (concurrentQueue.TryDequeue(out Func<T, IAsyncEnumerable<GameApplication>> result))
            {
                var applications = await result(_library)
                    .Where(application => application.AppId.IsNotNullOrEmpty())
                    .Where(application => !_library.Applications.ContainsKey(application.AppId!))
                    .ToDictionaryAsync(application => application.AppId!);

                applications.AsParallel().ForAll(keyValuePair =>
                {
                    _library.Applications.Add(keyValuePair);
                });
            }

            return _library;
        }

        public virtual Func<Task<T>>? BuildLibraryAsync => BuildGameLibraryAsync;
        public virtual Func<T, IAsyncEnumerable<GameApplication>>? GetApplicationsOfflineAsync { get; }
        public virtual Func<T, IAsyncEnumerable<GameApplication>>? GetApplicationsOnlineAsync { get; }
        public bool IsAvailable => _library.IsAvailable;
    }
}