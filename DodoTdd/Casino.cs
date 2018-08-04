using System;

namespace DodoTdd
{
    public class Casino
    {
        public int Chips { get; set; }

        public virtual void BuyChips(int amount)
        {

        }

        public void ValidateBet(int bet)
        {
            throw new ArgumentException("Bet is not multiple of five");
        }

        public Game CreateGame(Die die)
        {
            return new Game(die, this);
        }
    }
}