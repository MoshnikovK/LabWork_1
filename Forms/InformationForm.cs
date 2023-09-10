using System;
using System.Windows.Forms;

namespace Game2048.Forms
{
    public partial class InformationForm : Form
    {
        private StartForm _startForm;
        public InformationForm()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            _startForm = new StartForm();
            _startForm.Show();
            this.Hide();
        }
    }
}