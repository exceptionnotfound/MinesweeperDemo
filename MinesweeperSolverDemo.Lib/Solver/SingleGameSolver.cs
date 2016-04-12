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

        public bool IsUnsolveable { get; set; }

        public bool UseRandomGuesses { get; set; }

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
                    NextMove();
                    PairsMoves();
                }
                else
                {
                    if (UseRandomGuesses)
                    {
                        Console.WriteLine("RANDOM MOVE!");
                        RandomMove();
                        
                    }
                    else
                    {
                        IsUnsolveable = true;
                        break;
                    }
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
            else if(IsUnsolveable)
            {
                Board.Display();
                Console.WriteLine("Game is UNSOLVEABLE");
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
            var numberedPanels = Board.Panels.Where(x => x.IsRevealed && x.NearbyMines > 0);
            foreach (var numberPanel in numberedPanels)
            {
                var neighborPanels = Board.GetNearbyPanels(numberPanel.X, numberPanel.Y);
                var flaggedNeighbors = neighborPanels.Where(x => x.IsFlagged);
                if (flaggedNeighbors.Count() == numberPanel.NearbyMines && neighborPanels.Any(x => !x.IsRevealed && !x.IsFlagged))
                {
                    return true;
                }

                var neighborNumberPanels = neighborPanels.Where(x => x.NearbyMines > 0);
                foreach (var neighbor in neighborNumberPanels)
                {
                    //Find each unopened neighbor of both the original panel and the current neighbor panel
                    var nextDoorPanels = Board.GetNearbyPanels(neighbor.X, neighbor.Y).Where(x => !x.IsRevealed);
                    var commonNeighbors = nextDoorPanels.Intersect(neighborNumberPanels.Where(x => !x.IsRevealed));
                    var uniqueNeighbors = nextDoorPanels.Except(commonNeighbors);
                    if (neighbor.NearbyMines == numberPanel.NearbyMines && commonNeighbors.Where(x => x.IsFlagged).Count() == neighbor.NearbyMines)
                    {
                        foreach (var common in commonNeighbors)
                        {
                            Board.RevealPanel(common.X, common.Y);
                        }
                    }
                }
            }

            return false;
        }

        public void NextMove()
        {
            //Find any numbered panel where the number of flags around it equals its number, then click on every neighboring unrevealed panel.
            var numberedPanels = Board.Panels.Where(x => x.IsRevealed && x.NearbyMines > 0);
            foreach(var numberPanel in numberedPanels)
            {
                var neighborPanels = Board.GetNearbyPanels(numberPanel.X, numberPanel.Y);
                var flaggedNeighbors = neighborPanels.Where(x => x.IsFlagged);
                if(flaggedNeighbors.Count() == numberPanel.NearbyMines)
                {
                    //Reveal all unrevealed, unflagged neighbor panels
                    foreach(var unrevealedPanel in neighborPanels.Where(x=>!x.IsRevealed && !x.IsFlagged))
                    {
                        Board.RevealPanel(unrevealedPanel.X, unrevealedPanel.Y);
                    }
                }
            }
        }

        private void PairsMoves()
        {
            var numberedPanels = Board.Panels.Where(x => x.IsRevealed && x.NearbyMines > 0);
            foreach(var numberPanel in numberedPanels)
            {
                var neighborPanels = Board.GetNearbyPanels(numberPanel.X, numberPanel.Y).Where(x => x.NearbyMines > 0);
                foreach(var neighbor in neighborPanels)
                {
                    var nextDoorPanels = Board.GetNearbyPanels(neighbor.X, neighbor.Y).Where(x => !x.IsRevealed);
                    var commonNeighbors = nextDoorPanels.Intersect(neighborPanels.Where(x => !x.IsRevealed));
                    if (neighbor.NearbyMines == numberPanel.NearbyMines && commonNeighbors.Where(x => x.IsFlagged).Count() == neighbor.NearbyMines)
                    {
                        foreach (var common in commonNeighbors.Where(x=>!x.IsFlagged))
                        {
                            Board.RevealPanel(common.X, common.Y);
                        }
                    }
                }
            }
        }

        public void FlagObviousMines()
        {
            //Foreach revealed panel that has a count > 0, if the number of unrevealed squares around it matches its number, they must all be mines.
            var numberPanels = Board.GetRevealedPanels().Where(x => x.NearbyMines > 0);
            foreach(var panel in numberPanels)
            {
                var neighborPanels = Board.GetNearbyPanels(panel.X, panel.Y);
                if(neighborPanels.Count(x=>!x.IsRevealed) == panel.NearbyMines)
                {
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
                //Reveal all unrevealed, unflagged panels
                var unrevealedPanels = Board.Panels.Where(x => !x.IsFlagged && !x.IsRevealed);
                foreach(var panel in unrevealedPanels)
                {
                    Board.RevealPanel(panel.X, panel.Y);
                }
            }
        }
    }
}
