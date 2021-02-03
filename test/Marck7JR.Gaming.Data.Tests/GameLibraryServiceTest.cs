using Marck7JR.Gaming.Data.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Data
{
    public abstract class GameLibraryServiceTest<T, U> : GameLibraryTest<U>
        where T : GameLibraryService<U>
        where U : GameLibrary
    {
        public static T? Service { get; protected set; }

        [TestMethod]
        public virtual async Task BuildAsync_IsNotNull()
        {
            Assert.IsNotNull(Service);
            Assert.IsNotNull(Service?.GetLibraryAsync);

            var library = await Service!.GetLibraryAsync!.Invoke();

            Assert.IsNotNull(library.Applications);

            library.Applications?.ToList().ForEach(keyValuePair =>
            {
                TestContext?.WriteLine(keyValuePair.Value.ToTestContext());
            });
        }
    }
}
