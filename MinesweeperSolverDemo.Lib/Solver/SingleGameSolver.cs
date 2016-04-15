using MinesweeperSolverDemo.Lib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolverDemo.Lib.Solver
{
    public class SingleGameSolver : GameSolver
    {
        public GameBoard Board { get; set; }

        public Random Random { get; set; }

        public SingleGameSolver(Random rand)
        {
            Random = rand;
            int height = 0, width = 0, mines = 0;
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

            Board = new GameBoard(width, height, mines);
        }

        public SingleGameSolver(GameBoard board, Random rand)
        {
            Board = board;
            Random = rand;
        }

        public BoardStats Solve()
        {
            while (Board.Status == Enums.GameStatus.InProgress)
            {
                if (!Board.Panels.Any(x=>x.IsRevealed))
                {
                    FirstMove();
                }
                FlagObviousMines();
                Board.Display();

                if (HasAvailableMoves())
                {
                    ObviousNumbers();
                }
                else
                {
                    RandomMove();
                }

                Endgame();

                Board.Display();
            }

            if(Board.Status == Enums.GameStatus.Failed)
            {
                Board.DisplayFinal();
                Console.WriteLine("Solver Failed!");
            }
            else if (Board.Status == Enums.GameStatus.Completed)
            {
                Console.WriteLine("Solver SUCCESS");
            }

            return Board.GetStats();
        }

        public void FirstMove()
        {
            var randomX = Random.Next(1, Board.Width - 1);
            var randomY = Random.Next(1, Board.Height - 1);

            Board.FirstMove(randomX, randomY, Random);
            Board.RevealPanel(randomX, randomY);
        }

        public void RandomMove()
        {
            var randomID = Random.Next(1, Board.Panels.Count);
            var panel = Board.Panels.First(x => x.ID == randomID);
            while(panel.IsRevealed || panel.IsFlagged)
            {
                randomID = Random.Next(1, Board.Panels.Count);
                panel = Board.Panels.First(x => x.ID == randomID);
            }

            Board.RevealPanel(panel.X, panel.Y);
        }

        public bool HasAvailableMoves()
        {
            //Find any numbered panel where the number of flags around it equals its number, then click on every square around that.
            var numberedPanels = Board.Panels.Where(x => x.IsRevealed && x.AdjacentMines > 0);
            foreach (var numberPanel in numberedPanels)
            {
                var neighborPanels = Board.GetNeighbors(numberPanel.X, numberPanel.Y);
                var flaggedNeighbors = neighborPanels.Where(x => x.IsFlagged);
                if (flaggedNeighbors.Count() == numberPanel.AdjacentMines && neighborPanels.Any(x => !x.IsRevealed && !x.IsFlagged))
                {
                    return true;
                }
            }

            return false;
        }

        public void ObviousNumbers()
        {
            var numberedPanels = Board.Panels.Where(x => x.IsRevealed && x.AdjacentMines > 0);
            foreach(var numberPanel in numberedPanels)
            {
                //Foreach number panel
                var neighborPanels = Board.GetNeighbors(numberPanel.X, numberPanel.Y);

                //Get all of that panel's flagged neighbors
                var flaggedNeighbors = neighborPanels.Where(x => x.IsFlagged);

                //If the number of flagged neighbors equals the number in the current panel...
                if(flaggedNeighbors.Count() == numberPanel.AdjacentMines)
                {
                    //All hidden neighbors must *not* have mines in them, so reveal them.
                    foreach(var hiddenPanel in neighborPanels.Where(x=>!x.IsRevealed && !x.IsFlagged))
                    {
                        Board.RevealPanel(hiddenPanel.X, hiddenPanel.Y);
                    }
                }
            }
        }

        public void FlagObviousMines()
        {
            var numberPanels = Board.Panels.Where(x => x.IsRevealed && x.AdjacentMines > 0);
            foreach(var panel in numberPanels)
            {
                //For each revealed number panel on the board, get its neighbors.
                var neighborPanels = Board.GetNeighbors(panel.X, panel.Y);

                //If the total number of hidden panels == the number of mines revealed by this panel...
                if(neighborPanels.Count(x=>!x.IsRevealed) == panel.AdjacentMines)
                {
                    //All those adjacent hidden panels must be mines, so flag them.
                    foreach(var neighbor in neighborPanels.Where(x=>!x.IsRevealed))
                    {
                        Board.FlagPanel(neighbor.X, neighbor.Y);
                    }
                }
            }
        }

        public void Endgame()
        {
            //Count all the flagged panels.  If the number of flagged panels == the number of mines on the board, reveal all non-flagged panels.
            var flaggedPanels = Board.Panels.Where(x => x.IsFlagged).Count();
            if(flaggedPanels == Board.MineCount)
            {
                //Reveal all hidden, unflagged panels
                var hiddenPanels = Board.Panels.Where(x => !x.IsFlagged && !x.IsRevealed);
                foreach(var panel in hiddenPanels)
                {
                    Board.RevealPanel(panel.X, panel.Y);
                }
            }
        }
    }
}
