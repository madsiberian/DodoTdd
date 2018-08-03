using System;

namespace DodoTdd
{
    public class Game
    {
        public void AddPlayer(Player player)
        {
            if (_counter == 6)
                throw new InvalidOperationException("Game is full");

            ++_counter;
        }

        int _counter;

        public virtual void AcceptBetFromPlayer(int amount, Player player)
        {
        }
    }
}