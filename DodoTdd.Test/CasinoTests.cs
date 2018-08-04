using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DodoTdd.Test
{
    [TestClass]
    public class CasinoTests
    {
        /// <summary>
        /// Я, как казино, принимаю только ставки, кратные 5
        /// </summary>
        [TestMethod]
        public void ArgumentExceptionIsThrown_WhenValidatedBetIsNotMultipleOfFive()
        {
            var casino = new Casino();
            var bets = Enumerable
                .Range(1, 100)
                .Where(bet => bet % 5 != 0);

            foreach (var bet in bets)
            {
                Assert.ThrowsException<ArgumentException>(() => casino.ValidateBet(bet));
            }
        }

        /// <summary>
        /// Я, как казино, получаю фишки, которые проиграл игрок
        /// </summary>
        [TestMethod]
        public void ChipsCountIncreasedByBet_WhenPlayerLostTheGame()
        {
            var casino = new Casino();
            var player = new Player();
            int chipsAmount = 100;
            player.BuyFromCasino(chipsAmount, casino);
            var die = new Mock<Die>();
            die.Setup(x => x.Roll()).Returns(1);
            var game = casino.CreateGame(die.Object);
            player.Join(game);
            player.MakeBetOn(chipsAmount, 2);
            var formerChipsCount = casino.Chips;

            game.Run();

            Assert.AreEqual(formerChipsCount + chipsAmount, casino.Chips);
        }
    }
}
