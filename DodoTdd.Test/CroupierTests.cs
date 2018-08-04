using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DodoTdd.Test
{
    public class CroupierTests
    {
        /// <summary>
        /// Я, как крупье, могу сделать игру с двумя кубиками
        /// </summary>
        [TestMethod]
        public void CanCreateGameWithTwoDice()
        {
            var croupier = new Croupier();
            var casino = new Casino();

            var game = croupier.CreateTwoDiceGame(casino);

            Assert.AreEqual(2, game.RollCount);
        }
    }
}
