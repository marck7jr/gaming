using Marck7JR.Gaming.Data.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Marck7JR.Gaming.Data
{
    public class GameLibraryServiceFactory : IGameLibraryServiceFactory
    {
        private readonly IEnumerable<IGameLibraryService> _services;

        public GameLibraryServiceFactory(IEnumerable<IGameLibraryService> services)
        {
            _services = services;
        }

        public T? GetGameLibraryService<T>() where T : IGameLibraryService => (T?)_services.FirstOrDefault(service => service.GetType() == typeof(T));
        public IEnumerable<IGameLibraryService>? GetGameLibraryServices() => _services;
    }
}
