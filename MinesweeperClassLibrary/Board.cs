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

    public Board(int size, int difficulty, int rewardLimit)
    {
        // Throw an exception if the number of bombs exceeds the total area of the board
        if (difficulty < 1 || difficulty > size * size)
        {
            throw new ArgumentOutOfRangeException(nameof(difficulty),
                "The difficulty must be between 1 and " + size * size + ".");
        }

        // Initialize Size and Board
        Size = size;
        Difficulty = difficulty;
        Rewards = 0;
        RewardLimit = rewardLimit;

        Cells = new Cell[size, size];
        BombLocations = new List<(int, int)>();

        // Load Board with Cells
        for (int row = 0; row < Size; row++)
        for (int col = 0; col < Size; col++)
        {
            Cells[row, col] = new Cell(row, col);
        }
    }

    public int Size { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Cell[,] Cells { get; set; }
    public int Difficulty { get; set; }
    public int Rewards { get; set; }
    private int RewardLimit { get; }
    public List<(int, int)> BombLocations { get; set; }


    public void InitializeBoard(int startRow, int startCol)
    {
        SetupBombsAndRewards(startRow, startCol);
        FloodFill(startRow, startCol);
    }
    
    /// <summary>
    ///     Initialize game board with bombs
    /// </summary>
    private void SetupBombsAndRewards(int startRow, int startCol)
    {
        // Declare and initialize
        var random = new Random();
        int row = -1;
        int column = -1;
        int bombsPlaced = 0;
        int rewardsPlaced = 0;

        while (bombsPlaced < Difficulty)
        {
            // Get a random location on the board
            row = random.Next(Size);
            column = random.Next(Size);

            // If a bomb does not exist in the current location and
            // the location is not in the starting location "safe zone", create one
            // and increment the counter. Otherwise, do not increment the
            // counter and attempt to place again.
            if (!Cells[row, column].IsBomb && !SafeZone(row, column, startRow, startCol))
            {
                Cells[row, column].IsBomb = true;
                Cells[row, column].Neighbors = 9;
                CalculateNeighbors(row, column);
                BombLocations.Add((row, column));
                bombsPlaced++;
            }
        }
        
        
        while (rewardsPlaced < RewardLimit)
        {
            // Get a random location on the board
            row = random.Next(Size);
            column = random.Next(Size);

            // If a reward does not exist in the current location and
            // the current cell is not a bomb, create a reward
            // and increment the counter. Otherwise, do not increment the
            // counter and attempt to place again.
            if (!Cells[row, column].HasSpecialReward && !Cells[row,column].IsBomb)
            {
                Cells[row, column].HasSpecialReward = true;
                rewardsPlaced++;
            }
        }
    }

    /// <summary>
    /// Calculate a safe zone around starting cell to ensure a bomb is not placed
    /// on the starting cell or withing 1 cell distance from it in all directions
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <param name="startRow"></param>
    /// <param name="startCol"></param>
    /// <returns></returns>
    private bool SafeZone(int row, int column, int startRow, int startCol)
    {
        // Calculate the difference in rows and columns
        int rowDiff = Math.Abs(startRow - row);
        int colDiff = Math.Abs(startCol - column);

        // If both differences are <= 1, the cell is either the same or within one cell
        return rowDiff <= 1 && colDiff <= 1;
    }
    
    /// <summary>
    ///     Update cells surrounding a bomb.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    private void CalculateNeighbors(int row, int column)
    {
        // Declare and initialize
        int currentRow = 0;
        int currentCol = 0;

        // Represents all possible locations of neighboring cells, relative to the current cell.
        // The first array represents the row, and the second represents the column
        int[,] neighborLocations =
        {
            { 1, 0, -1, -1, -1, 0, 1, 1 }, // Row
            { -1, -1, -1, 0, 1, 1, 1, 0 } // Column
        };

        // Increment the 'neighbors' value for each neighboring cell
        for (int neighbor = 0; neighbor < 8; neighbor++)
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
    private bool CellIsOnBoard(int row, int column)
    {
        return row >= 0 && column >= 0 && row < Size && column < Size;
    }

    /// <summary>
    ///     Determine the state of game and return it.
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

    /// <summary>
    /// Method to control how rewards are collected
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public bool UseSpecialBonus(int row, int col)
    {
        // If the cell is uncovered and has a reward, collect it.
        if (Cells[row, col].HasSpecialReward && Cells[row, col].IsVisited)
        {
            Cells[row, col].HasSpecialReward = false;
            Rewards++;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Method to determine how cells should be filled
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    public void Visit(int row, int col)
    {
        // In traditional minesweeper, Flood Fill is only used if the selected
        // cell has no neighboring bombs. Otherwise, only the selected cell is
        // marked as visited.
        if (Cells[row, col].Neighbors == 0)
        {
            FloodFill(row, col);
        }
        else
        {
            Cells[row, col].IsVisited = true;
        }
    }

    /// <summary>
    /// Recursive method to mark empty cells as visited
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    private void FloodFill(int row, int col)
    {
        // If the cell is not on the board, do nothing.
        if (!CellIsOnBoard(row, col)) return;
        
        // If the cell is not visited, has zero bomb neighbors, and is not flagged,
        // then mark it as visited and continue the flood fill path
        if (!Cells[row, col].IsVisited && Cells[row, col].Neighbors == 0 && !Cells[row, col].IsFlagged)
        {
            Cells[row, col].IsVisited = true;
                
            FloodFill(row + 1, col);
            FloodFill(row - 1, col);
            FloodFill(row, col + 1);
            FloodFill(row, col - 1);
            FloodFill(row + 1, col + 1);
            FloodFill(row - 1, col - 1);
            FloodFill(row + 1, col - 1);
            FloodFill(row - 1, col + 1);
        }
        // Otherwise, if the cell has more than one neighbor and is not a bomb,
        // then mark is as visited and terminate the flood fill path
        else if (Cells[row, col].Neighbors != 0 && !Cells[row, col].IsBomb)
        {
            Cells[row, col].IsVisited = true;
        }
    }

    // Method to be implemented in future milestone
    public int DetermineFinalScore()
    {
        return 0;
    }
}