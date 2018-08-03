using System;
using System.Net.Cache;
using DodoTdd;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DodoTdd.Test
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void InGameIsTrue_WhenJoinedGame()
        {
            var player = new Player();
            var game = new Game();

            player.Join(game);

            Assert.IsTrue(player.InGame);
        }

        [TestMethod]
        public void InGameIsFalse_WhenLeavedGame()
        {
            var player = new Player();
            var game = new Game();
            player.Join(game);

            player.Leave(game);

            Assert.IsFalse(player.InGame);
        }

        [TestMethod]
        public void InvalidOperationIsThrown_WhenLeavingGameWhileNotInGame()
        {
            var player = new Player();
            var game = new Game();

            Assert.ThrowsException<InvalidOperationException>(() => { player.Leave(game); });
        }

        [TestMethod]
        public void HasRequestedChipsCount_WhenBoughtChipsFromCasino()
        {
            var player = new Player();
            var casino = new Casino();
            var requestAmount = 42;
            player.BuyFromCasino(requestAmount);

            Assert.AreEqual(requestAmount, player.chips);
        }
    }

    public class Casino
    {
    }
}
