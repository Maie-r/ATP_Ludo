using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            for (int i = 0; i < playercount; i++)
            {
                MakePlayerColor(i);
                Console.WriteLine($"Jogador {i + 1}");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nEnter para começar!");
            string debug = Console.ReadLine();
            StartGame(playercount, debug);
        }
        static void StartGame(int playercount, string debug)
        {
            Board board = new Board(playercount, debug);
            LogWriter log = new LogWriter("matchlog.txt");
            bool gameon = true;
            int roundcounter = 1;
            int winner = -1;
            while (gameon) // para o jogo inteiro
            {
                for (int i = 0; i < playercount && gameon; i++) // para cada round
                {
                    log.OnlyWriteLine("--- Jogada " + roundcounter + "---");
                    winner = PlayRound(i, board, log);
                    if (winner >= 0)
                    {
                        gameon = false;
                    }
                    roundcounter++;
                }
            }
            Console.Clear();
            log.WriteLine("Fim de jogo!!");
            log.Write($"\nO vencedor é... ");
            MakePlayerColor(winner);
            log.Write($"Jogador {winner + 1}");
            Console.ForegroundColor = ConsoleColor.White;
            log.WriteLine("! Bom jogo!");
            Console.ReadLine();
        }

        static int PlayRound(int player, Board board, LogWriter log)
        {
            Console.Clear();
            bool reroll = false;
            string[] dicetext = { $"O ", $"jogador {player + 1} ", "lança o dado!"};
            do
            {
                if (reroll)
                {
                    reroll = false;
                }
                Random r = new Random();
                int[] dice = new int[4];
                for (int i = 0; i < 3; i++)
                {
                    dice[i] = r.Next(1, 7);
                }
                int count = 0;
                bool stay = true;
                for (int i =0; i < dicetext.Length; i++)
                {
                    if (i == 1)
                    {
                        MakePlayerColor(player);
                    }
                    log.Write(dicetext[i]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                log.WriteLine("");
                while (stay)
                {
                    if (dice[count] != 0)
                    {
                        log.Write("Resultado: ");
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        log.WriteLine(" " +dice[count].ToString() + " ");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
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
                    log.WriteLine("Passou a vez!");
                }
                else
                {
                    for (int i = 0; i <= count; i++)
                    {
                        if (PawnChoices(board, player, dice[i], log) || PawnWon(board, player, log))
                        {
                            reroll = true;
                            dicetext[2] = "ganha outro dado!";
                            
                        }
                        if (GameOver(board) >= 0)
                        {
                            return player;
                        }
                    }
                }
            } while (reroll);
            log.WriteLine("Fim da jogada!\n");
            Console.ReadLine();
            Console.Clear();
            return -1;
        }

        static bool PawnChoices(Board board, int player, int dice, LogWriter log)
        {
            int temppawn = -1;
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
                    log.Write($"Mover qual peão com o dado {dice}? ");
                    foreach (int i in valid)
                    {
                        if (i != 0)
                        {
                            log.Write($"({i}) ");
                        }
                    }
                    log.WriteLine("");
                    while ((temppawn != valid[0] && temppawn != valid[1] && temppawn != valid[2] && temppawn != valid[3]) || temppawn <= 0 || temppawn >= 5)
                    {
                        try
                        {
                            temppawn = int.Parse(Console.ReadLine());
                            log.OnlyWriteLine(temppawn.ToString());
                        }
                        catch
                        {
                            log.WriteLine("Digite o numero do peão");
                        }
                    }
                }
                if (board.pawns[(temppawn-1), player].pos <= 0)
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
                    log.Write($"Mover qual peão com o dado {dice}? ");
                    foreach (int i in valid)
                    {
                        if (i != 0)
                        {
                            log.Write($"({i}) ");
                        }
                    }
                    log.WriteLine("");
                    while ((temppawn != valid[0] && temppawn != valid[1] && temppawn != valid[2] && temppawn != valid[3]) || temppawn <= 0 || temppawn >= 5)
                    {
                        try
                        {
                            temppawn = int.Parse(Console.ReadLine());
                            log.OnlyWriteLine(temppawn.ToString());
                        }
                        catch
                        {
                            log.WriteLine("Digite o numero do peão");
                        }
                    } 
                }
            }
            board.pawns[temppawn - 1, player].Move(dice);
            if (board.PieceEaten(temppawn, player, log))
            {
                return true;
            }
            return false;
        }

        static bool PawnWon(Board board, int player, LogWriter log)
        {
            for (int i = 0; i < board.pawns.GetLength(0); i++)
            {
                if (board.pawns[i, player].pos == 57)
                {
                    log.WriteLine($"O peão {i + 1} chegou ao final!");
                    board.pawns[i, player].Move(1);
                    return true;
                }
            }
            return false;
        }

        static int GameOver(Board board)
        {
            for (int i = 0; i < board.pawns.GetLength(1); i++)
            {
                int count = 0;
                for (int j = 0; j < board.pawns.GetLength(0); j++)
                {
                    if (board.pawns[j, i].pos == 58)
                    {
                        count++;
                    }
                }
                if (count >= 4)
                {
                    return i;
                }
            }
            return -1;
        }

        public static void MakePlayerColor(int player)
        {
            switch (player)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;

            }
        }

        public static void MakePlayerColorBackground(int player)
        {
            switch (player)
            {
                case 0:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                case 1:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                case 2:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                case 3:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;

            }
        }
    }

    class LogWriter
    {
        string path;
        StreamWriter writer;

        public LogWriter(string path)
        {
            this.path = path;
            writer = new StreamWriter(path, false);
            writer.Write("");
            writer.Close();
        }

        public void WriteLine(string text)
        {
            writer = new StreamWriter(path, true);
            writer.WriteLine(text);
            Console.WriteLine(text);
            writer.Close();
        }

        public void Write(string text)
        {
            writer = new StreamWriter(path, true);
            writer.Write(text);
            Console.Write(text);
            writer.Close();
        }
        public void OnlyWriteLine(string text)
        {
            writer = new StreamWriter(path, true);
            writer.WriteLine(text);
            writer.Close();
        }

    }
}
