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
            game.AddPlayer(this);
        }

        public void BuyFromCasino(int requestAmount, Casino casino)
        {
            chips = requestAmount;
            casino.BuyChips(requestAmount);
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