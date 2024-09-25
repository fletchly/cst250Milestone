/*
 * Owen Mount
 * CST-250
 * 09/21/2024
 * All work is my own
 */

namespace MinesweeperClassLibrary;

public class Board
{
    public enum GameState
    {
        PreStart,
        InProgress,
        Paused,
        Won,
        Lost
    }

    public Board(int size, int difficulty)
    {
        // Throw an exception if the number of bombs exceeds the total area of the board
        if (difficulty < 1 || difficulty > size * size)
        {
            throw new ArgumentOutOfRangeException(nameof(difficulty), message: "The difficulty must be between 1 and " + size * size + ".");
        }
        
        // Initialize Size and Board
        Size = size;
        Difficulty = difficulty;
        Rewards = 0;

        Cells = new Cell[size, size];

        // Load Board with Cells
        for (var row = 0; row < Size; row++)
        for (var col = 0; col < Size; col++)
            Cells[row, col] = new Cell(row, col);

        // Setup bombs
        SetupBombs();
    }

    public int Size { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Cell[,] Cells { get; set; }
    public int Difficulty { get; set; }

    public int Rewards { get; set; }

    /// <summary>
    ///     Initialize game board with bombs
    /// </summary>
    private void SetupBombs()
    {
        // Declare and initialize
        var random = new Random();
        var row = -1;
        var column = -1;
        var bombsPlaced = 0;

        while (bombsPlaced < Difficulty)
        {
            // Get a random location on the board
            row = random.Next(Size);
            column = random.Next(Size);

            // If a bomb does not exist in the current location, create one
            // and increment the counter. Otherwise, do not increment the
            // counter and attempt to place again.
            if (!Cells[row, column].IsBomb)
            {
                Cells[row, column].IsBomb = true;
                CalculateNeighbors(row, column);
                bombsPlaced++;
            }
        }
    }

    /// <summary>
    ///     Update cells surrounding a bomb.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    private void CalculateNeighbors(int row, int column)
    {
        // Declare and initialize
        var currentRow = 0;
        var currentCol = 0;

        // Represents all possible locations of neighboring cells, relative to the current cell.
        // The first array represents the row, and the second represents the column
        int[,] neighborLocations =
        {
            { 1, 0, -1, -1, -1, 0, 1, 1 }, // Row
            { -1, -1, -1, 0, 1, 1, 1, 0 } // Column
        };

        // Increment the 'neighbors' value for each neighboring cell
        for (var neighbor = 0; neighbor < 8; neighbor++)
        {
            currentRow = row + neighborLocations[0, neighbor];
            currentCol = column + neighborLocations[1, neighbor];

            // Only try to access the cell if it is on the board
            if (CellIsOnBoard(currentRow, currentCol)) Cells[currentRow, currentCol].Neighbors++;
        }
    }


    /// <summary>
    ///     Determine if cell is on board
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public bool CellIsOnBoard(int row, int column)
    {
        return row >= 0 && column >= 0 && row < Size && column < Size;
    }

    // Method to be implemented in future milestone
    public void DetermineGameState()
    {
    }

    // Method to be implemented in future milestone
    public void UseSpecialBonus()
    {
    }

    // Method to be implemented in future milestone
    public int DetermineFinalScore()
    {
        return 0;
    }
}