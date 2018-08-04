using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DodoTdd.Test
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void InvalidOperationIsThrown_WhenMoreThanSixPlayersJoin()
        {
            var game = new Casino().CreateGame(new Die());
            for (int i = 1; i <= 6; ++i)
                new Player().Join(game);

            Assert.ThrowsException<InvalidOperationException>(() => { new Player().Join(game); });
        }
    }
}
