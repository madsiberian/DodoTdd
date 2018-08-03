using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DodoTdd.Test
{
    [TestClass]
    public class CasinoTests
    {
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
    }
}
