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

        public Game CreateGame(Die die, int rollCount = 1)
        {
            return new Game(die, this, rollCount);
        }

        public static int GetMultiplier(int score, int rollCount)
        {
            return (int)Math.Pow(6, rollCount) / CountSumPossibilities(score, rollCount);
        }

        public static int CountSumPossibilities(int score, int rollCount)
        {
            if (rollCount == score)
                return 1;

            if (rollCount == 0 || score < rollCount)
                return 0;

            return CountSumPossibilities(score - 1, rollCount) +
                   CountSumPossibilities(score - 1, rollCount - 1) -
                   CountSumPossibilities(score - 7, rollCount - 1);
        }
    }
}