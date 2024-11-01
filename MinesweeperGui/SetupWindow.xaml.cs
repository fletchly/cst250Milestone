using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MinesweeperGui
{
    /// <summary>
    /// Interaction logic for SetupWindow.xaml
    /// </summary>
    public partial class SetupWindow : Window
    {
        public int Size;
        public int Difficulty;

        public SetupWindow()
        {
            InitializeComponent();
            Size = 16;
            Difficulty = 40;
            SldSize.Value = Size;
            SldDifficulty.Maximum = Math.Pow(Size - 1, 2);
            SldDifficulty.Value = Difficulty;
        }

        private void UpdateCustomContent()
        {
            if (BtnCustom != null)
            {
                BtnCustom.Content = $"Custom ({SldSize.Value}x{SldSize.Value} {SldDifficulty.Value} mines)";
            }
            
        }
        
        // Event handlers

        /// <summary>
        /// SldSize value changed event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SldSizeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Set the maximum difficulty to be within the
            // parameters specified in Board.cs
            if (SldDifficulty != null)
            {
                SldDifficulty.Maximum = Math.Pow(SldSize.Value - 1, 2);
                UpdateCustomContent();
            }
        }

        /// <summary>
        /// Difficulty slider changed event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SldDifficultyChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateCustomContent();
        }

        /// <summary>
        /// Easy button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEasyClick(object sender, RoutedEventArgs e)
        {
            Size = 9;
            Difficulty = 10;
            Close();
        }

        /// <summary>
        /// Medium button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMediumClick(object sender, RoutedEventArgs e)
        {
            Size = 16;
            Difficulty = 40;
            Close();
        }

        /// <summary>
        /// Hard button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnHardClick(object sender, RoutedEventArgs e)
        {
            Size = 30;
            Difficulty = 270;
            Close();
        }

        /// <summary>
        /// Custom button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCustomClick(object sender, RoutedEventArgs e)
        {
            Size = (int) SldSize.Value;
            Difficulty = (int) SldDifficulty.Value;
            Close();
        }
    }
}
