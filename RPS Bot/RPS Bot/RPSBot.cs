using BotInterface.Bot;
using BotInterface.Game;
using System;
using System.Collections.Generic;

namespace RPS_Bot
{
    public class RPSBot : IBot
    {
        const int maxDynamite = 100;
        const int maxConsecutiveDynamite = 5;
        int usedDynamite = 0;
        int consecutiveDynamite = 0;
        bool canDynamite = true;

        int firstHundred = 0;
        Dictionary<char, int> Moves = new Dictionary<char, int>();
        Move IBot.MakeMove(Gamestate gamestate)
        {
            if (usedDynamite < maxDynamite && consecutiveDynamite < maxConsecutiveDynamite && canDynamite && firstHundred > 100)
            {
                usedDynamite++;
                consecutiveDynamite++;
                return Move.D;
            }
            else
            {
                canDynamite = false;
                firstHundred++;
                if (consecutiveDynamite == 0)
                {
                    canDynamite = true;
                }
                if (consecutiveDynamite > 0)
                {
                    consecutiveDynamite--;
                }
                try
                {
                    if (gamestate.GetRounds()[gamestate.GetRounds().Length - 2].GetP2() != Move.D)
                    {
                        return gamestate.GetRounds()[gamestate.GetRounds().Length - 2].GetP2();
                    }
                    else
                    {
                        return Move.W;
                    }
                }
                catch
                {
                    return Move.W;
                }

            }
        }
    }
}
