using System;

namespace DodoTdd
{
    public class Player
    {
        public int chips;
        public bool InGame { get; set; }

        public void Join(Game game)
        {
            if (InGame)
            {
                throw new InvalidOperationException("Player is already in game");
            }

            InGame = true;
        }

        public void BuyFromCasino(int requestAmount)
        {
            chips = requestAmount;
        }

        public void LeaveGame()
        {
            if (!InGame)
            {
                throw new InvalidOperationException("Player is not in game");
            }

            InGame = false;
        }
    }
}