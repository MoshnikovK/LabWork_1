using Game2048.Convertes;
using Game2048.Forms;
using Game2048.Models;
using Game2048.Repositories;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Game2048
{
    public partial class ScoreForm : BaseForm
    {
        private readonly ScoreRepository _repository;
        private int? _userId;
        private DataSet _dataSet;
        private BaseForm _startForm;
        public ScoreForm(BaseForm baseForm, string connectionString, int? UserId)
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            _repository = new ScoreRepository(connectionString);
            _startForm = baseForm;
            _userId = UserId;
        }

        private async void ScoreForm_Load(object sender, EventArgs e)
        {
            await CheckUserResult();
        }

        private async Task GetAllScores()
        {
            var allScores = await _repository.GetAllScores();
            if (allScores != null)
            {
                _dataSet = ConvertToDs.ConvertToDataSet(allScores.ToList<ScoreModel>(), "Score");
                RefreshGrid(dataGridView1, panel2, panel3, _dataSet);
            }
            else
            {
                NoDataShow(dataGridView1, panel2, panel3);
            }
        }

        private async Task GetUserScore(int? id)
        {
            var data = await _repository.GetByUserId(id.Value);
            if (data != null)
            {
                _dataSet = ConvertToDs.ConvertToDataSet(data.ToList<ScoreModel>(), "Score");
                RefreshGrid(dataGridView1, panel2, panel3, _dataSet);
            }
            else
            {
                NoDataShow(dataGridView1, panel2, panel3);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            _startForm.Show();
            this.Hide();
        }

        private async void checkBoxAllResult_CheckedChanged(object sender, EventArgs e)
        {
            await CheckUserResult();
        }

        private async Task CheckUserResult()
        {
            if (_userId == null)
            {
                checkBoxAllResult.Visible = false;
                checkBoxAllResult.Checked = false;
                await GetAllScores();
            }
            else
            {
                checkBoxAllResult.Visible = true;
                if (checkBoxAllResult.Checked)
                {
                    await GetUserScore(_userId.Value);
                }
                else
                {
                    await GetAllScores();
                }
            }
        }
    }
}