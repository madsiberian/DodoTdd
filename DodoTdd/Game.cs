using System;
using System.Collections.Generic;

namespace DodoTdd
{
    public class Game
    {
        public Game(Die die)
        {
            _die = die;
        }

        public void AddPlayer(Player player)
        {
            if (_counter == 6)
                throw new InvalidOperationException("Game is full");

            ++_counter;
        }

        public virtual void AcceptBetFromPlayerOnScore(int amount, Player player, int score)
        {
            var bet = new Bet(amount, player);
            _bets.Add(score, bet);
        }

        public void Run()
        {
            var winningScore = _die.Roll();

            if(!_bets.ContainsKey(winningScore))
                return;

            var bet = _bets[winningScore];

            var player = bet.Player;
            player.Chips += bet.Amount * 6;
        }

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

            public int Amount { get;  }
            public Player Player { get;  }
        }
    }


}