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
        public int MineCount { get; set; }
        public List<Panel> Panels { get; set; }
        public GameStatus Status { get; set; }

        public double PercentMinesFlagged { get; set; }
        public double PercentPanelsRevealed { get; set; }

        public GameBoard(int width, int height, int mines)
        {
            Width = width;
            Height = height;
            MineCount = mines;
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
            var nearbyPanels = Panels.Where(panel => panel.X >= (latitude - 1) && panel.X <= (latitude + 1)
                                                 && panel.Y >= (longitude - 1) && panel.Y <= (longitude + 1));
            var currentPanel = Panels.Where(panel => panel.X == latitude && panel.Y == longitude);
            return nearbyPanels.Except(currentPanel).ToList();
        }

        public List<Panel> GetClosestNeighbors(int latitude, int longitude, int depth)
        {
            var nearbyPanels = Panels.Where(panel => panel.X >= (latitude - depth) && panel.X <= (latitude + depth)
                                                 && panel.Y >= (longitude - depth) && panel.Y <= (longitude + depth));
            var currentPanel = Panels.Where(panel => panel.X == latitude && panel.Y == longitude);
            return nearbyPanels.Except(currentPanel).ToList();
        }

        public void RevealPanel(int x, int y)
        {
            var panel = Panels.First(z => z.X == x && z.Y == y);
            panel.IsRevealed = true;
            panel.IsFlagged = false;
            if (panel.IsMine) Status = GameStatus.Failed; //Game over!
            if (!panel.IsMine && panel.NearbyMines == 0)
            {
                RevealZeros(x, y);
            }
            if (!panel.IsMine)
            {
                CompletionCheck();
            }
        }

        public void FirstMove(int x, int y, Random rand)
        {
            var depth = 0.25 * Width;
            var neighbors = GetClosestNeighbors(x, y, (int)depth);
            neighbors.Add(GetPanel(x, y));
            var mineList = Panels.Except(neighbors).OrderBy(user => rand.Next());
            var mineSlots = mineList.Take(MineCount).ToList().Select(z => new { z.X, z.Y });


            foreach (var mineCoord in mineSlots)
            {
                Panels.Single(z => z.X == mineCoord.X && z.Y == mineCoord.Y).IsMine = true;
            }

            foreach (var openPanel in Panels)
            {
                if (openPanel.IsMine)
                {
                    continue;
                }

                var nearbyPanels = GetNearbyPanels(openPanel.X, openPanel.Y);

                openPanel.NearbyMines = nearbyPanels.Count(z => z.IsMine);
            }
        }

        public void RevealZeros(int x, int y)
        {
            var neighborPanels = GetNearbyPanels(x, y).Where(panel => !panel.IsRevealed);
            foreach (var panel in neighborPanels)
            {
                panel.IsRevealed = true;
                if (panel.NearbyMines == 0)
                {
                    RevealZeros(panel.X, panel.Y);
                }
            }
        }

        public void Display()
        {
            string output = "";
            foreach (var panel in Panels)
            {
                if (panel.X == 1)
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
                else if(panel.IsRevealed && !panel.IsMine)
                {
                    output += panel.NearbyMines + " ";
                }
                else if(panel.IsRevealed && panel.IsMine)
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
                if (panel.X == 1)
                {
                    Console.WriteLine(output);
                    output = "";
                }
                if (panel.IsMine)
                {
                    output += "M ";
                }
                else if (!panel.IsMine)
                {
                    output += panel.NearbyMines + " ";
                }
            }
            Console.WriteLine(output); //Write the last line
        }

        private void CompletionCheck()
        {
            var unrevealedPanels = Panels.Where(x => !x.IsRevealed).Select(x => x.ID);
            var minePanels = Panels.Where(x => x.IsMine).Select(x => x.ID);
            if (!unrevealedPanels.Except(minePanels).Any())
            {
                Status = GameStatus.Completed;
            }
        }

        public BoardStats GetStats()
        {
            BoardStats stats = new BoardStats();

            stats.Mines = Panels.Count(x => x.IsMine);
            stats.FlaggedMinePanels = Panels.Count(x => x.IsMine && x.IsFlagged);

            stats.PercentMinesFlagged = Math.Round((double)(stats.FlaggedMinePanels / stats.Mines) * 100, 2);

            stats.TotalPanels = Panels.Count;
            stats.PanelsRevealed = Panels.Count(x => x.IsFlagged || x.IsRevealed);

            stats.PercentPanelsRevealed = Math.Round((double)(stats.PanelsRevealed / stats.TotalPanels) * 100, 2);

            return stats;
        }

        public List<Panel> GetRevealedPanels()
        {
            return Panels.Where(x => x.IsRevealed).ToList();
        }

        public Panel GetPanel(int x, int y)
        {
            return Panels.First(z => z.X == x && z.Y == y);
        }

        public void FlagPanel(int x, int y)
        {
            var panel = Panels.Where(z => z.X == x && z.Y == y).First();
            if(!panel.IsRevealed)
            {
                panel.IsFlagged = true;
            }
        }
    }
}
