using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DodoTdd.Test
{
    [TestClass]
    public class PlayerTests
    {
        /// <summary>
        /// Я, как игрок, могу войти в игру
        /// </summary>
        [TestMethod]
        public void InGameIsTrue_WhenJoinedGame()
        {
            var player = new Player();
            var game = CreateGame();

            player.Join(game);

            Assert.IsTrue(player.InGame);
        }

        /// <summary>
        /// Я, как игрок, могу выйти из игры
        /// </summary>
        [TestMethod]
        public void InGameIsFalse_WhenLeavedGame()
        {
            var player = new Player();
            var game = CreateGame();
            player.Join(game);

            player.LeaveGame();

            Assert.IsFalse(player.InGame);
        }

        /// <summary>
        /// Я, как игрок, не могу выйти из игры, если я в нее не входил
        /// </summary>
        [TestMethod]
        public void InvalidOperationIsThrown_WhenLeavingGameWhileNotInGame()
        {
            var player = new Player();

            Assert.ThrowsException<InvalidOperationException>(() => { player.LeaveGame(); });
        }

        /// <summary>
        /// Я, как игрок, могу играть только в одну игру одновременно
        /// </summary>
        [TestMethod]
        public void InvalidOperationIsThrown_WhenJoinGameIfAlreadyInGame()
        {
            var player = new Player();
            var game = CreateGame();
            player.Join(game);

            Assert.ThrowsException<InvalidOperationException>(() => { player.Join(new Casino().CreateGame(new Die())); });
        }

        /// <summary>
        /// Я, как игрок, могу купить фишки у казино, чтобы делать ставки
        /// </summary>
        [TestMethod]
        public void CanBuyChips()
        {
            var player = new Player();
            var casino = new Mock<Casino>();

            player.BuyFromCasino(12, casino.Object);

            casino.Verify(x => x.BuyChips(It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Я, как игрок, могу сделать ставку в игре в кости, чтобы выиграть
        /// </summary>
        [TestMethod]
        public void CanMakeBetInGame()
        {
            var player = CreatePlayerWithChips(12);
            var game = CreateGameMock();
            player.Join(game.Object);

            player.MakeBetOn(12, 1);

            game.Verify(x => x.AcceptBetFromPlayerOnScore(It.IsAny<int>(), It.IsAny<Player>(), It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Я, как игрок, не могу поставить фишек больше, чем я купил
        /// </summary>
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

        /// <summary>
        /// Я, как игрок, могу сделать несколько ставок на разные числа, чтобы повысить вероятность выигрыша
        /// </summary>
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

        /// <summary>
        /// Я, как игрок, могу поставить только на числа 1 - 6
        /// </summary>
        [TestMethod]
        public void ArgumentExceptionIsThrown_WhenBettingScoreNotInRangeFromOneToSix()
        {
            var player = CreatePlayerWithChips(100);
            var game = CreateGame();
            player.Join(game);

            var scores = Enumerable
                .Range(0, 100)
                .Where(score => score < 1 || score > 6);

            foreach (var score in scores)
            {
                Assert.ThrowsException<ArgumentException>(() => player.MakeBetOn(1, score));
            }
        }

        /// <summary>
        /// Я, как игрок, могу проиграть, если сделал неправильную ставку
        /// </summary>
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

        /// <summary>
        /// Я, как игрок, могу выиграть 6 ставок, если сделал правильную ставку
        /// </summary>
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

        /// <summary>
        /// Я, как игрок, могу сделать несколько ставок на разные числа и получить выигрыш по тем, которые выиграли
        /// </summary>
        [TestMethod]
        public void ChipsCountIncreasedBySixTimesWinningBetAmount_WhenOneOfTheBetsWonTheGame()
        {
            var player = CreatePlayerWithChips(100);
            var die = new Mock<Die>();
            var winningScore = 1;
            die.Setup(x => x.Roll()).Returns(winningScore);
            var game = new Casino().CreateGame(die.Object);
            player.Join(game);
            player.MakeBetOn(10, 1);
            player.MakeBetOn(10, 2);
            player.MakeBetOn(10, 3);
            var formerChipsCount = player.Chips;

            game.Run();

            Assert.AreEqual(formerChipsCount + 10 * 6, player.Chips);
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

        static Player CreatePlayerWithChips(int chipsAmount)
        {
            var player = new Player();
            player.BuyFromCasino(chipsAmount, new Casino());
            return player;
        }

        static Mock<Game> CreateGameMock()
        {
            return new Mock<Game>(new Die(), new Casino(), 1);
        }

        static Game CreateGame()
        {
            return new Casino().CreateGame(new Die());
        }
    }
}
