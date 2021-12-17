using BotInterface.Bot;
using BotInterface.Game;
using System;
using System.Collections.Generic;

namespace RPS_Bot
{
    public class RPSBot : IBot
    {
        const int maxDynamite = 100;
        int usedDynamite = 0;
        Dictionary<char, int> Moves = new Dictionary<char, int>();
        Move IBot.MakeMove(Gamestate gamestate)
        {
            if (usedDynamite < maxDynamite)
            {
                usedDynamite++;
                return Move.D;
            }
            else if (gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP2() != Move.D)
            {
                return gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP2();
            }
            else
            {
                return Move.P;
            }
        }
    }
}
