using Game2048.Forms;
using Game2048.Repositories;
using System;
using System.Windows.Forms;

namespace Game2048
{
    public partial class StartForm : BaseForm
    {
        private readonly string _connectionString;
        public int? UserId;
        private BaseSqliteRepository _repository;
        private WriteLoginForm _loginForm;
        private ScoreForm _scoreForm;
        private InformationForm _informationForm;
        public StartForm()
        {
            InitializeComponent();
            UserId = null;
            _connectionString = "DataSource=Game2048.db";
            _repository = new BaseSqliteRepository
            {
                ConnectionString = _connectionString
            };
            _repository.CreateDataBase();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonScore_Click(object sender, EventArgs e)
        {
            _scoreForm = new ScoreForm(this,_connectionString, UserId);
            this.Hide();
            _scoreForm.Show();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            _loginForm = new WriteLoginForm(this, _connectionString, true);
            UserId = _loginForm.userId;
            this.Hide();
            _loginForm.Show();
        }

        private void buttonRegistr_Click(object sender, EventArgs e)
        {
            _loginForm = new WriteLoginForm(this, _connectionString, false);
            this.Hide();
            _loginForm.Show();
        }

        private void buttonInformation_Click(object sender, EventArgs e)
        {
            _informationForm = new InformationForm();
            this.Hide();
            _informationForm.Show();
        }
    }
}