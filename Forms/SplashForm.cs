using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace Game2048.Forms
{
    public partial class SplashForm : Form
    {
        private int _index = 1;
        private const int _countImage = 6;
        private ResourceManager resourceManager = Properties.Resources.ResourceManager;
        public SplashForm()
        {
            InitializeComponent();
            timer1.Start();
            timer2.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            this.Hide();
            var form = new StartForm().ShowDialog();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            _index += 1;
            if (_index <= _countImage)
            {
                BackgroundImage = (Bitmap)resourceManager.GetObject($"_{_index}");
            }
            else
            {
                _index = 1;
                BackgroundImage = (Bitmap)resourceManager.GetObject($"_{_index}");
            }
        }

        private void SplashForm_Load(object sender, EventArgs e)
        {
            BackgroundImage = (Bitmap)resourceManager.GetObject($"_{_index}");
        }
    }
}
