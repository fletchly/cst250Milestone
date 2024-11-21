using MinesweeperClassLibrary.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ServiceStack.Text;
using static System.Formats.Asn1.AsnWriter;
using System.Security;

namespace MinesweeperClassLibrary.Data
{
    internal class ScoreDao
    {
        // Scores collection
        private List<GameStats> _scores;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScoreDao()
        {
            _scores = new List<GameStats>();
        }

        /// <summary>
        /// Add score to scoreboard
        /// </summary>
        /// <param name="stats"></param>
        /// <returns></returns>
        public bool AddScore(GameStats stats)
        {
            // Insert score into scoreboard
            var initialCount = _scores.Count;
            _scores.Add(stats);
            return (_scores.Count == initialCount + 1);
        }

        /// <summary>
        /// Get all scores
        /// </summary>
        /// <returns></returns>
        public List<GameStats> GetAllScores()
        {
            return _scores;
        }

        /// <summary>
        /// Save scores to file
        /// </summary>
        /// <returns></returns>
        public bool Save(string fileName)
        {
            // Declare and initialize
            string json = "", csv = "", scoreString = "";
            string[] scores = Array.Empty<string>();

            // Use a try/catch to handle exceptions
            try
            {
                if (fileName.EndsWith(".json"))
                {
                    // Serialize data list into JSON formatted string
                    json = ServiceStack.Text.JsonSerializer.SerializeToString(_scores);
                    File.WriteAllText(fileName, json);
                }
                else if (fileName.EndsWith(".csv"))
                {
                    // Serialize the data list into CSV formatted string
                    csv = CsvSerializer.SerializeToString(_scores);
                    File.WriteAllText(fileName, csv);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Load scores from file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Load(string fileName)
        {
            using (var file = File.OpenRead(fileName))
            {
                // Use a try/catch to handle exceptions
                try
                {
                    if (fileName.EndsWith(".json"))
                    {
                        _scores.AddRange(ServiceStack.Text.JsonSerializer.DeserializeFromStream<List<GameStats>>(file));
                    }
                    else if (fileName.EndsWith(".csv"))
                    {
                        _scores.AddRange(CsvSerializer.DeserializeFromStream<List<GameStats>>(file));
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            return true;
            }
        }

        /// <summary>
        /// Sort score list
        /// </summary>
        /// <param name="descending"></param>
        public void SortScores(bool descending)
        {
            if (!descending)
            {
                _scores = _scores.OrderBy(s => s.Time).ToList();
            }
            else
            {
                _scores = _scores.OrderByDescending(s => s.Time).ToList();
            }
        }
    }
}
