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

        public GameBoard(int width, int height, int bombs)
        {
            Width = width;
            Height = height;
            var rand = new Random();
            Panels = new List<Panel>();

            int id = 1;
            for(int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Panels.Add(new Panel(id, i, j));
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
            
        }

        public List<Panel> GetNearbyPanels(int latitude, int longitude)
        {
            var nearbyPanels = Panels.Where(x => x.Coordinate.Latitude >= (latitude - 1) && x.Coordinate.Latitude <= (latitude + 1)
                                                 && x.Coordinate.Longitude >= (longitude - 1) && x.Coordinate.Longitude <= (longitude + 1));
            var currentPanel = Panels.Where(x => x.Coordinate.Latitude == latitude && x.Coordinate.Longitude == longitude);
            return nearbyPanels.Except(currentPanel).ToList();
        }

        public void DisplayAll()
        {
            string output = "";
            foreach (var panel in Panels)
            {
                if(panel.Coordinate.Longitude == 0)
                {
                    Console.WriteLine(output);
                    output = "";
                }
                if(panel.IsBomb)
                {
                    output += "X ";
                }
                else
                {
                    output += panel.NearbyBombs + " ";
                }
            }
        }

        public void Display()
        {
            string output = "";
            foreach (var panel in Panels)
            {
                if (panel.Coordinate.Longitude == 0)
                {
                    Console.WriteLine(output);
                    output = "";
                }
                if (!panel.IsRevealed)
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

        public bool IsCompleted()
        {
            var unrevealedPanels = Panels.Where(x => !x.IsRevealed);
            var bombPanels = Panels.Where(x => x.IsBomb);
            if (unrevealedPanels.Equals(bombPanels)) return true;
            else return false;
        }

        public int CountUnrevealedBombs()
        {
            return Panels.Count(x => x.IsRevealed == false && x.IsBomb);
        }
    }
}
