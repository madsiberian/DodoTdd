using System;

namespace DodoTdd
{
    public class Player
    {
        public int Chips;
        public bool InGame { get; set; }


        public void Join(Game game)
        {
            if (InGame)
            {
                throw new InvalidOperationException("Player is already in game");
            }

            InGame = true;
            _game = game;
            game.AddPlayer(this);
        }

        public void BuyFromCasino(int requestAmount, Casino casino)
        {
            Chips = requestAmount;
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

        public void MakeBet(int amount)
        {
            _game.AcceptBetFromPlayer(amount, this);
        }

        Game _game;
    }
}