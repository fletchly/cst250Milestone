using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperClassLibrary.Model
{
    public class GameStats
    {
        // Class-level properties
        public string Name { get; set; }
        public int Score { get; set; }
        public int Time { get; set; }
        public int Rewards { get; set; }
        public int Size { get; set; }
        public int Difficulty { get; set; }
        public DateTime Date { get; set; }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="time"></param>
        /// <param name="rewards"></param>
        /// <param name="size"></param>
        /// <param name="difficulty"></param>
        /// <param name="date"></param>
        public GameStats(string name, int time, int rewards, int size, int difficulty, DateTime date)
        {
            Name = name;
            Time = time;
            Rewards = rewards;
            Size = size;
            Difficulty = difficulty;
            Date = date;


            // Calculate composite score and ensure it doesn't drop below 0
            Score = (int) (1000m - (1400m / 3m) * (1m / (difficulty + size)) * time);
            if (Score < 0)
            {
                Score = 0;
            }
        }
    }
}
