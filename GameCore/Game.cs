using System;
using System.Collections.Generic;
using System.Drawing;

namespace Game2048
{
    public class Game
    {
        #region Переменные
        public const int GRID_SIZE = 4;
        public Tile[,] State = new Tile[GRID_SIZE, GRID_SIZE];
        public Tile[,] StatePrev = new Tile[GRID_SIZE, GRID_SIZE];
        public int Score = 0;
        public int ScorePrev;
        public bool isBlock = false;
        public static bool isApprove = false;
        public int InitTiles { get; set; }                  // Количество стартовых ячеек (обычно две)
        #endregion
        #region Делегаты и события
        public delegate void UpdateDisplay(Tile[,] tiles);
        public event UpdateDisplay EvnUpdateDisplay;

        public delegate void CurrentScore(int score);
        public event CurrentScore EvnCurrentScore;

        public delegate void UpdHighScore(int score);
        public event UpdHighScore EvnHighScore;

        public delegate void Is2048(int value);
        public event Is2048 EvnIs2048;
        // Generate Error ~ Show an error message when generating a new cell fails
        public delegate void GenerateError();
        public event GenerateError EvnGenerateError;
        #endregion
        private int _highScore;
        public int HighScore
        {
            get
            {
                return _highScore;
            }
            set
            {
                EvnHighScore?.Invoke(_highScore);
            }

        }
        public Game()
        {
            InitTiles = 2;
            Movement.EvnUpdateScore += UpdateScore;
        }

        public void Generate(int hScore)
        {
            for (var i = 0; i < GRID_SIZE; i++)
            {
                for (var j = 0; j < GRID_SIZE; j++)
                {
                    State[i, j] = new Tile();
                }
            }
            for (var i = 0; i < InitTiles; i++)
            {
                isBlock = true;
                AddRandomTile();
            }
            Score = 0;
            EvnCurrentScore?.Invoke(Score);
            _highScore = hScore;
        }

        public void AddRandomTile()
        {
            if (!isBlock) return;
            var rnd = new Random();
            var value = rnd.Next(11) < 9 ? 2 : 4;
            var emptyTiles = new List<Point>();
            for (var i = 0; i < GRID_SIZE; i++)
            {
                for (var j = 0; j < GRID_SIZE; j++)
                {
                    if (State[i, j].Value == 0)
                    {
                        emptyTiles.Add(new Point(i, j));
                    }
                }
            }
            if (emptyTiles.Count > 0)
            {
                Point randomTitle = emptyTiles[rnd.Next(emptyTiles.Count)];
                State[randomTitle.X, randomTitle.Y] = new Tile(value);
            }
            EvnUpdateDisplay?.Invoke(State);
            isBlock = false;
        }

        public void Move(EControls controls)
        {
            switch (controls)
            {
                case EControls.Left:
                    Movement.MoveTilesLeft(State, ref isBlock);
                    ControlGame();
                    break;
                case EControls.Right:
                    Movement.MoveTilesRight(State, ref isBlock);
                    ControlGame();
                    break;
                case EControls.Up:
                    Movement.MoveTilesUp(State, ref isBlock);
                    ControlGame();
                    break;
                case EControls.Down:
                    Movement.MoveTilesDown(State, ref isBlock);
                    ControlGame();
                    break;
            }
        }

        private void ControlGame()
        {
            AddRandomTile();
            IsWin();
            EvnUpdateDisplay?.Invoke(State);
            if (IsGameOwer())
            {
                EvnGenerateError?.Invoke();
            }
        }

        public bool IsGameOwer()
        {
            for (var i = 0; i < GRID_SIZE; i++)
            {
                for (var j = 0; j < GRID_SIZE; j++)
                {
                    Tile tile = State[i, j];
                    if (tile.Value == 0 || (i > 0 && State[i - 1, j].Value == tile.Value) ||
                        (i < GRID_SIZE - 1 && State[i + 1, j].Value == tile.Value) ||
                        (j > 0 && State[i, j - 1].Value == tile.Value) ||
                        (j < GRID_SIZE - 1 && State[i, j + 1].Value == tile.Value))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void IsWin()
        {
            for (var i = 0; i < GRID_SIZE; i++)
            {
                for (var j = 0; j < GRID_SIZE; j++)
                {
                    Tile tile = State[i, j];
                    if (tile.Value == 2048 && !isApprove)
                    {
                        EvnIs2048?.Invoke(tile.Value);
                        while (isApprove)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void UpdateScore(int score)
        {
            Score += score;
            EvnCurrentScore?.Invoke(Score);
            if (Score <= _highScore) return;
            _highScore = Score;
            EvnHighScore?.Invoke(_highScore);
        }
    }
}