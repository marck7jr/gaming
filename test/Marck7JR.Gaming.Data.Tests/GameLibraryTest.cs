using Marck7JR.Gaming.Data.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marck7JR.Gaming.Data
{
    public abstract class GameLibraryTest<T>
            where T : IGameLibrary
    {
        public static T? Library { get; protected set; }
        public TestContext? TestContext { get; set; }

        [TestMethod]
        public virtual void IsAvailable_IsTrue()
        {
            Assert.IsNotNull(Library);
            Assert.IsTrue(Library!.IsAvailable);
        }
    }
}
