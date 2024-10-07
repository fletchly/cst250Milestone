/*
 * Owen Mount
 * CST-250
 * 09/21/2024
 * All work is my own
 */

namespace MinesweeperClassLibrary;

public class Board
{
    public int Size { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Cell[,] Cells { get; set; }
    public int Difficulty { get; set; }
    public int Rewards { get; set; }
    private int RewardLimit { get; set; }
    public enum GameState
    {
        PreStart,
        InProgress,
        Paused,
        Won,
        Lost
    }
    public List<(int, int)> BombLocations { get; set; }

    public Board(int size, int difficulty, int rewardLimit)
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
        RewardLimit = rewardLimit;
        
        Cells = new Cell[size, size];
        BombLocations = new List<(int, int)>();

        // Load Board with Cells
        for (var row = 0; row < Size; row++)
        for (var col = 0; col < Size; col++)
            Cells[row, col] = new Cell(row, col);
        
        // Setup bombs
        SetupBombsAndRewards();
    }

    /// <summary>
    ///     Initialize game board with bombs
    /// </summary>
    private void SetupBombsAndRewards()
    {
        // Declare and initialize
        var random = new Random();
        var row = -1;
        var column = -1;
        var bombsPlaced = 0;
        var rewardsPlaced = 0;

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
                BombLocations.Add((row, column));
                bombsPlaced++;
            }
        }

        while (rewardsPlaced < RewardLimit)
        {
            row = random.Next(Size);
            column = random.Next(Size);

            if (!Cells[row, column].HasSpecialReward)
            {
                Cells[row, column].HasSpecialReward = true;
                rewardsPlaced++;
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
    
    /// <summary>
    /// Determine the state of game and return it.
    /// </summary>
    /// <returns></returns>
    public GameState DetermineGameState()
    {
        // Declare and initialize
        bool allNonBombsVisited = true; // Assume that all non bombs have been visited by default
        bool allBombsFlagged = true; // Assume that all bombs have been flagged by default
        bool hasUnvisitedCells = false; // Assume that all cells have been visited by default
        
        
        // Iterate over every cell in the board
        foreach (var cell in Cells)
        {
            // If a bomb has been visited
            if (cell.IsVisited && cell.IsBomb)
            {
                if (Rewards > 0)
                {
                    Rewards--;
                }
                else
                {
                    return GameState.Lost;
                }
            }
            
            // If there exists a cell that is not a bomb and
            // has not been visited, we know that all
            // non-bombs have not been visited, which invalidates
            // one leg of our win condition
            if (!cell.IsVisited && !cell.IsBomb)
            {
                allNonBombsVisited = false;
            }

            // If there exists a cell that is a bomb and
            // has not been flagged, we know that at least
            // one cell that is a bomb has not been flagged,
            // which invalidates one leg of our win condition
            if (cell.IsBomb && !cell.IsFlagged)
            {
                allBombsFlagged = false;
            }

            // If there exists at least one cell that
            // has not been visited, we know that there
            // are more cells to visit, making our
            // "in progress" condition true
            if (!cell.IsVisited)
            {
                hasUnvisitedCells = true;
            }
            
        }

        // If every cell that is also a bomb has been
        // visited, then the game has been won.
        if (allNonBombsVisited || allBombsFlagged)
        {
            return GameState.Won;
        }

        // If there are more cells to visit,
        // the game is in-progress.
        if (hasUnvisitedCells)
        {
            return GameState.InProgress;
        }
        
        // If board is set up properly, this statement will never return.
        return GameState.InProgress;
    }
    
    public bool UseSpecialBonus(int row, int col)
    {
        if (Cells[row, col].HasSpecialReward && Cells[row, col].IsVisited)
        {
            Cells[row, col].HasSpecialReward = false;
            Rewards++;
            return true;
        }
        return false;
    }

    // Method to be implemented in future milestone
    public int DetermineFinalScore()
    {
        return 0;
    }
}