using MinesweeperSolverDemo.Lib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolverDemo.Helpers
{
    public static class BoardHelpers
    {
        public static GameBoard SetupBoard()
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

            return board;
        }

        public static int GetWidth()
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

        public static void WidthErrors(int width)
        {
            if (width == 0)
            {
                Console.WriteLine("The width of the board must be greater than 0.");
            }
            else if (width < 0)
            {
                Console.WriteLine("Please enter a valid positive number for the width.");
            }
        }

        public static int GetHeight()
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

        public static void HeightErrors(int height)
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

        public static int GetBombs()
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

        public static void BombsErrors(int bombs)
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

        public static void Display(GameBoard board)
        {
            Console.WriteLine("Here's your game board!");
            board.Display();
        }
    }
}
