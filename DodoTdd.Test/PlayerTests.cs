using System;
using System.Linq;
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
            var game = CreateGame();

            player.Join(game);

            Assert.IsTrue(player.InGame);
        }

        [TestMethod]
        public void InGameIsFalse_WhenLeavedGame()
        {
            var player = new Player();
            var game = CreateGame();
            player.Join(game);

            player.LeaveGame();

            Assert.IsFalse(player.InGame);
        }

        private static Game CreateGame()
        {
            return new Casino().CreateGame(new Die());
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
            var game = CreateGame();
            player.Join(game);

            Assert.ThrowsException<InvalidOperationException>(() => { player.Join(new Casino().CreateGame(new Die())); });
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
            var player = CreatePlayerWithChips(12);
            var game = CreateGameMock();
            player.Join(game.Object);

            player.MakeBetOn(12, 1);

            game.Verify(x => x.AcceptBetFromPlayerOnScore(It.IsAny<int>(), It.IsAny<Player>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void ArgumentExceptionIsThrown_WhenBettingMoreChipsThanIsAvailable()
        {
            var player = new Player();
            var casino = new Casino();
            var amount = 12;
            player.BuyFromCasino(amount, casino);
            var game = CreateGame();
            player.Join(game);

            Assert.ThrowsException<ArgumentException>(() => player.MakeBetOn(amount + 1, 1));
        }

        [TestMethod]
        public void CanMakeSeveralBets()
        {
            var player = CreatePlayerWithChips(12);
            var game = CreateGameMock();
            player.Join(game.Object);

            player.MakeBetOn(4, 1);
            player.MakeBetOn(4, 2);
            player.MakeBetOn(4, 3);

            game.Verify(x => x.AcceptBetFromPlayerOnScore(It.IsAny<int>(), It.IsAny<Player>(), It.IsAny<int>()), Times.Exactly(3));
        }

        [TestMethod]
        public void ArgumentExceptionIsThrown_WhenBettingScoreNotInRangeFromOneToSix()
        {
            var player = CreatePlayerWithChips(100);
            var game = CreateGameMock();
            player.Join(game.Object);

            var scores = Enumerable
                .Range(0, 100)
                .Where(score => score < 1 || score > 6);

            foreach (var score in scores)
            {
                Assert.ThrowsException<ArgumentException>(() => player.MakeBetOn(1, score));
            }
        }

        private static Mock<Game> CreateGameMock()
        {
            return new Mock<Game>(new Die(), new Casino());
        }

        [TestMethod]
        public void ChipsCountUnchanged_WhenLostTheGame()
        {
            int chipsAmount = 100;
            var player = CreatePlayerWithChips(chipsAmount);
            var die = new Mock<Die>();
            die.Setup(x => x.Roll()).Returns(1);
            var game = new Casino().CreateGame(die.Object);
            player.Join(game);
            player.MakeBetOn(chipsAmount, 2);
            var formerChipsCount = player.Chips;

            game.Run();

            Assert.AreEqual(formerChipsCount, player.Chips);
        }

        [TestMethod]
        public void ChipsCountIncreasedBySixTimesBetAmount_WhenWonTheGame()
        {
            int chipsAmount = 100;
            var player = CreatePlayerWithChips(chipsAmount);
            var die = new Mock<Die>();
            var winningScore = 1;
            die.Setup(x => x.Roll()).Returns(winningScore);
            var game = new Casino().CreateGame(die.Object);
            player.Join(game);
            player.MakeBetOn(chipsAmount, winningScore);
            var formerChipsCount = player.Chips;

            game.Run();

            Assert.AreEqual(formerChipsCount + chipsAmount * 6, player.Chips);
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

        private static Player CreatePlayerWithChips(int chipsAmount)
        {
            var player = new Player();
            player.BuyFromCasino(chipsAmount, new Casino());
            return player;
        }
    }
}
