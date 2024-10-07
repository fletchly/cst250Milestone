/*
 * Owen Mount
 * CST-250
 * 09/21/2024
 * All work is my own
 */

namespace MinesweeperClassLibrary;

/// <summary>
///     Game cell class
/// </summary>
public class Cell
{
    // Class level properties
    public int Row { get; set; } // Vertical location on the game board
    public int Column { get; set; } // Horizontal location on the game board
    public bool IsVisited { get; set; } // Indicates if the cell has been uncovered or not
    public bool IsBomb { get; set; } // Determines if the cell is a bomb or not
    public bool IsFlagged { get; set; } // Determines if the cell has been flagged
    public int Neighbors { get; set; } // The number of neighboring cells with a bomb
    public bool HasSpecialReward { get; set; } // Determines if the cell contains a special reward
    
    /// <summary>
    ///     Default constructor
    /// </summary>
    public Cell()
    {
        Row = -1;
        Column = -1;
        IsVisited = false;
        IsBomb = false;
        IsFlagged = false;
        Neighbors = -1;
        HasSpecialReward = false;
    }

    /// <summary>
    ///     Parameterized constructor
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    public Cell(int row, int column)
    {
        Row = row;
        Column = column;
        IsVisited = false;
        IsBomb = false;
        IsFlagged = false;
        Neighbors = 0;
        HasSpecialReward = false;
    }
}