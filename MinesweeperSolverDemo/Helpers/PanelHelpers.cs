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
        public static Coordinate GetPanelCoordinate()
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

            return new Coordinate()
            {
                Latitude = x,
                Longitude = y
            };
        }
    }
}
