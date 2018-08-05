using System;
using System.Collections.Generic;
using System.Linq;
using DodoTdd.Test.DSL;
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
            int chipsAmount = 100;
            var die = Create.Die.Rolling(1).Please();
            var game = casino.CreateGame(die);
            Create.Player.InCasino(casino).InGame(game).WithChips(chipsAmount).Betting(chipsAmount).On(2).Please();
            var formerChipsCount = casino.Chips;

            game.Run();

            Assert.AreEqual(formerChipsCount + chipsAmount, casino.Chips);
        }

        /// <summary>
        /// Я, как казино, определяю выигрышный коэффициент по вероятности выпадения того или иного номера
        /// </summary>
        [TestMethod]
        public void WinningMultiplierDependsOnWinningProbability()
        {
            var rollCount = 2;

            var multipliers = new Dictionary<int, int>
            {
                {  2, 36 },
                {  3, 18 },
                {  4, 12 },
                {  5,  9 },
                {  6,  7 },
                {  7,  6 },
                {  8,  7 },
                {  9,  9 },
                { 10, 12 },
                { 11, 18 },
                { 12, 36 }
            };

            foreach (var score in multipliers.Keys)
            {
                var multiplier = multipliers[score];
                Assert.AreEqual(multiplier, Casino.GetMultiplier(score, rollCount));
            }
        }
    }
}
