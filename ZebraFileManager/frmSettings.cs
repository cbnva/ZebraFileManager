using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZebraFileManager.Zebra;

namespace ZebraFileManager
{
    public partial class frmSettings : Form
    {
        Printer printer;
        public frmSettings(Printer printer)
        {
            InitializeComponent();
            this.printer = printer;
            BeginReloadSettings();
        }

        private void BeginReloadSettings()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
            {
                if (!printer.Connect())
                {
                    return;
                }

                var settings = printer.GetSettings();

                Invoke(new Action(() => dataGridView1.DataSource = settings));
            }));
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BeginReloadSettings();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
            {
                var row = dataGridView1.Rows[i];

                var setting = row.DataBoundItem as Setting;
                if (setting != null)
                {
                    var oldCell = row.Cells[valueDataGridViewTextBoxColumn.Index];
                    DataGridViewCell newCell = null;
                    switch (setting.Type)
                    {
                        case SettingType.Enum:
                            var range = setting.Range.Split(',').ToList();
                            range.Insert(0, "");
                            newCell = new DataGridViewComboBoxCell()
                            {
                                DataSource = range
                            };
                            break;
                        case SettingType.Bool:
                            newCell = new DataGridViewCheckBoxCell();
                            break;
                        case SettingType.Integer:
                            newCell = new DataGridViewTextBoxCell();
                            break;
                        case SettingType.IPV4_Address:
                            newCell = new DataGridViewTextBoxCell();
                            break;
                        case SettingType.Double:
                            newCell = new DataGridViewTextBoxCell();
                            break;
                        default:
                            newCell = new DataGridViewTextBoxCell();
                            break;
                    }
                    if (newCell != null)
                    {
                        row.Cells[valueDataGridViewTextBoxColumn.Index] = newCell;
                    }
                    row.Cells[valueDataGridViewTextBoxColumn.Index].ReadOnly = setting.Access == SettingAccess.R;
                }
            }

        }
    }
}
