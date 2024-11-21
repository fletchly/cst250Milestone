using Microsoft.Win32;
using MinesweeperClassLibrary.Business;
using MinesweeperClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
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
    /// Interaction logic for ScoreboardWindow.xaml
    /// </summary>
    public partial class ScoreboardWindow : Window
    {
        // Class-level properties
        public int Time { get; init; }
        public int Size { get; init; }
        public int Difficulty { get; init; }
        public int Rewards { get; init; }
        public DateTime Date { get; init; }

        // Instance variables
        string playerName = "";
        GameStatService gameStatService = new GameStatService();
        Binding binding = new Binding();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="time"></param>
        /// <param name="rewards"></param>
        /// <param name="date"></param>
        /// <param name="size"></param>
        /// <param name="difficulty"></param>
        public ScoreboardWindow(int time, int rewards, DateTime date, int size, int difficulty)
        {
            InitializeComponent();
            Time = time;
            Rewards = rewards;
            Date = date;
            Size = size;
            Difficulty = difficulty;
        }

        /// <summary>
        /// Get name from user input
        /// </summary>
        /// <returns></returns>
        private string GetName()
        {
            var nameInput = new InputWindow();
            nameInput.Owner = this;
            nameInput.ShowDialog();
            return nameInput.PlayerName;
        }

        /// <summary>
        /// Update scoreboard with new data
        /// </summary>
        private void UpdateScoreboard()
        {
            DgvScores.ItemsSource = gameStatService.GetAllScores();
            DgvScores.Items.Refresh();
        }

        /// <summary>
        /// Save Click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MniSaveClick(object sender, RoutedEventArgs e)
        {
            // Declare and initialize
            string fileName = "";

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "scores";
            sfd.DefaultExt = ".json";
            sfd.Filter = "CSV file (*.csv)|*.csv|JSON file (*.json)|*.json";

            bool? result = sfd.ShowDialog();

            if (result == true)
            {
                fileName = sfd.FileName;
            }

            gameStatService.Save(fileName);
        }

        /// <summary>
        /// Load click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MniLoadClick(object sender, RoutedEventArgs e)
        {
            // Declare and initialize
            string fileName = "";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "scores";
            ofd.DefaultExt = ".json";
            ofd.Filter = "CSV file (*.csv)|*.csv|JSON file (*.json)|*.json";

            bool? result = ofd.ShowDialog();

            if (result == true)
            {
                fileName = ofd.FileName;
            }

            gameStatService.Load(fileName);
            UpdateScoreboard();
        }

        /// <summary>
        /// Sort ascending button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MniAscClick(object sender, RoutedEventArgs e)
        {
            gameStatService.SortScores(false);
            UpdateScoreboard();
        }

        /// <summary>
        /// Sort descending button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MniDescClick(object sender, RoutedEventArgs e)
        {
            gameStatService.SortScores(true);
            UpdateScoreboard();
        }

        /// <summary>
        /// Scoreboard window load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScoreBoardWindowLoad(object sender, RoutedEventArgs e)
        {
            playerName = GetName();
            GameStats score = new GameStats(playerName, Time, Rewards, Size, Difficulty, Date);
            gameStatService.AddScore(score);
            UpdateScoreboard();
        }
    }
}
