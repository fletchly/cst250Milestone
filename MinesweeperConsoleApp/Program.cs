/*
 * Owen Mount
 * CST-250
 * 09/21/2024
 * All work is my own
 */

using MinesweeperClassLibrary;

/*
 * -------------------------------------------------------------
 * Main method start
 * -------------------------------------------------------------
 */

Board board = new Board(16, 40);

ShowAnswerKey(board);

/*
 * -------------------------------------------------------------
 * Main method end
 * -------------------------------------------------------------
 */

static void ShowAnswerKey(Board board)
{
    // Create a dictionary for easy color highlighting based on value
    var highlightLevels = new Dictionary<int, ConsoleColor>()
    {
        { 1, ConsoleColor.Blue },
        { 2, ConsoleColor.Green },
        { 3, ConsoleColor.Red },
        { 4, ConsoleColor.DarkBlue },
        { 5, ConsoleColor.Yellow },
        { 6, ConsoleColor.Cyan },
        { 7, ConsoleColor.Magenta },
        { 8, ConsoleColor.Gray }
    };
    
    // Print out entire board by iterating over cells array
     for (int row = 0; row < board.Size; row++)
     {
         for (int column = 0; column < board.Size; column++)
         {
             if (board.Cells[row, column].IsBomb) // If there is a bomb, print B
             {
                 Console.ForegroundColor = ConsoleColor.DarkRed;
                 Console.Write("B ");
                 Console.ResetColor();
             }
             else if (board.Cells[row, column].Neighbors > 0) // If the cell cas neighboring bombs, print how many
             {
                 Console.ForegroundColor = highlightLevels[board.Cells[row, column].Neighbors];
                 Console.Write($"{board.Cells[row, column].Neighbors} ");
                 Console.ResetColor();
             }
             else
             {
                 Console.Write(". "); // If the cell is not a bomb and has no neighbors, print a .
             }
         }
         Console.WriteLine();
     }
}