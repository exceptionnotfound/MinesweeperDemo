using MinesweeperSolverDemo.Lib.Enums;
using MinesweeperSolverDemo.Lib.Objects;
using MinesweeperSolverDemo.Lib.Solver;
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
            char input = 'S';
            while (input != 'Q')
            {
                RunTypeCommands();

                input = Console.ReadLine().ToUpper().First();

                if (input == 'P')
                {
                    PlayGame();
                }
                else if(input == 'M')
                {
                    MultiGameSolver();
                }
            }
        }

        private static void PlayCommands()
        {
            Console.WriteLine("Here are the commands you can enter:");
            Console.WriteLine("A - AutoSolve Board");
            Console.WriteLine("B - Display Board");
            Console.WriteLine("C - Display Commands");
            Console.WriteLine("R - Reveal a Panel");
            Console.WriteLine("N - New Game");
            Console.WriteLine("Q - Quit Game");
            Console.WriteLine("");
            Console.WriteLine("Here's the key for the panels on the game board:");
            Console.WriteLine("U - Hidden");
            Console.WriteLine("# - Number of adjacent panels (including diagonals) that have mines on them.");
            Console.WriteLine("F - A flagged panel");
            Console.WriteLine("X - Mine (don't reveal these!)");
        }

        private static void RunTypeCommands()
        {
            Console.WriteLine("How do you want to play?");
            Console.WriteLine("P - Play Game");
            Console.WriteLine("M - Run Multi-Game Solver");
            Console.WriteLine("Q - Quit");
        }

        private static void PlayGame()
        {
            char input = 'S';
            Random rand = new Random();
            SingleGameSolver solver = null;
            while(input != 'Q')
            {
                if (solver == null)
                {
                    solver = new SingleGameSolver(rand);
                    PlayCommands();
                }
                if (input == 'A')
                {
                    solver.Solve();
                }
                if (input == 'C')
                {
                    PlayCommands();
                }
                if (input == 'B')
                {
                    solver.Board.Display();
                }
                if (input == 'R')
                {
                    if (!solver.Board.Panels.Any(panel => panel.IsRevealed))
                    {
                        solver.FirstMove();
                    }

                    int x = 0, y = 0;
                    while (x <= 0)
                    {
                        //Get Horizontal Coordinate
                        Console.WriteLine("Enter horizontal coordinate:");
                        string xEntered = Console.ReadLine();
                        bool isValid = int.TryParse(xEntered, out x);
                        CoordinateErrors(x);
                    }

                    while (y <= 0)
                    {
                        Console.WriteLine("Enter vertical coordinate:");
                        string yEntered = Console.ReadLine();
                        bool isValid = int.TryParse(yEntered, out y);
                        CoordinateErrors(y);
                    }
                    solver.Board.RevealPanel(x, y);
                    solver.Board.Display();
                    if (solver.Board.Status == Lib.Enums.GameStatus.Failed)
                    {
                        Console.WriteLine("Game Over!");
                    }
                    //Check for board completion
                    if (solver.Board.Status == Lib.Enums.GameStatus.Completed)
                    {
                        Console.WriteLine("CONGRATULATIONS!");
                    }
                }

                input = Console.ReadLine().ToUpper().First();

                if (input == 'N')
                {
                    solver = null;
                }
            }
        }

        private static void CoordinateErrors(int coord)
        {
            if (coord == 0)
            {
                Console.WriteLine("Please enter a value greater than 0.");
            }
            else if (coord < 0)
            {
                Console.WriteLine("Please enter a valid positive integer.");
            }
        }

        private static void MultiGameSolver()
        {
            MultiGameSolver solver = new MultiGameSolver();
            solver.Run();
        }
    }
}
