/*
 * Owen Mount
 * CST-250
 * 09/21/2024
 * All work is my own
 */

using System.Text;
using MinesweeperClassLibrary;

// Declare and initialize
const bool debug = true; // Flag to set debug mode
var board = new Board(16, 40, 1);
Tuple<int, int> selectedCell;

/*
 * -------------------------------------------------------------
 * Main method start
 * -------------------------------------------------------------
 */

Console.WriteLine("Welcome to Minesweeper!");

// Initialize board after user selects starting location
// so the user doesn't select a bomb on first pick
Utility.PrintBoard(board);
selectedCell = Utility.GetRowColLocation(board);
board.InitializeBoard(selectedCell.Item1, selectedCell.Item2);

// Main game loop
while (board.DetermineGameState() == Board.GameState.InProgress)
{
    // If debugging is enabled, show the answers
    if (debug)
    {
        Console.WriteLine("[Debugging on] Answer Key:");
        Utility.ShowAnswerKey(board);
        Console.WriteLine();
    }

    Utility.PrintBoard(board);
    selectedCell = Utility.GetRowColLocation(board);

    Console.WriteLine($"Selected cell: ({selectedCell.Item1 + 1}, {selectedCell.Item2 + 1})");
    Utility.DoAction(board, selectedCell);
}

// Print final board
Utility.PrintBoard(board);

// Determine game results
if (board.DetermineGameState() == Board.GameState.Won)
{
    Console.WriteLine("Congratulations, you won!");
}

if (board.DetermineGameState() == Board.GameState.Lost)
{
    Console.WriteLine("Unfortunately you have been blown up.");
}


/*
 * -------------------------------------------------------------
 * Main method end
 * -------------------------------------------------------------
 */

/// <summary>
///     Utility class that handles console output/input
/// </summary>
internal static class Utility
{
    // Create a dictionary for easy color highlighting based on value
    private static readonly Dictionary<int, ConsoleColor> HighlightLevels = new()
    {
        { 1, ConsoleColor.Blue },
        { 2, ConsoleColor.Green },
        { 3, ConsoleColor.Red },
        { 4, ConsoleColor.DarkBlue },
        { 5, ConsoleColor.Yellow },
        { 6, ConsoleColor.Cyan },
        { 7, ConsoleColor.Magenta },
        { 8, ConsoleColor.DarkCyan }
    };

    /// <summary>
    ///     Print the current state of the board to the console
    /// </summary>
    /// <param name="board"></param>
    public static void PrintBoard(Board board)
    {
        // Declare and initialize
        string horizontalLine = "   " + HorizontalLine(board.Size);

        // Add column numbers and top gridline
        Console.Write("     ");
        for (int col = 1; col <= board.Size; col++)
        {
            Console.Write($"{col,-4}");
        }

        Console.WriteLine();
        Console.WriteLine(horizontalLine);


        for (int row = 0; row < board.Size; row++)
        {
            // Mark row number
            Console.Write($"{row + 1,-3}");
            for (int col = 0; col < board.Size; col++)
            {
                // Left cell boundary and padding
                Console.Write("| ");

                // Main logic for printing cells
                // If a cell has been visited, display the contents
                if (board.Cells[row, col].IsVisited)
                {
                    // Cell is bomb
                    if (board.Cells[row, col].IsBomb)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("B");

                    }
                    // Cell is not bomb
                    else
                    {
                        // Cell has special reward
                        if (board.Cells[row, col].HasSpecialReward)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write("R");
                        }
                        // Cell has no bomb neighbors
                        else if (board.Cells[row, col].Neighbors == 0)
                        {
                            Console.Write(" ");
                        }
                        // Cell has bomb neighbors
                        else
                        {
                            // Show number of neighboring bombs with corresponding highlight level
                            Console.ForegroundColor = HighlightLevels[board.Cells[row, col].Neighbors];
                            Console.Write(board.Cells[row, col].Neighbors);
                        }

                    }
                }
                // Cell has not been visited
                else
                {
                    // Cell is flagged
                    if (board.Cells[row, col].IsFlagged)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("F");
                    }
                    // Cell is not flagged
                    else
                    {
                        Console.Write("?");
                    }
                }

                // Reset console color and add right padding
                Console.ResetColor();
                Console.Write(" ");
            }

            // Insert final right border/bottom gridline and advance to the next row
            Console.Write("|");
            Console.WriteLine();
            Console.WriteLine(horizontalLine);
        }

        // Show number of rewards
        Console.WriteLine($"Defuse kits: {board.Rewards}");
    }

    /// <summary>
    ///     Generate a horizontal gridline with respect to size
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    private static string HorizontalLine(int size)
    {
        var sb = new StringBuilder();

        for (int col = 0; col < size; col++)
        {
            sb.Append("+---");
        }

        sb.Append("+");

        return sb.ToString();
    }

    /// <summary>
    ///     Show answers for a board
    /// </summary>
    /// <param name="board"></param>
    public static void ShowAnswerKey(Board board)
    {
        // Declare and initialize
        string horizontalLine = "   " + HorizontalLine(board.Size);

        // Add column numbers and top gridline
        Console.Write("     ");
        for (int col = 1; col <= board.Size; col++)
        {
            Console.Write($"{col,-4}");
        }

        Console.WriteLine();
        Console.WriteLine(horizontalLine);


        for (int row = 0; row < board.Size; row++)
        {
            // Mark row number
            Console.Write($"{row + 1,-3}");
            for (int col = 0; col < board.Size; col++)
            {
                // Left cell boundary and padding
                Console.Write("| ");

                // Main logic for printing cells
                // Cell is bomb
                if (board.Cells[row, col].IsBomb)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("B");

                }
                // Cell is not bomb
                else
                {
                    // Cell has special reward
                    if (board.Cells[row, col].HasSpecialReward)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("R");
                    }
                    // Cell has no bomb neighbors
                    else if (board.Cells[row, col].Neighbors == 0)
                    {
                        Console.Write(".");
                    }
                    // Cell has bomb neighbors
                    else
                    {
                        // Show number of neighboring bombs with corresponding highlight level
                        Console.ForegroundColor = HighlightLevels[board.Cells[row, col].Neighbors];
                        Console.Write(board.Cells[row, col].Neighbors);
                    }

                }

                // Reset console color and add right padding
                Console.ResetColor();
                Console.Write(" ");
            }

            // Insert final right border/bottom gridline and advance to the next row
            Console.Write("|");
            Console.WriteLine();
            Console.WriteLine(horizontalLine);
        }
    }

    /// <summary>
    ///     Utility method to get input with prompt
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    public static string GetInput(string prompt)
    {
        string? input = "";
        Console.Write(prompt + " ");
        input = Console.ReadLine();

        // Ensure input is not null or empty
        while (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Input cannot be null");
            Console.WriteLine(prompt);
            input = Console.ReadLine();
        }

        return input;
    }

    /// <summary>
    ///     Get row and column from the user and return as tuple
    /// </summary>
    /// <param name="board"></param>
    /// <returns></returns>
    public static Tuple<int, int> GetRowColLocation(Board board)
    {
        // Declare and initialize
        int row = -1;
        int col = -1;
        bool valid = false;

        // Get the row and col
        do
        {
            valid =
                int.TryParse(GetInput("Enter row:"), out row) &&
                int.TryParse(GetInput("Enter column:"), out col);

            // Ensure parsed result is within bounds
            if (valid)
            {
                valid = col > 0 && col <= board.Size && row > 0 && row <= board.Size;
            }

            // Notify user if entry is invalid
            if (!valid)
            {
                Console.WriteLine("Invalid input. Try again.");
            }

        } while (!valid);

        // Values are offset to work with 0-indexing
        return new Tuple<int, int>(row - 1, col - 1);
    }

    /// <summary>
    ///     Method to take action on cells.
    /// </summary>
    /// <param name="board"></param>
    /// <param name="location"></param>
    public static void DoAction(Board board, Tuple<int, int> location)
    {
        // Get input from user
        string action = GetInput("What would you like to do? (Flag, Visit, Use)").ToLower();

        switch (action)
        {
            // Mark a cell as visited
            case "visit":
                board.Visit(location.Item1, location.Item2);
                break;
            // Flag a cell
            case "flag":
                board.Cells[location.Item1, location.Item2].IsFlagged = true;
                break;
            // Use/collect a reward
            case "use":
                // Only allow a bonus to be collected if it exists and has been uncovered
                Console.WriteLine(board.UseSpecialBonus(location.Item1, location.Item2)
                    ? $"You collected a bomb defuse kit! You now have {board.Rewards}."
                    : "There is no reward here");

                break;
            default:
                Console.WriteLine("Invalid action. Try again.");
                break;
        }
    }
}