using Marck7JR.Gaming.Data.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Marck7JR.Gaming.Data
{
    public class GameLibraryFactory : IGameLibraryFactory
    {
        private readonly IEnumerable<IGameLibrary> _libraries;

        public GameLibraryFactory(IEnumerable<IGameLibrary> libraries)
        {
            _libraries = libraries;
        }

        public IEnumerable<IGameLibrary> GetGameLibraries() => _libraries;
        public IGameLibrary? GetGameLibrary(IGameApplication application) => _libraries.FirstOrDefault(library => library.GetType() == application.Issuer);
        public T? GetGameLibrary<T>() where T : IGameLibrary => (T?)_libraries.FirstOrDefault(library => library.GetType() == typeof(T));
    }
}
