using Game2048.Forms;
using Game2048.Models;
using Game2048.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game2048
{
    public partial class GameBoardForm : Form
    {
        private ScoreRepository _repository;
        private int? _userId;
        public Game Game = new Game();
        public int _gameId;
        private int _hScore;
        private int _score;
        private UserMenuForm _userMenu;
        private DialogForm _dialogForm;


        public Dictionary<Keys, EControls> ControlsMap = new Dictionary<Keys, EControls>
        {
            { Keys.Up, EControls.Up },
            { Keys.Down, EControls.Down },
            { Keys.Right, EControls.Right },
            { Keys.Left, EControls.Left }
        };
        public GameBoardForm(string connectionString, int? UserId, UserMenuForm userMenu)
        {
            InitializeComponent();
            labelHighScore.Text = "Лучший счёт: 0";
            Game.isApprove = false;
            _repository = new ScoreRepository(connectionString);
            _userId = UserId;
            _userMenu = userMenu;
            Game.EvnUpdateDisplay += UpdateDisplay;
            Game.EvnCurrentScore += Score;
            Game.EvnHighScore += HighScore;
            Game.EvnIs2048 += IsWin;
            Game.EvnGenerateError += ShowMessage;
            Task T = InitGame();
        }

        private async void IsWin(int value)
        {
            _dialogForm = new DialogForm("Желаете ли продолжить игру или сохранить результат?", "Продолжить", "Сохранить");
            if (_dialogForm.ShowDialog() == DialogResult.Yes)
            {
                Game.isApprove = true;
            }
            else
            {
                await SaveScore(_gameId, _score, _hScore);
                this.Hide();
                _userMenu.Show();
            }
        }

        private async Task InitGame()
        {
            _hScore = 0;
            _score = 0;
            _hScore = await _repository.GetHighScore(_userId.Value);
            await _repository.AddAsync(new ScoreModel { UserId = _userId.Value, Score = 0 });
            _gameId = await _repository.GetLastId();
            Game.Generate(_hScore);
            labelHighScore.Text = $"Лучший счёт: {_hScore}";
        }

        private void UpdateDisplay(Tile[,] tiles)
        {
            for (var i = 0; i < Game.GRID_SIZE; i++)
            {
                for (var j = 0; j < Game.GRID_SIZE; j++)
                {
                    //Search  controls
                    var label = this.Controls.Find("lbl" + i + j, true).FirstOrDefault() as Label;
                    var panel = this.Controls.Find("pnl" + i + j, true).FirstOrDefault() as Panel;
                    //Update value and color
                    Tile tile = tiles[i, j];
                    label.Text = tile.Value > 0 ? tile.Value.ToString() : "";
                    panel.BackColor = GetTileColor(tile);
                    if (label.Text.Length > 4)
                        label.Font = new Font("Microsoft Sans Serif", 6F,
                            FontStyle.Bold, GraphicsUnit.Point);
                    else
                        label.Font = new Font("Microsoft Sans Serif", 10F,
                            FontStyle.Bold, GraphicsUnit.Point);
                }
            }
        }
        private void Score(int score)
        {
            _score = score;
            labelScore.Text = $@"Счёт: {score}";
        }
        private void HighScore(int score)
        {
            _hScore = score;
            labelHighScore.Text = $@"Лучший счёт: {score}";
        }

        private Color GetTileColor(Tile tile)
        {
            switch (tile.Value)
            {
                case 2:
                    return Color.FromArgb(238, 228, 218);
                case 4:
                    return Color.FromArgb(237, 224, 200);
                case 8:
                    return Color.FromArgb(242, 177, 121);
                case 16:
                    return Color.FromArgb(245, 149, 99);
                case 32:
                    return Color.FromArgb(246, 124, 95);
                case 64:
                    return Color.FromArgb(246, 94, 59);
                case 128:
                    return Color.FromArgb(237, 207, 114);
                case 256:
                    return Color.FromArgb(237, 204, 97);
                case 512:
                    return Color.FromArgb(237, 200, 80);
                case 1024:
                    return Color.FromArgb(237, 197, 63);
                case 2048:
                    return Color.FromArgb(237, 194, 46);
                default:
                    return Color.FromArgb(205, 193, 180);
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            EControls controls;
            controls = ControlsMap.TryGetValue(e.KeyData, out controls) ? controls : EControls.Invalid;
            Game.Move(controls);
        }

        private async void btnNewGame_Click(object sender, EventArgs e)
        {
            await SaveScore(_gameId, _score, _hScore);
            await InitGame();
        }

        private static void ShowMessage()
        {
            MessageBox.Show(@"Конец игры! У вас закончились свободные места.",
                @"Конец игры",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private async Task SaveScore(int gameId, int score, int hScore)
        {
            if (score <= hScore)
            {
                await UpdateScore(gameId, score);
            }
            else if(Game.isApprove)
            {
                await UpdateScore(gameId, score);
            }
        }

        private async Task UpdateScore(int gameId, int score)
        {
            var data = new ScoreModel
            {
                Id = gameId,
                UserId = _userId.Value,
                Score = score
            };
            await _repository.UpdateAsync(data);
        }

        private async void labelExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this,
                            "Действительно хотите выйти?",
                            "Выход в пользовательское меню",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Question) == DialogResult.OK)
            {
                await SaveScore(_gameId, _score, _hScore);
                this.Hide();
                _userMenu.Show();
            }
        }
    }
}