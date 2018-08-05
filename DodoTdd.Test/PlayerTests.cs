using System;
using DodoTdd.Test.DSL;
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
            var player = Create.Player.InSomeGame().Please();

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
            var player = Create.Player.InSomeGame().Please();

            Assert.ThrowsException<InvalidOperationException>(() => { player.Join(new Casino().CreateGame(new Die())); });
        }

        /// <summary>
        /// Я, как игрок, могу купить фишки у казино, чтобы делать ставки
        /// </summary>
        [TestMethod]
        public void CanBuyChips()
        {
            var casino = new Mock<Casino>();
            var player = Create.Player.InCasino(casino.Object).Please();

            player.BuyFromCasino(12, casino.Object);

            casino.Verify(x => x.BuyChips(It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Я, как игрок, могу сделать ставку в игре в кости, чтобы выиграть
        /// </summary>
        [TestMethod]
        public void CanMakeBetInGame()
        {
            var game = CreateGameMock();
            var player = Create.Player.InGame(game.Object).WithChips(12).Please();
           
            player.MakeBetOn(12, 1);

            game.Verify(x => x.AcceptBetFromPlayerOnScore(It.IsAny<int>(), It.IsAny<Player>(), It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Я, как игрок, не могу поставить фишек больше, чем я купил
        /// </summary>
        [TestMethod]
        public void ArgumentExceptionIsThrown_WhenBettingMoreChipsThanIsAvailable()
        {
            var amount = 12;
            var player = Create.Player.InSomeGame().WithChips(amount).Please();

            Assert.ThrowsException<ArgumentException>(() => player.MakeBetOn(amount + 1, 1));
        }

        /// <summary>
        /// Я, как игрок, могу сделать несколько ставок на разные числа, чтобы повысить вероятность выигрыша
        /// </summary>
        [TestMethod]
        public void CanMakeSeveralBets()
        {
            var game = CreateGameMock();
            Create.Player
                .WithChips(12)
                .InGame(game.Object)
                .Betting(4).On(1)
                .Betting(4).On(2)
                .Betting(4).On(3)
                .Please();

            game.Verify(x => x.AcceptBetFromPlayerOnScore(It.IsAny<int>(), It.IsAny<Player>(), It.IsAny<int>()), Times.Exactly(3));
        }

        /// <summary>
        /// Я, как игрок, могу поставить только на числа 1 - 6
        /// (первый)
        /// </summary>
        [TestMethod]
        public void ArgumentExceptionIsThrown_WhenBettingScoreIsLowerThanOneInOneDieGame()
        {
            var player = Create.Player.InSomeGame().WithChips(100).Please();
            var invalidScore = 0;

            Assert.ThrowsException<ArgumentException>(() => player.MakeBetOn(1, invalidScore));
        }

        /// <summary>
        /// Я, как игрок, могу поставить только на числа 1 - 6
        /// (второй)
        /// </summary>
        [TestMethod]
        public void ArgumentExceptionIsThrown_WhenBettingScoreIsGreaterThanSixInOneDieGame()
        {
            var player = Create.Player.InSomeGame().WithChips(100).Please();
            var invalidScore = 7;

            Assert.ThrowsException<ArgumentException>(() => player.MakeBetOn(1, invalidScore));
        }

        /// <summary>
        /// Я, как игрок, могу проиграть, если сделал неправильную ставку
        /// </summary>
        [TestMethod]
        public void ChipsCountUnchanged_WhenLostTheGame()
        {
            var casino = new Casino();
            var die = Create.Die.Rolling(1).Please();
            var game = casino.CreateGame(die);
            int chipsAmount = 100;
            var player = Create.Player.InCasino(casino).InGame(game).WithChips(chipsAmount).Betting(chipsAmount).On(2).Please();
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
            var casino = new Casino();
            var winningScore = 1;
            var die = Create.Die.Rolling(winningScore).Please();
            var game = casino.CreateGame(die);
            int chipsAmount = 100;
            var player = Create.Player.InCasino(casino).InGame(game).WithChips(chipsAmount).Betting(chipsAmount).On(winningScore).Please();
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
            var casino = new Casino();
            var winningScore = 1;
            var die = Create.Die.Rolling(winningScore).Please();
            var game = casino.CreateGame(die);
            var winningAmount = 7;
            var player =
                Create.Player
                .InCasino(casino).InGame(game)
                .WithChips(100)
                .Betting(winningAmount).On(winningScore)
                .Betting(11).On(2)
                .Betting(13).On(3)
                .Please();
            var formerChipsCount = player.Chips;

            game.Run();

            Assert.AreEqual(formerChipsCount + winningAmount * 6, player.Chips);
        }

        /// <summary>
        /// Я, как игрок, могу делать ставки на числа от 2 до 12
        /// (первый)
        /// </summary>
        [TestMethod]
        public void ArgumentExceptionIsThrown_WhenBettingScoreIsLowerThanTwoInTwoDiceGame()
        {
            var casino = new Casino();
            var game = casino.CreateGame(new Die(), 2);
            var player = Create.Player.InCasino(casino).InGame(game).WithChips(100).Please();
            var invalidScore = 1;

            Assert.ThrowsException<ArgumentException>(() => player.MakeBetOn(1, invalidScore));
        }

        /// <summary>
        /// Я, как игрок, могу делать ставки на числа от 2 до 12
        /// (второй)
        /// </summary>
        [TestMethod]
        public void ArgumentExceptionIsThrown_WhenBettingScoreIsGreaterThanTwelveInTwoDiceGame()
        {
            var casino = new Casino();
            var game = casino.CreateGame(new Die(), 2);
            var player = Create.Player.InCasino(casino).InGame(game).WithChips(100).Please();
            var invalidScore = 13;

            Assert.ThrowsException<ArgumentException>(() => player.MakeBetOn(1, invalidScore));
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
