using BotInterface.Bot;
using BotInterface.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPS_Bot
{
    public class RPSBot : IBot
    {
        const int maxDynamite = 100;
        const int maxConsecutiveDynamite = 1;
        int usedDynamite = 0;
        int consecutiveDynamite = 0;
        bool canDynamite = true;

        int opponentConsecutiveDynamite = 0;
        int groupedDynamiteCounter = 0;
        int groupedDynamiteTrigger = 3;
        bool opponentClumpDynamite = false;

        int opponentCurrentClump = 0;
        int waterSuccess = 0;
        int dynamiteSuccess = 0;
        int successRate = 0;
        bool offsetDueToFail = false;

        List<int> opponentMoves = new List<int>() { 0, 0, 0, 0, 0 };//0:D 1:W 2:R 3:P 4:S


        Move IBot.MakeMove(Gamestate gamestate)
        {
            try
            {
                if ((gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP1() == Move.W) && (gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP2() != Move.D))
                {
                    waterSuccess++;
                }
                else
                {
                    waterSuccess--;
                }
                if ((gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP1() == Move.D) && (gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP2() != Move.W))
                {
                    dynamiteSuccess++;
                }
                else
                {
                    dynamiteSuccess--;
                }
                if ((gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP1() == Move.R) && (gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP2() == Move.P))
                {
                    successRate++;
                }
                else if ((gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP1() == Move.R) && (gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP2() == Move.S))
                {
                    successRate--;
                }
                if ((gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP1() == Move.P) && (gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP2() == Move.R))
                {
                    successRate++;
                }
                else if ((gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP1() == Move.P) && (gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP2() == Move.S))
                {
                    successRate--;
                }
                if ((gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP1() == Move.S) && (gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP2() == Move.P))
                {
                    successRate++;
                }
                else if ((gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP1() == Move.S) && (gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP2() == Move.R))
                {
                    successRate--;
                }
            }
            catch { }
            if (successRate <= -25)
            {
                offsetDueToFail = true;
            }
            else { offsetDueToFail = false; }

            Random rand = new Random();
            int randomNumber = rand.Next(1, 1235);
            if (groupedDynamiteCounter >= groupedDynamiteTrigger)
            {
                opponentClumpDynamite = true;
            }
            try
            {
                if (gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP2() == Move.R)
                {
                    opponentMoves[2]++;
                    try
                    {
                        if ((gamestate.GetRounds()[gamestate.GetRounds().Length - 2].GetP2() == Move.R) /*&& (gamestate.GetRounds()[gamestate.GetRounds().Length - 3].GetP2() == Move.R)*/)
                        {
                            opponentCurrentClump = 2;
                        }
                        else
                        {
                            opponentCurrentClump = 0;
                        }
                    }
                    catch { }
                }
                else if (gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP2() == Move.P)
                {
                    opponentMoves[3]++;
                    try
                    {
                        if ((gamestate.GetRounds()[gamestate.GetRounds().Length - 2].GetP2() == Move.P) /*&& (gamestate.GetRounds()[gamestate.GetRounds().Length - 3].GetP2() == Move.P)*/)
                        {
                            opponentCurrentClump = 3;
                        }
                        else
                        {
                            opponentCurrentClump = 0;
                        }
                    }
                    catch { }
                }
                else if (gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP2() == Move.S)
                {
                    opponentMoves[4]++;
                    try
                    {
                        if ((gamestate.GetRounds()[gamestate.GetRounds().Length - 2].GetP2() == Move.S) /*&& (gamestate.GetRounds()[gamestate.GetRounds().Length - 3].GetP2() == Move.S)*/)
                        {
                            opponentCurrentClump = 4;
                        }
                        else
                        {
                            opponentCurrentClump = 0;
                        }
                    }
                    catch { }
                }
                else if (gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP2() == Move.D)
                {
                    opponentMoves[0]++;
                    try
                    {
                        if (gamestate.GetRounds()[gamestate.GetRounds().Length - 2].GetP2() == Move.D)
                        {
                            opponentConsecutiveDynamite++;
                        }
                    }
                    catch { }
                }
                else
                {
                    opponentConsecutiveDynamite = 0;
                }
                if (opponentConsecutiveDynamite > 2)
                {
                    groupedDynamiteCounter++;
                }
            }
            catch { }
            try
            {
                if (gamestate.GetRounds()[gamestate.GetRounds().Length - 1].GetP2() == Move.D && opponentMoves[0] < 100 && opponentClumpDynamite && waterSuccess >= -10)
                {
                    return Move.W;
                }
            }
            catch
            { }
            if (usedDynamite < maxDynamite && consecutiveDynamite < maxConsecutiveDynamite && canDynamite)
            {
                if (dynamiteSuccess >= -5)
                {
                    usedDynamite++;
                    consecutiveDynamite++;
                    return Move.D;
                }
            }
            canDynamite = false;

            if (consecutiveDynamite == 0)
            {
                canDynamite = true;
            }
            if (consecutiveDynamite > 0)
            {
                consecutiveDynamite--;
            }
            int mostUsed = 2;
            for (int i = 3; i < 5; i++)
            {
                if (opponentMoves[mostUsed] < opponentMoves[i]) { mostUsed = i; }
            }
            if (offsetDueToFail)
            {
                mostUsed = ((mostUsed + randomNumber) % 3) + 2;
            }
            if (opponentCurrentClump == 2)
            {
                return Move.P;
            }
            else if (opponentCurrentClump == 3)
            {
                return Move.S;
            }
            else if (opponentCurrentClump == 4)
            {
                return Move.R;
            }
            if (mostUsed == 2)
            {
                return Move.P;
            }
            else if (mostUsed == 3)
            {
                return Move.S;
            }
            else
            {
                return Move.R;
            }

        }
    }
}
