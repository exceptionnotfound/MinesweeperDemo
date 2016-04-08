using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolverDemo.Helpers
{
    public static class ErrorHelpers
    {
        public static void CoordinateErrors(int coord)
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
    }
}
