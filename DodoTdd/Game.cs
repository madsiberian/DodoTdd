using System;
using System.Collections.Generic;

namespace DodoTdd
{
    public class Game
    {
        public Game(Die die, Casino casino, int rollCount = 1)
        {
            _die = die;
            _casino = casino;
            RollCount = rollCount;
        }

        public int RollCount { get; }

        public void AddPlayer(Player player)
        {
            if (_counter == 6)
                throw new InvalidOperationException("Game is full");

            ++_counter;
        }

        public virtual void AcceptBetFromPlayerOnScore(int amount, Player player, int score)
        {
            if (score < 1  * RollCount || score > 6 * RollCount)
                throw new ArgumentException("Invalid score");

            var bet = new Bet(amount, player);
            _bets.Add(score, bet);
        }

        public void Run()
        {
            var winningScore = _die.Roll();

            foreach (var score in _bets.Keys)
            {
                var bet = _bets[score];
                if (score == winningScore)
                {
                    var player = bet.Player;
                    player.Chips += bet.Amount * 6;
                    continue;
                }

                _casino.Chips += bet.Amount;
            }
        }

        Casino _casino;
        int _counter;
        Die _die;
        Dictionary<int, Bet> _bets = new Dictionary<int, Bet>();

        public class Bet
        {
            public Bet(int amount, Player player)
            {
                Amount = amount;
                Player = player;
            }

            public int Amount { get; }
            public Player Player { get; }
        }
    }
}