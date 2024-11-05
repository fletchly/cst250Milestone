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
using System.Windows.Threading;

namespace MinesweeperGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Declare instance variables
        SetupWindow? Setup;
        Board Board;
        double ButtonSize;
        DispatcherTimer Timer;
        TimeSpan GameTime;

        /// <summary>
        /// Dictionary for highlighting colors
        /// </summary>
        private static readonly Dictionary<int, Color> HighlightLevels = new()
        {
            { 1, Color.FromRgb(25, 41, 250) },
            { 2, Color.FromRgb(72, 127, 30) },
            { 3, Color.FromRgb(251, 0, 6) },
            { 4, Color.FromRgb(0, 0, 109) },
            { 5, Color.FromRgb(107, 0, 1)},
            { 6, Color.FromRgb(14, 110, 109) },
            { 7, Color.FromRgb(0, 0, 0) },
            { 8, Color.FromRgb(109, 109, 109) }
        };

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            
            // Initialize default board
            Board = new Board(16, 40, 1);

            // Initialize timer
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(TimerTick);
            Timer.Interval = TimeSpan.FromSeconds(1);

            // Initialize TimeSpan
            GameTime = new TimeSpan();

            // Perform further initialization on game board
            InitializeBoardGrid();
        }

        /// <summary>
        /// Populate the board grid with appropriate number of buttons
        /// </summary>
        private void InitializeBoardGrid()
        {
            // Determine button size and ensure board is square
            ButtonSize = GrdBoard.Width / Board.Size;
            GrdBoard.Height = GrdBoard.Width;

            // Setup board rows and cols
            for (int size = 0; size < Board.Size; size++)
            {
                GrdBoard.RowDefinitions.Add(new RowDefinition());
                GrdBoard.ColumnDefinitions.Add(new ColumnDefinition());
                GrdBoard.RowDefinitions[size].Height = GridLength.Auto;
                GrdBoard.ColumnDefinitions[size].Width = GridLength.Auto;
            }

            // Populate board with cells
            foreach (Cell cell in Board.Cells)
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
                button.MouseRightButtonDown += new MouseButtonEventHandler(BtnCellRightClick);

                // Add button to board
                GrdBoard.Children.Add(button);
                
            }

            UpdateBoard();
        }

        /// <summary>
        /// Logic to update the board display
        /// </summary>
        private void UpdateBoard()
        {
            int row, col;
            Cell currentCell;
            Board.GameState state = Board.DetermineGameState();

            LblMines.Content = Board.FlagsLeft;
            LblRewards.Content = Board.Rewards;

            // Iterate over each button that needs updating
            foreach (Button button in GrdBoard.Children)
            {
                button.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                row = Grid.GetRow(button); col = Grid.GetColumn(button);
                currentCell = Board.Cells[row, col];

                // If a cell has been visited, disable it and show its contents
                if (currentCell.IsVisited)
                {
                    button.IsEnabled = false;

                    // Cell is a bomb
                    if (currentCell.IsBomb)
                    {
                        button.Content = new Image
                        {
                            Source = new BitmapImage(new Uri("/Icons/mine.png", UriKind.RelativeOrAbsolute))
                        };
                    }
                    // Cell is not a bomb
                    else
                    {
                        // Cell has special reward
                        if (currentCell.HasSpecialReward)
                        {
                            button.Content = new Image
                            {
                                Source = new BitmapImage(new Uri("/Icons/reward.png", UriKind.RelativeOrAbsolute))
                            };
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
                            button.Foreground = new SolidColorBrush(HighlightLevels[currentCell.Neighbors]);
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
                        button.Content = new Image
                        {
                            Source = new BitmapImage(new Uri("/Icons/flag.png", UriKind.RelativeOrAbsolute))
                        };
                    }
                    // Cell has not been flagged
                    else
                    {
                        button.Content = "?";
                    }
                }
            }

            // Determine game state and create a new game
            if(state == Board.GameState.Lost)
            {
                Timer.Stop();
                MessageBox.Show("Game Lost");
                NewGame();
            }
            else if (state == Board.GameState.Won)
            {
                Timer.Stop();
                MessageBox.Show("Game Won");
                NewGame();
            }
        }

        private void NewGame()
        {
            // Show a new setup window as a
            // dialog box attached to the main form
            Setup = new SetupWindow();
            Setup.Owner = this;
            Setup.ShowDialog();

            // Refresh the board with new game parameters
            GrdBoard.Children.Clear();
            Board = new Board(Setup.Size, Setup.Difficulty, 1);
            Timer.Stop();
            GameTime = TimeSpan.Zero;
            LblTimer.Content = GameTime.Seconds;
            InitializeBoardGrid();
        }

        // Event handlers

        /// <summary>
        /// New game button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNewGameClick(object sender, RoutedEventArgs e)
        {
            // Call the new game method
            NewGame();
        }

        /// <summary>
        /// Cell button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCellClick(object sender, RoutedEventArgs e)
        {
            // Cast the sender to a button
            Button clickedButton = (Button) sender;

            // Collect a special reward if it exists and has been visited
            if (Board.Cells[Grid.GetRow(clickedButton), Grid.GetColumn(clickedButton)].HasSpecialReward && Board.Cells[Grid.GetRow(clickedButton), Grid.GetColumn(clickedButton)].IsVisited)
            {
                Board.Cells[Grid.GetRow(clickedButton), Grid.GetColumn(clickedButton)].HasSpecialReward = false;
                Board.Rewards++;
            }

            // If the board has not been initialized yet (i.e. PreStart),
            // initialize
            if (Board.DetermineGameState() == Board.GameState.PreStart)
            {
                Board.InitializeBoard(Grid.GetRow(clickedButton), Grid.GetColumn(clickedButton));
                Timer.Start();
            }
            else
            {
                // Only allow a cell to be visited if it is not flagged
                if (!Board.Cells[Grid.GetRow(clickedButton), Grid.GetColumn(clickedButton)].IsFlagged)
                {
                    Board.Visit(Grid.GetRow(clickedButton), Grid.GetColumn(clickedButton));
                }
            }

            UpdateBoard();
        }

        /// <summary>
        /// Right click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCellRightClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;

            // Toggle the isFlagged property on the selected cell.
            Board.ToggleFlag(Grid.GetRow(clickedButton), Grid.GetColumn(clickedButton));

            UpdateBoard();
        }

        /// <summary>
        /// Timer tick event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object? sender, EventArgs e)
        {
            GameTime += TimeSpan.FromSeconds(1);
            LblTimer.Content = GameTime.Seconds.ToString();
        }
    }
}