using System.Collections.Generic;

namespace Marck7JR.Gaming.Data.Contracts
{
    public interface IGameLibraryFactory
    {
        public IGameLibrary? GetGameLibrary(IGameApplication application);
        public T? GetGameLibrary<T>() where T : IGameLibrary;
        public IEnumerable<IGameLibrary> GetGameLibraries();
    }
}
