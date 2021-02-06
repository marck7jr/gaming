using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marck7JR.Gaming.Data.Tests
{
    [TestClass]
    public class GameApplicationTest
    {
        [TestMethod]
        public void GameApplication_AreEquals()
        {
            GameApplication x = new(), y = new();

            x.AppId = y.AppId = Guid.NewGuid().ToString();
            x.Issuer = y.Issuer = typeof(GameApplicationTest);

            Assert.AreEqual(x, y);
            Assert.IsTrue(x == y);
        }

        [TestMethod]
        public void GameApplication_AreNotEquals()
        {
            GameApplication x = new(), y = new();

            x.AppId = Guid.NewGuid().ToString();
            y.AppId = Guid.NewGuid().ToString();

            x.Issuer = y.Issuer = typeof(GameApplicationTest);

            Assert.AreNotEqual(x, y);
            Assert.IsTrue(x != y);
            Assert.IsTrue(x != null);
        }

        [TestMethod]
        public void GameApplication_FirtOrDefault_AreNotNull()
        {
            GameApplication x = new(), y = new();

            x.AppId = y.AppId = Guid.NewGuid().ToString();
            x.Issuer = y.Issuer = typeof(GameApplicationTest);
        }
    }
}
