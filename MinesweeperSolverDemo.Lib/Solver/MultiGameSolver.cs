using MinesweeperSolverDemo.Lib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolverDemo.Lib.Solver
{
    public class MultiGameSolver : GameSolver
    {
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        public int MinesCount { get; set; }
        public int BoardsCount { get; set; }

        public int GamesCompleted { get; set; }
        public int GamesFailed { get; set; }

        public MultiGameSolver()
        {
            int height = 0, width = 0, mines = 0, boards = 0;
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

            while (mines <= 0)
            {
                mines = GetMines();
                MinesErrors(mines);
            }

            while (boards <= 0)
            {
                boards = GetBoards();
                BoardsErrors(boards);
            }

            BoardWidth = width;
            BoardHeight = height;
            MinesCount = mines;
            BoardsCount = boards;
        }

        public void Run()
        {
            Random rand = new Random();
            List<BoardStats> stats = new List<BoardStats>();
            Console.WriteLine("Solving Games...");
            for(int i = 0; i < BoardsCount; i++)
            {
                GameBoard board = new GameBoard(BoardWidth, BoardHeight, MinesCount);
                SingleGameSolver solver = new SingleGameSolver(board, rand);
                var boardStats = solver.Solve();
                stats.Add(boardStats);

                if(solver.Board.Status == Enums.GameStatus.Completed)
                {
                    GamesCompleted++;
                }
                else if(solver.Board.Status == Enums.GameStatus.Failed)
                {
                    GamesFailed++;
                }
            }

            Console.WriteLine("Games Completed: " + GamesCompleted.ToString());
            Console.WriteLine("Games Failed: " + GamesFailed.ToString());

            //Calculate stats
            var totalMines = stats.Sum(x => x.Mines);
            var totalFlaggedMines = stats.Sum(x => x.FlaggedMinePanels);
            var totalFlaggedMinesPercent = Math.Round(((totalFlaggedMines / totalMines) * 100F), 2);
            Console.WriteLine("Mines Flagged: " + totalFlaggedMinesPercent.ToString() + "%");

            var totalPanels = stats.Sum(x => x.TotalPanels);
            var revealedPanels = stats.Sum(x => x.PanelsRevealed);
            var totalRevealedPanelsPercent = Math.Round((revealedPanels / totalPanels) * 100F, 2);
            Console.WriteLine("Panels Revealed: " + totalRevealedPanelsPercent.ToString() + "%");
        }
    }
}
