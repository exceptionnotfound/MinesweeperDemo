using MinesweeperSolverDemo.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolverDemo.Lib.Objects
{
    public class GameBoard
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<Panel> Panels { get; set; }
        public GameStatus Status { get; set; }

        public GameBoard(int width, int height, int bombs, Random rand)
        {
            Width = width;
            Height = height;
            Panels = new List<Panel>();

            int id = 1;
            for(int i = 1; i <= height; i++)
            {
                for (int j = 1; j <= width; j++)
                {
                    Panels.Add(new Panel(id, j, i));
                    id++;
                }
            }

            var bombList = Panels.OrderBy(user => rand.Next());
            var bombSlots = bombList.Take(bombs).ToList().Select(x=>x.Coordinate);


            foreach(var bombCoord in bombSlots)
            {
                Panels.Single(x => x.Coordinate == bombCoord).IsBomb = true;
            }

            foreach(var panel in Panels)
            {
                if(panel.IsBomb)
                {
                    continue;
                }

                var nearbyPanels = GetNearbyPanels(panel.Coordinate.Latitude, panel.Coordinate.Longitude);

                panel.NearbyBombs = nearbyPanels.Count(x => x.IsBomb);
            }

            Status = GameStatus.InProgress;
            
        }

        public List<Panel> GetNearbyPanels(int latitude, int longitude)
        {
            var nearbyPanels = Panels.Where(x => x.Coordinate.Latitude >= (latitude - 1) && x.Coordinate.Latitude <= (latitude + 1)
                                                 && x.Coordinate.Longitude >= (longitude - 1) && x.Coordinate.Longitude <= (longitude + 1));
            var currentPanel = Panels.Where(x => x.Coordinate.Latitude == latitude && x.Coordinate.Longitude == longitude);
            return nearbyPanels.Except(currentPanel).ToList();
        }

        public void RevealPanel(Coordinate coordinate)
        {
            RevealPanel(coordinate.Latitude, coordinate.Longitude);
        }

        public void RevealPanel(int x, int y)
        {
            var panel = Panels.First(z => z.Coordinate.Latitude == x && z.Coordinate.Longitude == y);
            panel.IsRevealed = true;
            if (panel.IsBomb) Status = GameStatus.Failed; //Game over!
            if (!panel.IsBomb && panel.NearbyBombs == 0)
            {
                RevealZeros(x, y);
            }
            if (Status != GameStatus.Failed)
            {
                CompletionCheck();
            }
        }

        public void RevealZeros(Coordinate coordinate)
        {
            RevealZeros(coordinate.Latitude, coordinate.Longitude);
        }

        public void RevealZeros(int x, int y)
        {
            var neighborPanels = GetNearbyPanels(x, y).Where(panel => !panel.IsRevealed);
            foreach (var panel in neighborPanels)
            {
                panel.IsRevealed = true;
                if (panel.NearbyBombs == 0)
                {
                    RevealZeros(panel.Coordinate.Latitude, panel.Coordinate.Longitude);
                }
            }
        }

        public void Display()
        {
            string output = "";
            foreach (var panel in Panels)
            {
                if (panel.Coordinate.Latitude == 1)
                {
                    Console.WriteLine(output);
                    output = "";
                }
                if (panel.IsFlagged)
                {
                    output += "F ";
                }
                else if (!panel.IsRevealed)
                {
                    output += "U ";
                }
                else if(panel.IsRevealed && !panel.IsBomb)
                {
                    output += panel.NearbyBombs + " ";
                }
                else if(panel.IsRevealed && panel.IsBomb)
                {
                    output += "X ";
                }
            }
            Console.WriteLine(output); //Write the last line
        }

        public bool IsValidWidth(int width)
        {
            return width > 0 && width <= Width;
        }

        public bool IsValidHeight(int height)
        {
            return height > 0 && height <= Height;
        }

        private void CompletionCheck()
        {
            var unrevealedPanels = Panels.Where(x => !x.IsRevealed).Select(x=>x.ID);
            var bombPanels = Panels.Where(x => x.IsBomb).Select(x => x.ID);
            if (!unrevealedPanels.Except(bombPanels).Any())
            {
                Status = GameStatus.Completed;
            }
        }

        public int CountUnrevealedBombs()
        {
            return Panels.Count(x => x.IsRevealed == false && x.IsBomb);
        }

        public List<Panel> GetUnrevealedPanels()
        {
            return Panels.Where(x => x.IsRevealed == false).ToList();
        }

        public List<Panel> GetRevealedPanels()
        {
            return Panels.Where(x => x.IsRevealed).ToList();
        }

        public void FlagPanel(int x, int y)
        {
            var panel = Panels.Where(z => z.Coordinate.Latitude == x && z.Coordinate.Longitude == y).First();
            if(!panel.IsRevealed)
            {
                panel.IsFlagged = true;
            }
        }
    }
}
