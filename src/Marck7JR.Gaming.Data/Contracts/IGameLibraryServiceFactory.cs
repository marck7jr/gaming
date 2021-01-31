using System.Collections.Generic;

namespace Marck7JR.Gaming.Data.Contracts
{
    public interface IGameLibraryServiceFactory
    {
        public T? GetGameLibraryService<T>() where T : IGameLibraryService;
        public IEnumerable<IGameLibraryService>? GetGameLibraryServices();
    }
}
