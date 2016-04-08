using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolverDemo.Lib.Objects
{
    public class Panel
    {
        public int ID { get; set; }
        public Coordinate Coordinate { get; set; }
        public bool IsBomb { get; set; }
        public int NearbyBombs { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }

        public Panel(int id, int x, int y)
        {
            ID = id;
            Coordinate = new Coordinate()
            {
                Latitude = x,
                Longitude = y
            };
        }
    }
}
