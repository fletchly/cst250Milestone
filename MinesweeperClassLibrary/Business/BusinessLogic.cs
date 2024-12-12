using MinesweeperClassLibrary.Model;

namespace MinesweeperClassLibrary.Business;

public class BusinessLogic
{
    private Board _board = new Board(DefaultBoardSize, DefaultDifficulty, DefaultRewardLimit);
    public TimeSpan GameTime { get; set; }
    public bool GameInProgress { get; set; }

    private int _score = 0;

    private const int DefaultBoardSize = 16;
    private const int DefaultDifficulty = 40;
    private const int DefaultRewardLimit = 1;
    
    /// <summary>
    /// Setup default board
    /// </summary>
    public void StartDefaultGame()
    {
        GameInProgress = false;
        _board = new Board(DefaultBoardSize, DefaultDifficulty, DefaultRewardLimit);
        GameTime = TimeSpan.Zero;
        _score = 0;
    }

    /// <summary>
    /// Setup new board
    /// </summary>
    /// <param name="size"></param>
    /// <param name="difficulty"></param>
    public void StartCustomGame(int size, int difficulty)
    {
        GameInProgress = false;
        _board = new Board(size, difficulty, DefaultRewardLimit);
        GameTime = TimeSpan.Zero;
        _score = 0;
    }

    /// <summary>
    /// Take action on a cell
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    public void VisitCell(int row, int column)
    {
        // Collect special reward if one exists
        if (_board.Cells[row, column].HasSpecialReward && _board.Cells[row, column].IsVisited)
        {
            _board.Cells[row, column].HasSpecialReward = false;
            _board.Rewards++;
        }

        // If the game is in PreStart, initialize the board
        if (_board.DetermineGameState() == Board.GameState.PreStart)
        {
            _board.InitializeBoard(row, column);
            GameInProgress = true;
        }
        else
        {
            if (!_board.Cells[row, column].IsFlagged)
            {
                _board.Visit(row, column);
            }
        }
    }

    /// <summary>
    /// Toggle flag on specific cell
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    public void ToggleFlagOnCell(int row, int column)
    {
        _board.ToggleFlag(row, column);
    }
    
    /// <summary>
    /// Get board size
    /// </summary>
    /// <returns></returns>
    public int GetBoardSize()
    {
        return _board.Size;
    }

    /// <summary>
    /// Get Cells from board
    /// </summary>
    /// <returns></returns>
    public Cell[,] GetBoardCells()
    {
        return _board.Cells;
    }

    /// <summary>
    /// get game state from board
    /// </summary>
    /// <returns></returns>
    public Board.GameState GetGameState()
    {
        return _board.DetermineGameState();
    }

    /// <summary>
    /// Get remaining flags from board
    /// </summary>
    /// <returns></returns>
    public int GetFlags()
    {
        return _board.FlagsLeft;
    }

    /// <summary>
    /// Get current rewards from board
    /// </summary>
    /// <returns></returns>
    public int GetRewards()
    {
        return _board.Rewards;
    }

    /// <summary>
    /// Get board difficulty
    /// </summary>
    /// <returns></returns>
    public int GetDifficulty()
    {
        return _board.Difficulty;
    }

    /// <summary>
    /// Increment the score
    /// </summary>
    public void IncTimer()
    {
        _score++;
    }

    /// <summary>
    /// Return the score
    /// </summary>
    /// <returns></returns>
    public int GetTimer()
    {
        return _score;
    }

}