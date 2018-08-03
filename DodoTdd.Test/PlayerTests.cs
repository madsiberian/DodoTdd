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

            player.LeaveGame();

            Assert.IsFalse(player.InGame);
        }

        [TestMethod]
        public void InvalidOperationIsThrown_WhenLeavingGameWhileNotInGame()
        {
            var player = new Player();

            Assert.ThrowsException<InvalidOperationException>(() => { player.LeaveGame(); });
        }

        [TestMethod]
        public void InvalidOperationIsThrown_WhenJoinGameIfAlreadyInGame()
        {
            var player = new Player();
            var game = new Game();
            player.Join(game);

            Assert.ThrowsException<InvalidOperationException>(() => { player.Join(new Game()); });
        }

        [TestMethod]
        public void HasRequestedChipsCount_WhenBoughtChipsFromCasino()
        {
            var player = new Player();
            var requestAmount = 42;
            player.BuyFromCasino(requestAmount);

            Assert.AreEqual(requestAmount, player.chips);
        }
    }

    public class Casino
    {
    }
}
