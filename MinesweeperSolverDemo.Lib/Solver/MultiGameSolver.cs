using MinesweeperSolverDemo.Lib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolverDemo.Lib.Solver
{
    public class MultiGameSolver
    {
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        public int BombsCount { get; set; }
        public int BoardsCount { get; set; }

        public int GamesCompleted { get; set; }
        public int GamesFailed { get; set; }
        public int GamesUnsolvable { get; set; }

        public MultiGameSolver()
        {
            int height = 0, width = 0, bombs = 0, boards = 0;
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

            while (boards <= 0)
            {
                boards = GetBoards();
                BoardsErrors(boards);
            }

            BoardWidth = width;
            BoardHeight = height;
            BombsCount = bombs;
            BoardsCount = boards;
        }

        public void Run()
        {
            Random rand = new Random();
            Console.WriteLine("Solving Games...");
            for(int i = 0; i < BoardsCount; i++)
            {
                GameBoard board = new GameBoard(BoardWidth, BoardHeight, BombsCount, rand);
                SingleGameSolver solver = new SingleGameSolver(board);
                solver.Solve();

                if(solver.Board.Status == Enums.GameStatus.Completed)
                {
                    GamesCompleted++;
                }
                else if(solver.Board.Status == Enums.GameStatus.Failed)
                {
                    GamesFailed++;
                }
                else if(solver.IsUnsolveable)
                {
                    GamesUnsolvable++;
                }
            }

            Console.WriteLine("Games Completed: " + GamesCompleted.ToString());
            Console.WriteLine("Games Failed: " + GamesFailed.ToString());
            Console.WriteLine("Unsolveable Games: " + GamesUnsolvable.ToString());
        }

        

        private int GetWidth()
        {
            Console.Write("Please enter the width of the boards: ");
            string widthEntered = Console.ReadLine();
            int width;
            bool isValid = int.TryParse(widthEntered, out width);
            if (isValid)
            {
                return width;
            }
            else return -1;
        }

        private int GetHeight()
        {
            Console.Write("Please enter the height of the boards: ");
            string heightEntered = Console.ReadLine();
            int height;
            bool isValid = int.TryParse(heightEntered, out height);
            if (isValid)
            {
                return height;
            }
            else return -1;
        }

        private int GetBombs()
        {
            Console.Write("Please enter the number of bombs on each board: ");
            string bombsEntered = Console.ReadLine();
            int bombs;
            bool isValid = int.TryParse(bombsEntered, out bombs);
            if (isValid)
            {
                return bombs;
            }
            else return -1;
        }

        private int GetBoards()
        {
            Console.Write("Please enter the number of boards to be solved: ");
            string boardsEntered = Console.ReadLine();
            int boards;
            bool isValid = int.TryParse(boardsEntered, out boards);
            if (isValid)
            {
                return boards;
            }
            else return -1;
        }

        private void WidthErrors(int width)
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

        private void HeightErrors(int height)
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

        private void BombsErrors(int bombs)
        {
            if (bombs == 0)
            {
                Console.WriteLine("The number of bombs must be greater than 0.");
            }
            else if (bombs < 0)
            {
                Console.WriteLine("Please enter a valid positive number for the number of bombs on the board.");
            }
        }

        private void BoardsErrors(int boards)
        {
            if (boards == 0)
            {
                Console.WriteLine("The number of boards must be greater than 0.");
            }
            else if (boards < 0)
            {
                Console.WriteLine("Please enter a valid positive number for the number of boards to be solved.");
            }
        }
    }
}
