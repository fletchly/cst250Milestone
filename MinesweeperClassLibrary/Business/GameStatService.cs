using MinesweeperClassLibrary.Data;
using MinesweeperClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperClassLibrary.Business
{
    public class GameStatService
    {
        ScoreDao scoreDao = new ScoreDao();

        /// <summary>
        /// Add score to list
        /// </summary>
        /// <param name="gameStats"></param>
        /// <returns></returns>
        public bool AddScore(GameStats gameStats)
        {
            return scoreDao.AddScore(gameStats);
        }

        /// <summary>
        /// Get all scores
        /// </summary>
        /// <returns></returns>
        public List<GameStats> GetAllScores()
        {
            return scoreDao.GetAllScores();
        }

        /// <summary>
        /// Save scores
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Save(string fileName)
        { 
            return scoreDao.Save(fileName);
        }

        /// <summary>
        /// Load scores
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Load(string fileName)
        {
            return scoreDao.Load(fileName);
        }

        /// <summary>
        /// Sort score list
        /// </summary>
        /// <param name="descending"></param>
        public void SortScores(bool descending)
        {
            scoreDao.SortScores(descending);
        }
    }
}
