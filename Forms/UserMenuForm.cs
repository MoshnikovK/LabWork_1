using Game2048.Repositories;
using System;
using System.Windows.Forms;

namespace Game2048.Forms
{
    public partial class UserMenuForm : BaseForm
    {
        private readonly ScoreRepository _repository;
        private StartForm _startForm;
        private ScoreForm _scoreForm;
        private GameBoardForm _gameBoardForm;
        private int? _userId;
        public UserMenuForm(string connectionString, int userId, StartForm start)
        {
            InitializeComponent();
            _repository = new ScoreRepository(connectionString);
            _userId = userId;
            _startForm = start;
        }

        private void buttonNewGame_Click(object sender, EventArgs e)
        {
            _gameBoardForm = new GameBoardForm(_repository.ConnectionString, _userId, this);
            _gameBoardForm.Show();
            this.Hide();
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            var _dialog = MessageBox.Show("Действительно хотите выйти?",
                "Выход в главное меню",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);
            if (_dialog == DialogResult.OK)
            {
                _startForm.UserId = null;
                _startForm.Show();
                this.Hide();
            }
        }

        private void buttonScore_Click(object sender, EventArgs e)
        {
            _scoreForm = new ScoreForm(this, _repository.ConnectionString, _userId);
            _scoreForm.Show();
            this.Hide();
        }
    }
}