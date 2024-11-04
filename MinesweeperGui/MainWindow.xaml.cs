using Microsoft.Win32.SafeHandles;
using MinesweeperClassLibrary;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinesweeperGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Declare instance variables
        SetupWindow? setup;
        Board board;
        double ButtonSize;
        Button[,] Buttons;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            board = new Board(16, 40, 1);
            InitializeBoardGrid();
        }

        /// <summary>
        /// Populate the board grid with appropriate number of buttons
        /// </summary>
        private void InitializeBoardGrid()
        {
            // Determine button size and ensure board is square
            ButtonSize = GrdBoard.Width / board.Size;
            GrdBoard.Height = GrdBoard.Width;

            // Setup board rows and cols
            for (int size = 0; size < board.Size; size++)
            {
                GrdBoard.RowDefinitions.Add(new RowDefinition());
                GrdBoard.ColumnDefinitions.Add(new ColumnDefinition());
                GrdBoard.RowDefinitions[size].Height = GridLength.Auto;
                GrdBoard.ColumnDefinitions[size].Width = GridLength.Auto;
            }

            // Populate board with cells
            foreach (Cell cell in board.Cells)
            {
                // Create a new button for each cell and align it to the board
                Button button = new Button();

                // Use the WPF grid's row and column properties
                // for cells to lay out board
                Grid.SetRow(button, cell.Row);
                Grid.SetColumn(button, cell.Column);

                // Set button size and width
                button.Width = ButtonSize;
                button.Height = ButtonSize;

                // Subscribe to the cell button event handler
                button.Click += new RoutedEventHandler(BtnCellClick);

                // Add button to board
                GrdBoard.Children.Add(button);
                
            }
        }

        /// <summary>
        /// Logic to update the board display
        /// </summary>
        private void UpdateBoard()
        {
            int row, col;
            Cell currentCell;

            // Iterate over each button that needs updating
            foreach (Button button in GrdBoard.Children)
            {
                row = Grid.GetRow(button); col = Grid.GetColumn(button);
                currentCell = board.Cells[row, col];

                // If a cell has been visited, disable it and show its contents
                if (currentCell.IsVisited)
                {
                    button.IsEnabled = false;

                    // Cell is a bomb
                    if (currentCell.IsBomb)
                    {
                        button.Content = "B";
                    }
                    // Cell is not a bomb
                    else
                    {
                        // Cell has special reward
                        if (currentCell.HasSpecialReward)
                        {
                            button.Content = "R";
                            button.IsEnabled = true;
                        }
                        // Cell has no bomb neighbors
                        else if (currentCell.Neighbors == 0)
                        {
                            button.Content = "";
                        }
                        // Cell has bomb neighbors
                        else
                        {
                            button.Content = $"{currentCell.Neighbors}";
                        }
                    }
                }
                // Cell has not been visited
                else
                {
                    // Cell has been flagged
                    if (currentCell.IsFlagged)
                    {
                        button.Content = "F";
                    }
                    // Cell has not been flagged
                    else
                    {
                        button.Content = "?";
                    }
                }
            }
        }

        // Event handlers

        /// <summary>
        /// New game button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNewGameClick(object sender, RoutedEventArgs e)
        {
            // Show a new setup window as a
            // dialog box attached to the main form
            setup = new SetupWindow();
            setup.Owner = this;
            setup.ShowDialog();

            // Refresh the board with new game parameters
            GrdBoard.Children.Clear();
            board = new Board(setup.Size, setup.Difficulty, 1);
            InitializeBoardGrid();
        }

        /// <summary>
        /// Cell button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCellClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;

            if (board.DetermineGameState() == Board.GameState.PreStart)
            {
                board.InitializeBoard(Grid.GetRow(clickedButton), Grid.GetColumn(clickedButton));
            }
            else
            {
                board.Visit(Grid.GetRow(clickedButton), Grid.GetColumn(clickedButton));
            }

            UpdateBoard();
        }
    }
}