using Marck7JR.Gaming.Data.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Marck7JR.Gaming.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGameLibraryService<T>(this IServiceCollection services)
            where T : class, IGameLibraryService
        {
            Type? type = typeof(T).BaseType.GetGenericArguments()
                .FirstOrDefault();

            if (type is not null)
            {
                services.AddTransient<T>();
                services.AddSingleton(type);

                return services;
            }

            throw new NullReferenceException();
        }
    }
}
