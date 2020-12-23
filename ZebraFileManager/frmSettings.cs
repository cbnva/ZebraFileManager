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
            ThreadPool.QueueUserWorkItem(new WaitCallback(x => {
                if (!printer.Connect())
                {
                    return;
                }

                var results = printer.RunCommand("{}{\"allconfig\":null}");
                var parsed = Newtonsoft.Json.JsonConvert.DeserializeObject(results) as Newtonsoft.Json.Linq.JObject;
                var settings = new List<Setting>();
                foreach (JProperty token in parsed["allconfig"]?.Children())
                {
                    var setting = token.First.ToObject<Setting>();
                    setting.Name = token.Name;
                    settings.Add(setting);
                }
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
            var row = dataGridView1.Rows[e.RowIndex];

        }
    }
}
