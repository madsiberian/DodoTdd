﻿using System;

namespace DodoTdd
{
    public class Player
    {
        public int chips;
        public bool InGame { get; set; }

        public void Join(Game game)
        {
            InGame = true;
        }

        public void BuyFromCasino(int requestAmount)
        {
            chips = requestAmount;
        }

        public void Leave(Game game)
        {
            if (!InGame)
            {
                throw new InvalidOperationException("Not in Game");
            }

            InGame = false;
        }
    }
}