using System;

namespace DodoTdd
{
    public class Casino
    {
        public virtual void BuyChips(int amount)
        {

        }

        public void ValidateBet(int bet)
        {
            throw new ArgumentException("Bet is not multiple of five");
        }
    }
}