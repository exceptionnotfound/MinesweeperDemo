using MinesweeperSolverDemo.Helpers;
using MinesweeperSolverDemo.Lib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolverDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            GameBoard board = null;
            char input = 'S';
            while (input != 'Q')
            {
                if (board == null)
                {
                    board = BoardHelpers.SetupBoard();
                    BoardHelpers.Display(board);
                    Commands();
                }

                if(input == 'C')
                {
                    Commands();
                }
                if(input == 'B')
                {
                    BoardHelpers.Display(board);
                }
                if(input == 'R')
                {
                    var coordinate = PanelHelpers.GetPanelCoordinate();
                    board.RevealPanel(coordinate);
                    BoardHelpers.Display(board);
                    if(board.Status == Lib.Enums.GameStatus.Failed)
                    {
                        Console.WriteLine("Game Over!");
                    }
                    //Check for board completion
                    if (board.Status == Lib.Enums.GameStatus.Completed)
                    {
                        Console.WriteLine("CONGRATULATIONS!");
                    }
                }

                input = Console.ReadLine().ToUpper().First();

                if (input == 'N')
                {
                    board = null;
                }
            }
        }

        private static void Commands()
        {
            Console.WriteLine("Here are the commands you can enter:");
            Console.WriteLine("B - Display Board");
            Console.WriteLine("C - Display Commands");
            Console.WriteLine("R - Reveal a Panel");
            Console.WriteLine("N - New Game");
            Console.WriteLine("");
            Console.WriteLine("Here's the key for the panels on the game board:");
            Console.WriteLine("U - Unrevealed");
            Console.WriteLine("# - Number of adjacent panels (including diagonals) that have bombs on them.");
            Console.WriteLine("F - A flagged panel");
            Console.WriteLine("X - Bomb (don't reveal these!)");
        }
    }
}
