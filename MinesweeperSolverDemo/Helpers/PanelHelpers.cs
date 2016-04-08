using MinesweeperSolverDemo.Lib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolverDemo.Helpers
{
    public static class PanelHelpers
    {
        public static bool RevealPanel(GameBoard board)
        {
            int x = 0, y = 0;
            while (x <= 0)
            {
                //Get Horizontal Coordinate
                Console.WriteLine("Enter horizontal coordinate:");
                string xEntered = Console.ReadLine();
                bool isValid = int.TryParse(xEntered, out x);
                ErrorHelpers.CoordinateErrors(x);
            }

            while (y <= 0)
            {
                Console.WriteLine("Enter vertical coordinate:");
                string yEntered = Console.ReadLine();
                bool isValid = int.TryParse(yEntered, out y);
                ErrorHelpers.CoordinateErrors(x);
            }

            var panel = board.Panels.Where(pane => pane.Coordinate.Latitude == (x - 1) && pane.Coordinate.Longitude == (y - 1)).First();
            panel.IsRevealed = true;
            if (panel.IsBomb) return false; //Game over!
            if (panel.NearbyBombs == 0)
            {
                RevealZeros(board, x, y);
            }
            return true;

        }

        public static void RevealZeros(GameBoard board, int x, int y)
        {
            var neighborPanels = board.GetNearbyPanels(x, y).Where(panel => !panel.IsRevealed);
            foreach (var panel in neighborPanels)
            {
                panel.IsRevealed = true;
                if (panel.NearbyBombs == 0)
                {
                    RevealZeros(board, panel.Coordinate.Latitude, panel.Coordinate.Longitude);
                }
            }
        }
    }
}
