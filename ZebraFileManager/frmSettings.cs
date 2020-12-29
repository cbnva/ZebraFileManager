using Library.Forms;
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
        SortableBindingList<Setting> settings;
        List<Setting> changedSettings;
        public frmSettings(Printer printer)
        {
            InitializeComponent();
            this.printer = printer;
            this.changedSettings = new List<Setting>();
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

                if (settings != null)
                {
                    settings.ListChanged -= Settings_ListChanged;
                }

                changedSettings.Clear();
                settings = new SortableBindingList<Setting>(printer.GetSettings());
                settings.ListChanged += Settings_ListChanged;

                Invoke(new Action(() => { Filter(); btnSave.Text = $"Save ({changedSettings.Count})"; }));
            }));
        }

        private void Settings_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged && e.PropertyDescriptor.Name == "Value")
            {
                var setting = settings[e.OldIndex];
                if (!changedSettings.Contains(setting))
                {
                    changedSettings.Add(setting);
                    Invoke(new Action(() => btnSave.Text = $"Save ({changedSettings.Count})"));
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BeginReloadSettings();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
            {
                foreach (var setting in changedSettings)
                {
                    printer.SetSettingSGD(setting.Name, setting.Value);
                }
                changedSettings.Clear();
                Invoke(new Action(() => btnSave.Text = $"Save ({changedSettings.Count})"));
                BeginReloadSettings();
            }));
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == this.valueDataGridViewTextBoxColumn.Index && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].IsInEditMode)
            {
                var setting = dataGridView1.Rows[e.RowIndex].DataBoundItem as Setting;
                if (!setting.IsValidValue(e.FormattedValue))
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText = "Invalid Value";
                    e.Cancel = true;
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText = "";
                }

            }
        }

        private void btnExportChangedAsZPL_Click(object sender, EventArgs e)
        {
            if (changedSettings.Count != 0)
            {
                var sb = new StringBuilder("! U ");
                foreach (var item in changedSettings)
                {
                    sb.AppendLine($"setvar \"{item.Name}\" \"{item.Value}\"");
                }
                sb.AppendLine("END ");

                using (var fd = new SaveFileDialog())
                {
                    fd.Filter = ".zpl file (*.zpl)|*.zpl";
                    if (fd.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.File.WriteAllText(fd.FileName, sb.ToString());
                    }
                }
            }

        }

        private void btnExportNonDefaults_Click(object sender, EventArgs e)
        {
            var nonDefaultSettings = settings.Where(x => x.Access == SettingAccess.RW && x.Value != x.Default).ToList();
            if (nonDefaultSettings.Count != 0)
            {
                var sb = new StringBuilder("! U ");
                foreach (var item in nonDefaultSettings)
                {
                    sb.AppendLine($"setvar \"{item.Name}\" \"{item.Value}\"");
                }
                sb.AppendLine("END ");

                using (var fd = new SaveFileDialog())
                {
                    fd.Filter = ".zpl file (*.zpl)|*.zpl";
                    if (fd.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.File.WriteAllText(fd.FileName, sb.ToString());
                    }
                }
            }

        }


        System.Threading.Timer filterTimer;
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (filterTimer == null)
                filterTimer = new System.Threading.Timer(x => Filter());
            filterTimer.Change(500, Timeout.Infinite);
        }
        object filterLock = new object();
        private void Filter()
        {
            lock (filterLock)
            {
                var oldList = dataGridView1.DataSource as SortableBindingList<Setting>;
                if (!string.IsNullOrWhiteSpace(txtFilter.Text))
                {
                    var filtered = new SortableBindingList<Setting>(settings.Where(x => x.Name.ToLower().Contains(txtFilter.Text.ToLower())).ToList());
                    if (oldList != null)
                        filtered.CopySortingFrom(oldList);

                    Invoke(new Action<SortableBindingList<Setting>>(x => dataGridView1.DataSource = x), filtered);
                }
                else
                {
                    if (oldList != null)
                        settings.CopySortingFrom(oldList);

                    Invoke(new Action<SortableBindingList<Setting>>(x => dataGridView1.DataSource = x), settings);
                }
            }
        }
    }
}
