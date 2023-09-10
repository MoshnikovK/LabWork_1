using System.Windows.Forms;

namespace Game2048.Forms
{
    public partial class DialogForm : Form
    {
        public DialogForm()
        {
            InitializeComponent();   
        }
        public DialogForm(string message, string buttonText1, string buttonText2) : this()
        {
            label1.Text = message;
            buttonYes.Text = buttonText1;
            buttonNo.Text = buttonText2;
        }
    }
}