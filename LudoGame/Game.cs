using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame
{
    internal class Game
    {
        static void Main(string[] args)
        {
            int playercount = 0;
            while (playercount !=2 && playercount != 4)
            {
                Console.Clear();
                Console.WriteLine("Bem vindo ao Ludo!");
                Console.WriteLine("\nQuantos Jogadores? (2 ou 4)");
                try
                {
                    playercount = int.Parse(Console.ReadLine());
                }
                catch
                {

                }
            }
            Console.WriteLine("\nEnter para começar!");
            string debug = Console.ReadLine();
            StartGame(playercount, debug);
        }
        static void StartGame(int playercount, string debug)
        {
            Board board = new Board(playercount, debug);
            bool gameon = true;
            while (gameon) // para o jogo inteiro
            {
                for (int i = 0; i < playercount; i++) // para cada round
                {
                    PlayRound(i, board);
                }
            }
        }

        static void PlayRound(int player, Board board)
        {
            Console.Clear();
            bool reroll = false;
            do
            {
                Random r = new Random();
                int[] dice = new int[4];
                for (int i = 0; i < 3; i++)
                {
                    dice[i] = r.Next(1, 7);
                }
                int count = 0;
                bool stay = true;
                Console.WriteLine($"O jogador {player + 1} lança o dado!");
                while (stay)
                {
                    if (dice[count] != 0)
                    {
                        Console.WriteLine("Resultado: " + dice[count]);
                    }
                    if (dice[count] == 6 && count < 3)
                    {
                        count++;
                    }
                    else
                    {
                        stay = false;
                    }
                }
                if (count >= 3)
                {
                    Console.WriteLine("Passou a vez!");
                }
                else
                {
                    for (int i = 0; i <= count; i++)
                    {
                        if (PawnChoices(board, player, dice[i]))
                        {
                            reroll = true;
                            Console.WriteLine($"O jogador {player + 1} ganha outro dado!");
                        }
                        else
                        {
                            reroll = false;
                        }
                    }
                }
            } while (reroll);
            Console.WriteLine("Fim da jogada!");
            Console.ReadLine();
            Console.Clear();
        }

        static bool PawnChoices(Board board, int player, int dice)
        {
            int temppawn;
            int[] valid = new int[4];
            if (dice >= 6)
            {
                for (int i = 0; i < board.pawns.GetLength(0); i++)
                {
                    if ((board.pawns[i, player].pos + dice) <= 57)
                    {
                        valid[i] = i + 1;
                    }
                }
                if (valid[0] == 0 && valid[1] == 0 && valid[2] == 0 && valid[3] == 0)
                { 
                    return false; 
                }
                else
                {
                    Console.Write($"Mover qual peão com o dado {dice}? ");
                    foreach (int i in valid)
                    {
                        if (i != 0)
                        {
                            Console.Write($"({i}) ");
                        }
                    }
                    Console.WriteLine();
                    do
                    {
                        try
                        {
                            temppawn = int.Parse(Console.ReadLine());
                        }
                        catch
                        {
                            Console.WriteLine("Digite o numero do peão");
                            temppawn = int.Parse(Console.ReadLine());
                        }
                    } while ((temppawn != valid[0] && temppawn != valid[1] && temppawn != valid[2] && temppawn != valid[3]) || temppawn <= 0 || temppawn >= 5) ;
                }
                if (board.pawns[(temppawn-1), player].pos >= 0)
                {
                    dice -= 5;
                }
            }
            else
            {
                for (int i = 0; i < board.pawns.GetLength(0); i++)
                {
                    if (board.pawns[i, player].pos > 0 && (board.pawns[i, player].pos + dice) <= 57)
                    {
                        valid[i] = i + 1;
                        
                    }
                }
                if (valid[0] == 0 && valid[1] == 0 && valid[2] == 0 && valid[3] == 0)
                { 
                    return false; 
                }
                else
                {
                    Console.Write($"Mover qual peão com o dado {dice}? ");
                    foreach (int i in valid)
                    {
                        if (i != 0)
                        {
                            Console.Write($"({i}) ");
                        }
                    }
                    Console.WriteLine();
                    do
                    {
                        try
                        {
                            temppawn = int.Parse(Console.ReadLine());
                        }
                        catch
                        {
                            Console.WriteLine("Digite o numero do peão");
                            temppawn = int.Parse(Console.ReadLine());
                        }
                    } while ((temppawn != valid[0] && temppawn != valid[1] && temppawn != valid[2] && temppawn != valid[3]) || temppawn <= 0 || temppawn >= 5);
                }
            }
            board.MovePawn(board.pawns[(temppawn - 1), player], dice);
            if (board.PieceEaten(temppawn, player))
            {
                return true;
            }
            return false;
        }
    }
}
