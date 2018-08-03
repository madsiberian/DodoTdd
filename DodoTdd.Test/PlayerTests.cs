using System;
using System.Net.Cache;
using System.Security.Policy;
using DodoTdd;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
        public void CanBuyChips()
        {
            var player = new Player();
            var casino = new Mock<Casino>();

            player.BuyFromCasino(12, casino.Object);

            casino.Verify(x => x.BuyChips(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void CanMakeBetInGame()
        {
            var player = new Player();
            player.BuyFromCasino(12, new Casino());
            var game = new Mock<Game>();
            player.Join(game.Object);

            player.MakeBetOn(12, 1);

            game.Verify(x => x.AcceptBetFromPlayer(It.IsAny<int>(), It.IsAny<Player>()), Times.Once);
        }

        [TestMethod]
        public void ArgumentExceptionIsThrown_WhenBettingMoreChipsThanIsAvailable()
        {
            var player = new Player();
            var casino = new Casino();
            var amount = 12;
            player.BuyFromCasino(amount, casino);
            var game = new Game();
            player.Join(game);

            Assert.ThrowsException<ArgumentException>(() => player.MakeBetOn(amount + 1, 1));
        }

        [TestMethod]
        public void CanMakeSeveralBets()
        {
            var player = new Player();
            player.BuyFromCasino(12, new Casino());
            var game = new Mock<Game>();
            player.Join(game.Object);

            player.MakeBetOn(4, 1);
            player.MakeBetOn(4, 2);
            player.MakeBetOn(4, 3);

            game.Verify(x => x.AcceptBetFromPlayer(It.IsAny<int>(), It.IsAny<Player>()), Times.Exactly(3));
        }

        

        [TestMethod]
        public void HasRequestedChipsCount_WhenBoughtChipsFromCasino()
        {
            var player = new Player();
            var requestAmount = 42;
            var casino = new Casino();
            player.BuyFromCasino(requestAmount, casino);

            Assert.AreEqual(requestAmount, player.Chips);
        }
    }


    //class Casino
    //{
    //    public Game CreateGame()
    //    {
    //        return new Game(this);
    //    }

    //    public void ValidBet(int amount)
    //    {
    //        return true;
    //    }
    //}

    //class Game
    //{
    //    private Casino _casino;

    //    public virtual void AcceptBetFromPlayer(int amount, Player player)
    //    {
    //        _casino.ValidBet(amount);
    //    }
    //}


    //class Player
    //{

    //}


}
