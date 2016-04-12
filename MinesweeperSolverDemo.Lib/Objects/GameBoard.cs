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
        public int BombCount { get; set; }
        public List<Panel> Panels { get; set; }
        public GameStatus Status { get; set; }

        public GameBoard(int width, int height, int bombs)
        {
            Width = width;
            Height = height;
            BombCount = bombs;
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

            Status = GameStatus.InProgress;
            
        }

        public List<Panel> GetNearbyPanels(int latitude, int longitude)
        {
            var nearbyPanels = Panels.Where(x => x.Coordinate.Latitude >= (latitude - 1) && x.Coordinate.Latitude <= (latitude + 1)
                                                 && x.Coordinate.Longitude >= (longitude - 1) && x.Coordinate.Longitude <= (longitude + 1));
            var currentPanel = Panels.Where(x => x.Coordinate.Latitude == latitude && x.Coordinate.Longitude == longitude);
            return nearbyPanels.Except(currentPanel).ToList();
        }

        public List<Panel> GetClosestNeighbors(int latitude, int longitude, int depth)
        {
            var nearbyPanels = Panels.Where(x => x.Coordinate.Latitude >= (latitude - depth) && x.Coordinate.Latitude <= (latitude + depth)
                                                 && x.Coordinate.Longitude >= (longitude - depth) && x.Coordinate.Longitude <= (longitude + depth));
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
            panel.IsFlagged = false;
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

        public void FirstMove(int x, int y, Random rand)
        {
            var depth = 0.25 * Width;
            var neighbors = GetClosestNeighbors(x, y, (int)depth);
            neighbors.Add(GetPanel(x, y));
            var bombList = Panels.Except(neighbors).OrderBy(user => rand.Next());
            var bombSlots = bombList.Take(BombCount).ToList().Select(z => z.Coordinate);


            foreach (var bombCoord in bombSlots)
            {
                Panels.Single(z => z.Coordinate == bombCoord).IsBomb = true;
            }

            foreach (var openPanel in Panels)
            {
                if (openPanel.IsBomb)
                {
                    continue;
                }

                var nearbyPanels = GetNearbyPanels(openPanel.Coordinate.Latitude, openPanel.Coordinate.Longitude);

                openPanel.NearbyBombs = nearbyPanels.Count(z => z.IsBomb);
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

        public void DisplayFinal()
        {
            string output = "";
            foreach (var panel in Panels)
            {
                if (panel.Coordinate.Latitude == 1)
                {
                    Console.WriteLine(output);
                    output = "";
                }
                if (panel.IsBomb)
                {
                    output += "M ";
                }
                else if (!panel.IsBomb)
                {
                    output += panel.NearbyBombs + " ";
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

        public Panel GetPanel(int x, int y)
        {
            return Panels.First(z => z.Coordinate.Latitude == x && z.Coordinate.Longitude == y);
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
