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
                    board = SetupBoard();
                }

                if(input == 'C')
                {
                    Commands();
                }
                if(input == 'B')
                {
                    DisplayBoard(board);
                }
                if(input == 'R')
                {
                    bool isStillPlaying = RevealPanel(board);
                    DisplayBoard(board);
                    if(!isStillPlaying)
                    {
                        Console.WriteLine("Game Over!");
                    }
                    //Check for board completion
                    if (board.IsCompleted())
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

        private static GameBoard SetupBoard()
        {
            int height = 0, width = 0, bombs = 0;
            while (width <= 0)
            {
                width = GetWidth();
                WidthErrors(width);
            }

            while (height <= 0)
            {
                height = GetHeight();
                HeightErrors(height);
            }

            while (bombs <= 0)
            {
                bombs = GetBombs();
                BombsErrors(bombs);
            }

            var board = new GameBoard(width, height, bombs);

            DisplayBoard(board);
            Commands();

            return board;
        }

        private static int GetWidth()
        {
            Console.Write("Please enter the width of the board: ");
            string widthEntered = Console.ReadLine();
            int width;
            bool isValid = int.TryParse(widthEntered, out width);
            if (isValid)
            {
                return width;
            }
            else return -1;
        }

        private static void WidthErrors(int width)
        {
            if(width == 0)
            {
                Console.WriteLine("The width of the board must be greater than 0.");
            }
            else if (width < 0)
            {
                Console.WriteLine("Please enter a valid positive number for the width.");
            }
        }

        private static int GetHeight()
        {
            Console.Write("Please enter the height of the board: ");
            string heightEntered = Console.ReadLine();
            int height;
            bool isValid = int.TryParse(heightEntered, out height);
            if (isValid)
            {
                return height;
            }
            else return -1;
        }

        private static void HeightErrors(int height)
        {
            if (height == 0)
            {
                Console.WriteLine("The height of the board must be greater than 0.");
            }
            else if (height < 0)
            {
                Console.WriteLine("Please enter a valid positive number for the height.");
            }
        }

        private static int GetBombs()
        {
            Console.Write("Please enter the number of bombs on the board: ");
            string bombsEntered = Console.ReadLine();
            int bombs;
            bool isValid = int.TryParse(bombsEntered, out bombs);
            if (isValid)
            {
                return bombs;
            }
            else return -1;
        }

        private static void BombsErrors(int bombs)
        {
            if (bombs == 0)
            {
                Console.WriteLine("The nunmber of bombs must be greater than 0.");
            }
            else if (bombs < 0)
            {
                Console.WriteLine("Please enter a valid positive number for the number of bombs on the board.");
            }
        }

        private static void Commands()
        {
            Console.WriteLine("Here are the commands you can enter:");
            Console.WriteLine("B - Display Board");
            Console.WriteLine("C - Display Commands");
            Console.WriteLine("R - Reveal a Panel");
            Console.WriteLine("F - Flag a Panel");
            Console.WriteLine("N - New Game");
            Console.WriteLine("");
            Console.WriteLine("Here's the key for the panels on the game board:");
            Console.WriteLine("U - Unrevealed");
            Console.WriteLine("# - Number of adjacent panels (including diagonals) that have bombs on them.");
            Console.WriteLine("F - A flagged panel");
            Console.WriteLine("X - Bomb (don't reveal these!)");
        }

        private static void DisplayBoard(GameBoard board)
        {
            Console.WriteLine("Here's your game board!");
            board.Display();
        }

        private static bool RevealPanel(GameBoard board)
        {
            int x = 0, y = 0;
            while(x <= 0)
            {
                //Get Horizontal Coordinate
                Console.WriteLine("Enter horizontal coordinate:");
                string xEntered = Console.ReadLine();
                bool isValid = int.TryParse(xEntered, out x);
                CoordinateErrors(x);
            }

            while(y <= 0)
            {
                Console.WriteLine("Enter vertical coordinate:");
                string yEntered = Console.ReadLine();
                bool isValid = int.TryParse(yEntered, out y);
                CoordinateErrors(x);
            }

            var panel = board.Panels.Where(pane => pane.Coordinate.Latitude == (x - 1) && pane.Coordinate.Longitude == (y - 1)).First();
            panel.IsRevealed = true;
            if (panel.IsBomb) return false; //Game over!
            if(panel.NearbyBombs == 0)
            {
                RevealZeros(board, x, y);
            }
            return true;
            
        }

        private static void RevealZeros(GameBoard board, int x, int y)
        {
            var neighborPanels = board.GetNearbyPanels(x, y).Where(panel=>!panel.IsRevealed);
            foreach(var panel in neighborPanels)
            {
                panel.IsRevealed = true;
                if(panel.NearbyBombs == 0)
                {
                    RevealZeros(board, panel.Coordinate.Latitude, panel.Coordinate.Longitude);
                }
            }
        }

        private static void CoordinateErrors(int coord)
        {
            if(coord == 0)
            {
                Console.WriteLine("Please enter a value greater than 0.");
            }
            else if (coord < 0)
            {
                Console.WriteLine("Please enter a valid positive integer.");
            }
        }
    }
}
