using System;
using System.IO;
using System.Threading;

namespace Tic_Tac_Toe_Last
{
    internal class Program
    {
        static char[,] tboard = new char[3, 3];
        static char _human;
        static char _ai;
        static int _humanScore = 0;
        static int _aiScore = 0;
        static int _drawScore = 0;
        static int _round = 1;
        static void Main(string[] args)
        {
            TicTacToeStart();
        }
        static void TicTacToeStart()
        {
            
            Console.Write("X ? O: ");
            _human = ValidateInput(char.Parse(Console.ReadLine().ToUpper()));
            _ai = (_human == 'X') ? 'O' : 'X';

            while (_humanScore < 3 && _aiScore < 3)
            {
                InitializeBoard();
                if (_human == 'X')
                {
                    isHumanTurn();
                    GameHistory();
                }
                else
                {
                    isAiTurn();
                    GameHistory();
                }

                if (ifWinning(_human)) // count human win
                { 
                    _humanScore++;
                }
                else if (ifWinning(_ai)) // count ai win
                {
                    _aiScore++;
                }
                else // count draw
                {
                    _drawScore++;
                }
                Console.WriteLine("\n");
                Console.WriteLine($"Player - {_humanScore}    CPU - {_aiScore}    Draw - {_drawScore}");
                _round++;
            }
            if (_humanScore == 3)
            {
                Console.WriteLine("Congratulations! You have won the game!");
            }
            else if (_aiScore == 3)
            {
                Console.WriteLine("CPU has won! Better luck next time!");
            }
            GameHistory();
        }
        static char ValidateInput(char symbol)
        {
            Console.Clear();
            if (symbol != 'X' && symbol != 'O')
            {
                Console.WriteLine("Must choose between X and O only!");
                Console.Write("X or O: ");
                symbol = char.Parse(Console.ReadLine().ToUpper());
                return ValidateInput(symbol);
            }
            return symbol;
        }
        static void InitializeBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tboard[i, j] = ' ';
                }
            }
        }
        static void DisplayBoard()
        {
            Console.Clear();
            Console.WriteLine($"Player - {_humanScore}    CPU - {_aiScore}    Draw - {_drawScore}");
            Console.WriteLine("\t     |     |               |     |     ");
            Console.WriteLine($"\t  {tboard[0, 0]}  |  {tboard[0, 1]}  |  {tboard[0, 2]}        0 0 | 0 1 | 0 2 ");
            Console.WriteLine("\t_____|_____|_____     _____|_____|_____");
            Console.WriteLine("\t     |     |               |     |     ");
            Console.WriteLine($"\t  {tboard[1, 0]}  |  {tboard[1, 1]}  |  {tboard[1, 2]}        1 0 | 1 1 | 1 2 ");
            Console.WriteLine("\t_____|_____|_____     _____|_____|_____");
            Console.WriteLine("\t     |     |               |     |     ");
            Console.WriteLine($"\t  {tboard[2, 0]}  |  {tboard[2, 1]}  |  {tboard[2, 2]}        2 0 | 2 1 | 2 2 ");
            Console.WriteLine("\t     |     |               |     |     ");
            Console.WriteLine("\n");
        }
        static void isHumanTurn()
        {
            DisplayBoard();
            bool humanTurn = (_human == 'X');
            while (true)
            {
                Console.WriteLine($"Round {_round}");
                if (humanTurn)
                {
                    HumanMove();
                    DisplayBoard();
                    if (ifWinning(_human))
                    {
                        break;
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                    AiMove();
                    DisplayBoard();
                    if (ifWinning(_ai))
                    {
                        break;
                    }
                }

                if (isFull())
                {
                    break;
                }
                humanTurn = !humanTurn; // Switch turns
            }
        }
        static void isAiTurn()
        {
            DisplayBoard();
            bool aiTurn = (_ai == 'X');

            while (true)
            {
                Console.WriteLine($"Round {_round}");
                if (aiTurn)
                {       
                    Thread.Sleep(2000);
                    AiMove();
                    DisplayBoard();
                    if (ifWinning(_ai))
                    {
                        break;
                    }
                }
                else
                {
                    HumanMove();
                    DisplayBoard();
                    if (ifWinning(_human))
                    {
                        break;
                    }
                }

                if (isFull())
                {
                    break;
                }
                aiTurn = !aiTurn; // Switch turns
            }
        }
        static void HumanMove()
        {
            int x, y;
            do
            {
                Console.WriteLine("\n");
                Console.Write("Enter your moves (row column): ");
                string[] move = Console.ReadLine().Split();
                x = int.Parse(move[0]);
                y = int.Parse(move[1]);
            } while (!CheckMove(x, y));

            tboard[x, y] = _human;
        }
        static void AiMove()
        {
            int bestMove = int.MinValue;
            int bestX = -1;
            int bestY = -1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (CheckMove(i, j))
                    {
                        tboard[i, j] = _ai;
                        int move = Minimax(tboard, 0, false); 
                        tboard[i, j] = ' ';

                        if (move > bestMove)
                        {
                            bestMove = move;
                            bestX = i;
                            bestY = j;
                        }
                    }
                }
            }

            tboard[bestX, bestY] = _ai;
        }
        static int Minimax(char[,] tboard, int depth, bool isMaximizing)
        {
            if (ifWinning(_human))
            {
                return -1;
            }

            if (ifWinning(_ai))
            {
                return 1;
            }

            if (isFull())
            {
                return 0;
            }

            if (isMaximizing)
            {
                int bestScore = int.MinValue;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (tboard[i, j] == ' ')
                        {
                            tboard[i, j] = _ai;
                            int score = Minimax(tboard, depth + 1, false);
                            tboard[i, j] = ' ';
                            bestScore = Math.Max(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (tboard[i, j] == ' ')
                        {
                            tboard[i, j] = _human;
                            int score = Minimax(tboard, depth + 1, true);
                            tboard[i, j] = ' ';
                            bestScore = Math.Min(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
        }
        static bool isFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (tboard[i, j] == ' ')
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        static bool CheckMove(int x, int y)
        {
            if (x < 0 || x > 2 || y < 0 || y > 2)
            {
                Console.WriteLine($"Move {x} {y} should be from 0 - 2 only.");
                return false;
            }
            else if (tboard[x, y] != ' ')
            {
                Console.WriteLine($"Cell {x}, {y} is already occupied.");
                return false;
            }
            return true;
        }
        static bool ifWinning(char player)
        {
            // Check for winning rows and columns
            for (int c = 0; c < 3; c++)
            {
                if (tboard[c, 0] == player && tboard[c, 1] == player && tboard[c, 2] == player)
                    return true;
                if (tboard[0, c] == player && tboard[1, c] == player && tboard[2, c] == player)
                    return true;
            }

            if (tboard[0, 0] == player && tboard[1, 1] == player && tboard[2, 2] == player)
                return true;

            if (tboard[0, 2] == player && tboard[1, 1] == player && tboard[2, 0] == player)
                return true;

            // Continue the game
            return false;
        }
        static void GameHistory()
        {
            string outputfile = "Gamehistory.txt";
            using (StreamWriter GameHistory = new StreamWriter(outputfile))
            {
                GameHistory.WriteLine($"Player - {_humanScore}    CPU - {_aiScore}    Draw - {_drawScore}");
                GameHistory.WriteLine("\t     |     |               |     |     ");
                GameHistory.WriteLine($"\t  {tboard[0, 0]}  |  {tboard[0, 1]}  |  {tboard[0, 2]}        0 0 | 0 1 | 0 2 ");
                GameHistory.WriteLine("\t_____|_____|_____     _____|_____|_____");
                GameHistory.WriteLine("\t     |     |               |     |     ");
                GameHistory.WriteLine($"\t  {tboard[1, 0]}  |  {tboard[1, 1]}  |  {tboard[1, 2]}        1 0 | 1 1 | 1 2 ");
                GameHistory.WriteLine("\t_____|_____|_____     _____|_____|_____");
                GameHistory.WriteLine("\t     |     |               |     |     ");
                GameHistory.WriteLine($"\t  {tboard[2, 0]}  |  {tboard[2, 1]}  |  {tboard[2, 2]}        2 0 | 2 1 | 2 2 ");
                GameHistory.WriteLine("\t     |     |               |     |     ");
                GameHistory.WriteLine("\n");
            }
        }
    }
}