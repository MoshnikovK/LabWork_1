using System.Data;
using System.Windows.Forms;

namespace Game2048.Forms
{
    public class BaseForm : Form
    {
        protected void NoDataShow(DataGridView dataGrid, Panel panel, Panel panel1)
        {
            dataGrid.Visible = false;
            panel.Visible = true;
            panel1.Visible = false;
        }

        protected void RefreshGrid(DataGridView dataGrid, Panel panel, Panel panel1, DataSet dataSet)
        {
            dataGrid.Visible = true;
            panel1.Visible = true;
            panel.Visible = false;
            var bs = new BindingSource
            {
                DataSource = dataSet.Tables[0]
            };
            dataGrid.DataSource = bs;
            dataGrid.Refresh();
        }
    }
}