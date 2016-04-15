using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolverDemo.Lib.Objects
{
    public class BoardStats
    {
        public double TotalPanels { get; set; }
        public double PanelsRevealed { get; set; }
        public double Mines { get; set; }
        public double FlaggedMinePanels { get; set; }
        public double PercentMinesFlagged { get; set; }
        public double PercentPanelsRevealed { get; set; }
    }
}
