using System;
using System.Collections.Generic;

namespace DodoTdd.Test.DSL
{
    public class PlayerBuilder
    {
        public PlayerBuilder InSomeGame()
        {
            _game = _casino.CreateGame(new Die());
            return this;
        }

        public PlayerBuilder InGame(Game game)
        {
            _game = game;
            return this;
        }

        public PlayerBuilder WithChips(int amount)
        {
            _chipsAmount = amount;
            return this;
        }

        public Player Please()
        {
            if (_betChipsAmount != 0)
                throw new InvalidOperationException("Correct order is: .Betting(amount).On(score)");

            var player = new Player();
            if (_game != null)
                player.Join(_game);
            if (_chipsAmount > 0)
                player.BuyFromCasino(_chipsAmount, _casino);
            foreach (var bet in _bets)
                player.MakeBetOn(bet.Amount, bet.Score);

            return player;
        }

        public PlayerBuilder InCasino(Casino casino)
        {
            _casino = casino;
            return this;
        }

        public PlayerBuilder Betting(int chipsAmount)
        {
            if (_betChipsAmount != 0)
                throw new InvalidOperationException("Correct order is: .Betting(amount).On(score)");

            _betChipsAmount = chipsAmount;
            return this;
        }

        public PlayerBuilder On(int score)
        {
            if(_betChipsAmount == 0)
                throw  new InvalidOperationException("Correct order is: .Betting(amount).On(score)");

            _bets.Add(new Bet(_betChipsAmount, score));
            _betChipsAmount = 0;
            return this;
        }

        Casino _casino = new Casino();
        Game _game;
        int _chipsAmount;
        List<Bet> _bets = new List<Bet>();
        int _betChipsAmount;

        private class Bet
        {
            public Bet(int amount, int score)
            {
                Amount = amount;
                Score = score;
            }

            public int Amount { get; }
            public int Score { get; }
        }
    }
}