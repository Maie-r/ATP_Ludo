using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame
{
    class Board
    {
        public Pawn[,] pawns;
        public Board(int playercount, string debug) {
            pawns = new Pawn[4, playercount];
            for (int i = 0; i < playercount; i++)
            {
                for (int j = 0; j < pawns.GetLength(0); j++)
                {
                    pawns[j, i] = new Pawn(j, i);
                }
            }
            if (debug == "debug") //
            {
                // set up for capture
                pawns[0, 0].Move(9);
                pawns[1, 0].Move(8);
                pawns[2, 0].Move(7);
                pawns[3, 0].Move(6);
                pawns[0, 1].Move(36);
                pawns[1, 1].Move(36);
                pawns[2, 1].Move(35);
                pawns[3, 1].Move(37);
            }
        }

        public void MovePawn(Pawn pawn, int amount)
        {
            pawn.Move(amount);
        }

        public void BoardStatus(LogWriter log)
        {
            for (int i = 0; i < pawns.GetLength(0); i++)
            {
                for (int j = 0; j < pawns.GetLength(1); j++)
                {
                    log.Write(pawns[i, j].pos + " ");
                }
                log.WriteLine("");
            }
        }

        public bool PieceEaten(int lastmoved, int player, LogWriter log)
        {
            BoardStatus(log);
            int pos = pawns[lastmoved - 1, player].pos;
            switch (player)
            {
                case 0:
                    int[] offsets1 = {0, 26, 13, 39, 0, -26, 39, 13};
                    if (CheckOverlap(player, pos, offsets1, log))
                    {
                        return true;
                    }
                    break;
                case 1:
                    int[] offsets2 = { 26, 0, 39, 13, -26, 0, 13, 39};
                    if (CheckOverlap(player, pos, offsets2, log))
                    {
                        return true;
                    }
                    break;
                case 2:
                    int[] offsets3 = { 39, 13, 0, 26, 13, 39, 0, -26};
                    if (CheckOverlap(player, pos, offsets3, log))
                    {
                        return true;
                    }
                    break;
                case 3:
                    int[] offsets4 = { 13, 39, 26, 0, 39, 13, -26, 0};
                    if (CheckOverlap(player, pos, offsets4, log))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        bool CheckOverlap(int player, int pos, int[] offsets, LogWriter log)
        {
            int failsafe = 0;
            bool result = false;
            if (player == 0)
            {
                failsafe++;
            }
            for (int i = 0 + failsafe; i < pawns.GetLength(1); i++)
            {
                for (int j = 0; j < pawns.GetLength(0); j++)
                {
                    if (pos <= 1 || pos == 9 || pos == 14 || pos == 22 || pos == 27 || pos == 35 || pos == 40 || pos == 48 || pos >= 53)
                    {
                        Console.WriteLine("Protegido!");
                        return false;
                    }
                    else
                    {
                        if (pos > pawns[j, i].pos)
                        {
                            if (pos == (pawns[j, i].pos + offsets[i]))
                            {
                                log.WriteLine($"Comeu o peão {pawns[j, i].id + 1} do jogador {pawns[j, i].team + 1}!");
                                pawns[j, i].Return();
                                result = true;
                            }
                        }
                        else
                        {
                            if (pos == (pawns[j, i].pos + offsets[i+4]))
                            {
                                log.WriteLine($"Comeu o peão {pawns[j, i].id + 1} do jogador {pawns[j, i].team + 1}!");
                                pawns[j, i].Return();
                                result = true;
                            }
                        }
                        
                    }
                }
                if ((i + 1) == player)
                {
                    i++;
                }
            }
            return result;
        }
    }
}
