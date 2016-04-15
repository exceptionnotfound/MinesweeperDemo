using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolverDemo.Lib.Solver
{
public class GameSolver
{
    protected int GetWidth()
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

    protected int GetHeight()
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

    protected int GetMines()
    {
        Console.Write("Please enter the number of mines on each board: ");
        string minesEntered = Console.ReadLine();
        int mines;
        bool isValid = int.TryParse(minesEntered, out mines);
        if (isValid)
        {
            return mines;
        }
        else return -1;
    }

    protected int GetBoards()
    {
        Console.Write("Please enter the number of board to be solved: ");
        string boardsEntered = Console.ReadLine();
        int boards;
        bool isValid = int.TryParse(boardsEntered, out boards);
        if (isValid)
        {
            return boards;
        }
        else return -1;
    }

    protected void WidthErrors(int width)
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

    protected void HeightErrors(int height)
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

    protected void MinesErrors(int mines)
    {
        if (mines == 0)
        {
            Console.WriteLine("The number of mines must be greater than 0.");
        }
        else if (mines < 0)
        {
            Console.WriteLine("Please enter a valid positive number for the number of mines on the board.");
        }
    }

    protected void BoardsErrors(int boards)
    {
        if (boards == 0)
        {
            Console.WriteLine("The number of boards must be greater than 0.");
        }
        else if (boards < 0)
        {
            Console.WriteLine("Please enter a valid positive number for the number of boards to be solved.");
        }
    }
}
}
