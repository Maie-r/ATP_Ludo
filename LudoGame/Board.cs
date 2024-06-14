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
                Console.WriteLine();
            }
            if (debug == "debug")
            {
                // set up for capture
                pawns[0, 0].Move(12);
                pawns[1, 0].Move(11);
                pawns[2, 0].Move(10);
                pawns[3, 0].Move(9);
                pawns[0, 1].Move(1);
                pawns[1, 1].Move(2);
                pawns[2, 1].Move(3);
                pawns[3, 1].Move(4);
            }
        }

        public void MovePawn(Pawn pawn, int amount)
        {
            pawn.Move(amount);
        }

        public void BoardStatus()
        {
            for (int i = 0; i < pawns.GetLength(0); i++)
            {
                for (int j = 0; j < pawns.GetLength(1); j++)
                {
                    //Console.Write(GameState[i, j] + " ");
                    Console.Write(pawns[i, j].pos + " ");
                }
                Console.WriteLine();
            }
        }

        public bool PieceEaten(int lastmoved, int player)
        {
            BoardStatus();
            int pos = pawns[lastmoved - 1, player].pos;
            switch (player)
            {
                case 0:
                    int[] offsets1 = {0, 13, 26, 39};
                    if (CheckOverlap(player, pos, offsets1))
                    {
                        Console.WriteLine("Peça comida!");
                        return true;
                    }
                    break;
                case 1:
                    int[] offsets2 = { 39, 0, 13, 26  };
                    if (CheckOverlap(player, pos, offsets2))
                    {
                        Console.WriteLine("Peça comida!");
                        return true;
                    }
                    break;
                case 2:
                    int[] offsets3 = { 26, 36, 0, 13 };
                    if (CheckOverlap(player, pos, offsets3))
                    {
                        Console.WriteLine("Peça comida!");
                        return true;
                    }
                    break;
                case 3:
                    int[] offsets4 = { 13, 26, 36, 0 };
                    if (CheckOverlap(player, pos, offsets4))
                    {
                        Console.WriteLine("Peça comida!");
                        return true;
                    }
                    break;
            }
            return false;
        }

        bool CheckOverlap(int player, int pos, int[] offsets)
        {
            int failsafe = 0; // TO DO: CHECK FOR PROTECTED AREAS
            if (player == 0)
            {
                failsafe++;
            }
            for (int i = 0 + failsafe; i < pawns.GetLength(1); i++)
            {
                for (int j = 0; j < pawns.GetLength(0); j++)
                {
                    if (pos == (pawns[j, i].pos + offsets[i]))
                    {
                        pawns[j, i].Return();
                        return true;
                    }
                }
                if ((i + 1) == player)
                {
                    i++;
                }
            }
            return false;
        }
    }
}
