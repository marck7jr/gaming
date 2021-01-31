using Marck7JR.Gaming.Data.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

namespace Marck7JR.Gaming.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGameLibraryService<T>(this IServiceCollection services)
            where T : class, IGameLibraryService
        {
            services.TryAddSingleton<IGameLibraryFactory, GameLibraryFactory>();
            services.TryAddSingleton<IGameLibraryServiceFactory, GameLibraryServiceFactory>();

            Type? type = typeof(T).BaseType.GetGenericArguments()
                .FirstOrDefault();

            if (type is not null)
            {
                services.AddSingleton<IGameLibraryService, T>();
                services.AddSingleton(typeof(IGameLibrary), type);

                return services;
            }

            throw new NullReferenceException();
        }
    }
}
