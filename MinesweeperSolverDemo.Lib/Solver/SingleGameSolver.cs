using MinesweeperSolverDemo.Lib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolverDemo.Lib.Solver
{
    public class SingleGameSolver
    {
        public GameBoard Board { get; set; }

        public List<Panel> CompletedPanels { get; set; }
        public bool IsUnsolveable { get; set; }

        public int MoveCounter { get; set; }
        public SingleGameSolver()
        {
            MoveCounter = 0;

            Random rand = new Random();
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

            Board = new GameBoard(width, height, bombs, rand);
        }

        public SingleGameSolver(GameBoard board)
        {
            Board = board;
        }

        public int GetWidth()
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

        public int GetHeight()
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

        public int GetBombs()
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

        public void CoordinateErrors(int coord)
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

        public void BombsErrors(int bombs)
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

        public void HeightErrors(int height)
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

        public void WidthErrors(int width)
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

        public void Solve()
        {
            Random rand = new Random();
            while (Board.Status == Enums.GameStatus.InProgress && !IsUnsolveable)
            {
                if(!Board.Panels.Any(x=>x.IsRevealed))
                {
                    RandomMove(rand);
                    FlagObviousMines();
                }

                while(MoveCounter == Board.Panels.Count(x=>x.IsRevealed) && Board.Status != Enums.GameStatus.Failed)
                {
                    RandomMove(rand);
                    FlagObviousMines();
                }

                if (HasAvailableMoves())
                {
                    NextMove();
                    FlagObviousMines();
                }
                else
                {
                    IsUnsolveable = true;
                }

                Board.Display();
            }

            if(Board.Status == Enums.GameStatus.Failed)
            {
                Console.WriteLine("Solver Failed!");
            }
            if (Board.Status == Enums.GameStatus.Completed)
            {
                Console.WriteLine("Solver SUCCESS");
            }
            if(IsUnsolveable)
            {
                Console.Write("Game is UNSOLVEABLE.");
            }
        }

        public void RandomMove(Random rand)
        {
            var randomID = rand.Next(1, Board.Panels.Count);
            var panel = Board.Panels.First(x => x.ID == randomID);
            while(panel.IsRevealed)
            {
                randomID = rand.Next(1, Board.Panels.Count);
                panel = Board.Panels.First(x => x.ID == randomID);
            }

            Board.RevealPanel(panel.Coordinate.Latitude, panel.Coordinate.Longitude);
            MoveCounter++;
        }

        public bool HasAvailableMoves()
        {
            //Find any numbered panel where the number of flags around it equals its number, then click on every square around that.
            var numberedPanels = Board.Panels.Where(x => x.IsRevealed && x.NearbyBombs > 0);
            foreach (var numberPanel in numberedPanels)
            {
                var neighborPanels = Board.GetNearbyPanels(numberPanel.Coordinate.Latitude, numberPanel.Coordinate.Longitude);
                var flaggedNeighbors = neighborPanels.Where(x => x.IsFlagged);
                if (flaggedNeighbors.Count() == numberPanel.NearbyBombs && neighborPanels.Any(x => !x.IsRevealed && !x.IsFlagged))
                {
                    return true;
                }
            }

            return false;
        }

        public void NextMove()
        {
            //Find any numbered panel where the number of flags around it equals its number, then click on every square around that.
            var numberedPanels = Board.Panels.Where(x => x.IsRevealed && x.NearbyBombs > 0);
            foreach(var numberPanel in numberedPanels)
            {
                var neighborPanels = Board.GetNearbyPanels(numberPanel.Coordinate.Latitude, numberPanel.Coordinate.Longitude);
                var flaggedNeighbors = neighborPanels.Where(x => x.IsFlagged);
                if(flaggedNeighbors.Count() == numberPanel.NearbyBombs)
                {
                    //Reveal all unrevealed, unflagged neighbor panels
                    foreach(var unrevealedPanel in neighborPanels.Where(x=>!x.IsRevealed && !x.IsFlagged))
                    {
                        Board.RevealPanel(unrevealedPanel.Coordinate.Latitude, unrevealedPanel.Coordinate.Longitude);
                        MoveCounter++;
                    }
                }
            }
        }

        public void FlagObviousMines()
        {
            //Foreach revealed panel that has a count > 0, if the number of unrevealed squares around it matches its number, they must all be mines.
            var numberPanels = Board.GetRevealedPanels().Where(x => x.NearbyBombs > 0);
            foreach(var panel in numberPanels)
            {
                var neighborPanels = Board.GetNearbyPanels(panel.Coordinate.Latitude, panel.Coordinate.Longitude);
                if(neighborPanels.Count(x=>!x.IsRevealed) == panel.NearbyBombs)
                {
                    foreach(var neighbor in neighborPanels.Where(x=>!x.IsRevealed))
                    {
                        Board.FlagPanel(neighbor.Coordinate.Latitude, neighbor.Coordinate.Longitude);
                    }
                }
            }
        }

        public void ChangeCompletedPanels()
        {
            var revealedPanels = Board.GetRevealedPanels();
        }
    }
}
