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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnNewGameClick(object sender, RoutedEventArgs e)
        {
            setup = new SetupWindow();
            setup.Owner = this;
            setup.ShowDialog();
        }
    }
}