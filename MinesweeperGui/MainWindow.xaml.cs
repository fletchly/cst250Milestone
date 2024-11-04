using Microsoft.Win32.SafeHandles;
using MinesweeperClassLibrary;
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
        SetupWindow? setup;
        Board board;
        Button[,] buttons;

        double ButtonSize;


        public MainWindow()
        {
            InitializeComponent();
            board = new Board(16, 40, 1);
            InitializeBoardGrid();
        }

        private void InitializeBoardGrid()
        {
            ButtonSize = GrdBoard.Width / board.Size;
            GrdBoard.Height = GrdBoard.Width;

            for (int size = 0; size < board.Size; size++)
            {
                GrdBoard.RowDefinitions.Add(new RowDefinition());
                GrdBoard.ColumnDefinitions.Add(new ColumnDefinition());
                GrdBoard.RowDefinitions[size].Height = GridLength.Auto;
                GrdBoard.ColumnDefinitions[size].Width = GridLength.Auto;
            }

            foreach (Cell cell in board.Cells)
            {
                Button button = new Button();
                Grid.SetRow(button, cell.Row);
                Grid.SetColumn(button, cell.Column);
                button.Width = ButtonSize;
                button.Height = ButtonSize;

                GrdBoard.Children.Add(button);
                
            }
        }

        // Event handlers
        private void BtnNewGameClick(object sender, RoutedEventArgs e)
        {
            setup = new SetupWindow();
            setup.Owner = this;
            setup.ShowDialog();
            GrdBoard.Children.Clear();
            board = new Board(setup.Size, setup.Difficulty, 1);
            InitializeBoardGrid();
        }
    }
}