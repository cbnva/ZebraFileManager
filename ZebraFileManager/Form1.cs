using ZebraFileManager.Zebra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using File = ZebraFileManager.Zebra.File;
using Microsoft.Win32;
using RJCP.IO.Ports;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using Library.Forms;

namespace ZebraFileManager
{
    public partial class Form1 : Form
    {
        private ListViewColumnSorter lvwColumnSorter;
        private Dictionary<Printer, StringBuilder> messageLogWithoutBinary = new Dictionary<Printer, StringBuilder>();
        private Dictionary<Printer, StringBuilder> messageLogWithBinary = new Dictionary<Printer, StringBuilder>();

        public Form1()
        {
            InitializeComponent();

            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;
            RefreshBTPorts();
            RefreshUSBPorts();
        }

        private void RefreshBTPorts()
        {
            ThreadPool.QueueUserWorkItem(y =>
            {
                var ports = RJCP.IO.Ports.SerialPortStream.GetPortDescriptions().Where(x => !x.Description.ToLower().StartsWith("standard serial over bluetooth link")).ToList();
                ports.AddRange(GetBTPorts());
                Invoke(new Action<List<PortDescription>>(x =>
                {
                    comboBox1.DisplayMember = "Description";
                    comboBox1.DataSource = x;
                }), ports);
            });
        }
        private void RefreshUSBPorts()
        {
            ThreadPool.QueueUserWorkItem(y =>
            {
                var ports = USBPrinter.GetUSBPrinterPorts();

                Invoke(new Action<List<string>>(x =>
                {
                    cbUSB.DataSource = x;
                }), ports);
            });
        }

        List<PortDescription> GetBTPorts()
        {
            var ports = new List<PortDescription>();
            var regex = new Regex(@"^[a-fA-F0-9]&[a-fA-F0-9]{7,8}&[a-fA-F0-9]&(?<MAC>[a-fA-F0-9]{12})_[a-fA-F0-9]+$", RegexOptions.Compiled);
            using (var bthenum = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum\\BTHENUM"))
            {
                if (bthenum != null)
                {
                    var keynames = bthenum.GetSubKeyNames();
                    foreach (var btDeviceName in keynames.Where(x => x.StartsWith("{00001101-0000-1000-8000-00805f9b34fb}")))
                    {
                        using (var btDevKey = bthenum.OpenSubKey(btDeviceName)) // BTHENUM\{GUID}*
                        {
                            foreach (var subkeyName in btDevKey.GetSubKeyNames().Where(x => regex.IsMatch(x) && keynames.Contains($"Dev_{regex.Match(x).Groups["MAC"].Value}")))
                            {
                                using (var devParam = btDevKey.OpenSubKey(subkeyName + "\\Device Parameters")) // BTHENUM\{GUID}*\aeouaoeu\Device Parameters
                                {
                                    var portName = devParam.GetValue("PortName") as string;

                                    if (string.IsNullOrEmpty(portName))
                                        continue;

                                    var mac = regex.Match(subkeyName).Groups["MAC"].Value;
                                    using (var rawDevKey = bthenum.OpenSubKey($"Dev_{mac}"))
                                    {
                                        var name = rawDevKey.GetSubKeyNames().Where(x => x.ToUpper().EndsWith(mac.ToUpper())).FirstOrDefault();
                                        if (!string.IsNullOrEmpty(name))
                                        {
                                            using (var rawDevSubKey = rawDevKey.OpenSubKey(name))
                                            {
                                                var friendlyName = rawDevSubKey.GetValue("FriendlyName") as string;
                                                if (!string.IsNullOrEmpty(friendlyName))
                                                {
                                                    ports.Add(new PortDescription(portName, $"{portName} - {friendlyName} - {mac}"));
                                                }
                                            }
                                        }
                                    }


                                }
                            }
                        }
                    }
                }
            }

            return ports;
        }

        Printer CurrentPrinter
        {
            get
            {
                if (InvokeRequired)
                {
                    return (Printer)(Invoke(new Func<Printer>(() => CurrentPrinter)));
                }
                return treeView1.SelectedNode?.Tag as Printer ?? treeView1.SelectedNode?.Parent?.Tag as Printer;
            }
        }
        Drive CurrentDrive
        {
            get
            {
                return treeView1.SelectedNode?.Tag as Drive;
            }
        }

        private void btnAddPrinterByIP_Click(object sender, EventArgs e)
        {
            var p = new IPPrinter();
            StartMonitoringPrinterMessages(p);
            p.Host = txtIP.Text;

            var node = new TreeNode(p.Host);
            node.Tag = p;
            treeView1.Nodes.Add(node);
            treeView1.SelectedNode = node;
            BeginRefreshPrinterNode(node);
        }

        Dictionary<Printer, NotifyCollectionChangedEventHandler> messageEventHandlers = new Dictionary<Printer, NotifyCollectionChangedEventHandler>();

        void StartMonitoringPrinterMessages(Printer p)
        {
            var handler = new NotifyCollectionChangedEventHandler((sender, e) =>
            {
                PrinterMessageCollectionChanged(p, e);
            });
            p.Messages.CollectionChanged += handler;
            messageEventHandlers[p] = handler;
        }

        void StopMonitoringPrinterMessages(Printer p)
        {
            if (messageEventHandlers.ContainsKey(p))
                p.Messages.CollectionChanged -= messageEventHandlers[p];
        }

        void PrinterMessageCollectionChanged(Printer p, NotifyCollectionChangedEventArgs e)
        {
            // Adding is the only one we expect.
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (!messageLogWithBinary.ContainsKey(p))
                    messageLogWithBinary[p] = new StringBuilder();
                if (!messageLogWithoutBinary.ContainsKey(p))
                    messageLogWithoutBinary[p] = new StringBuilder();

                foreach (PrinterMessage item in e.NewItems)
                {
                    var stringContents = item.StringContents;
                    if (item.IsBinaryString)
                    {
                        messageLogWithBinary[p].Append($"{item.TimeGenerated.ToString("yyyy-MM-dd HH:mm:ss")} {item.Direction} Binary Message ({item.ByteContents.Length} bytes)\r\n{stringContents}\r\n\r\n");
                        messageLogWithoutBinary[p].Append($"{item.TimeGenerated.ToString("yyyy-MM-dd HH:mm:ss")} {item.Direction} Binary Message ({item.ByteContents.Length} bytes)\r\n\r\n");

                    }
                    else
                    {
                        var log = $"{item.TimeGenerated.ToString("yyyy-MM-dd HH:mm:ss")} {item.Direction} String Message {item.StringContents.Length} chars\r\n{stringContents}\r\n\r\n";
                        messageLogWithBinary[p].Append(log);
                        messageLogWithoutBinary[p].Append(log);

                    }
                }

                RefreshMessageLog();
            }
        }

        void RefreshMessageLog()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(RefreshMessageLog));
                return;
            }


            var printer = CurrentPrinter;

            if (printer != null)
            {
                if (chkShowBinaryMessages.Checked && messageLogWithBinary.ContainsKey(printer))
                    txtHistory.Text = messageLogWithBinary[CurrentPrinter].ToString();
                else if (messageLogWithoutBinary.ContainsKey(printer))
                    txtHistory.Text = messageLogWithBinary[CurrentPrinter].ToString();
                else
                    txtHistory.Text = "{No History}";

                txtHistory.Select(txtHistory.TextLength, 0);
                txtHistory.ScrollToCaret();
            }
        }

        void BeginRefreshPrinterNode(TreeNode node, string drive = null)
        {
            ThreadPool.QueueUserWorkItem(x =>
            {

                var p = node.Tag as Printer;

                if (p.Connected || p.Connect())
                {
                    try
                    {
                        var fs = p.GetFileSystem(drive);
                        this.Invoke((Action<TreeNode, FileSystem>)EndRefreshPrinterNode, node, p.LastFileSystemResults);
                    }
                    catch (Exception ex)
                    {
                        Invoke(new Action<Exception>(z => MessageBox.Show(z.Message)), ex);
                    }
                }
            }, node);
        }

        void EndRefreshPrinterNode(TreeNode node, FileSystem fs)
        {
            Drive selectedDrive = null;
            if (treeView1.SelectedNode?.Parent != null && treeView1.SelectedNode.Parent == node && treeView1.SelectedNode.Tag is Drive)
            {
                selectedDrive = treeView1.SelectedNode.Tag as Drive;
            }
            node.Nodes.Clear();
            TreeNode toselect = null;
            foreach (var drive in fs.Drives)
            {
                var driveNode = new TreeNode($"{drive.Letter}: ({drive.Name}) - {BytesToString(drive.Free)} free of {BytesToString(drive.Size)}");
                driveNode.Tag = drive;
                if (selectedDrive?.Letter == drive.Letter)
                {
                    toselect = driveNode;
                }

                node.Nodes.Add(driveNode);
            }
            if (toselect != null)
                treeView1.SelectedNode = toselect;
        }
        /// <summary>
        /// https://stackoverflow.com/a/4975942
        /// </summary>
        /// <param name="byteCount"></param>
        /// <returns></returns>
        static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var treeNode = treeView1.SelectedNode;
            if (treeNode.Tag is Drive && treeNode.Parent.Tag is Printer)
            {
                var drive = (Drive)treeNode.Tag;
                var printer = (Printer)treeNode.Parent.Tag;
                var fs = printer.LastFileSystemResults;
                listView1.BeginUpdate();
                listView1.Items.Clear();
                foreach (var file in fs.FileSystemEntries.Where(x => x.Drive == drive.Letter))
                {
                    var lvItem = new ListViewItem(new[] { file.Filename, BytesToString(file.Size), Path.GetExtension(file.Filename) + " file", string.Join(", ", file.Attributes) });
                    lvItem.Tag = file;
                    listView1.Items.Add(lvItem);
                }
                listView1.EndUpdate();
            }

            RefreshMessageLog();
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listView1.Sort();
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var files = new List<File>();
            Printer p = treeView1.SelectedNode.Parent?.Tag as Printer;
            if (p == null)
                return;

            if (listView1.SelectedItems.Contains(e.Item as ListViewItem))
            {
                files.AddRange(listView1.SelectedItems.OfType<ListViewItem>().Where(x => x.Tag is File).Select(x => x.Tag as File));
            }
            else if ((e.Item as ListViewItem).Tag is File)
            {
                files.Add((e.Item as ListViewItem).Tag as File);
            }

            ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
            {
                var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(tempDir);
                var tempFiles = new List<string>();
                foreach (var file in files)
                {
                    var targetPath = Path.Combine(tempDir, Path.GetFileName(file.Filename));
                    var bits = p.GetFileContents(file.Path);
                    System.IO.File.WriteAllBytes(targetPath, bits);
                    tempFiles.Add(targetPath);
                }

                Invoke(new Action<List<string>>(y => DoDragDrop(new DataObject(DataFormats.FileDrop, y.ToArray()), DragDropEffects.Copy)), tempFiles);

            }));
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("FileDrop") && treeView1.SelectedNode.Tag is Drive && treeView1.SelectedNode.Parent.Tag is Printer)
            {
                var drive = treeView1.SelectedNode.Tag as Drive;
                var printer = treeView1.SelectedNode.Parent.Tag as Printer;
                // User dropped a file (or multiple)
                var filenames = e.Data.GetData("FileDrop") as string[];
                if (filenames != null)
                {
                    foreach (var file in filenames)
                    {
                        var targetName = $"{drive.Letter}:{Path.GetFileName(file)}";
                        printer.SetFileContents(targetName, System.IO.File.ReadAllBytes(file));

                    }
                    BeginRefreshPrinterNode(treeView1.SelectedNode.Parent, drive.Letter);
                }
            }
        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && treeView1.SelectedNode.Tag is Drive && treeView1.SelectedNode.Parent.Tag is Printer)
            {

                var printer = treeView1.SelectedNode.Parent.Tag as Printer;
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    var file = item.Tag as File;
                    if (file != null)
                    {
                        printer.DeleteFile(file.Path);
                        BeginRefreshPrinterNode(treeView1.SelectedNode.Parent, file.Drive);
                    }
                }
            }
            else if (e.KeyCode == Keys.F2 && listView1.SelectedItems.Count != 0 && listView1.SelectedItems[0].Tag is File)
            {
                listView1.SelectedItems[0].BeginEdit();
            }
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Drive && treeView1.SelectedNode.Parent.Tag is Printer)
            {
                var item = listView1.Items[e.Item];
                var printer = treeView1.SelectedNode.Parent.Tag as Printer;
                var file = item.Tag as File;
                if (file != null && !e.CancelEdit && !string.IsNullOrWhiteSpace(e.Label))
                {
                    var newName = file.Drive + ":" + e.Label;
                    printer.RenameFile(file.Path, newName);
                    file.Filename = e.Label;
                    BeginRefreshPrinterNode(treeView1.SelectedNode.Parent, file.Drive);
                }
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            MainContainer.Panel2Collapsed = !MainContainer.Panel2Collapsed;
        }

        private void btnAddBT_Click(object sender, EventArgs e)
        {
            var port = comboBox1.SelectedValue as PortDescription;
            if (port != null)
            {

                var p = new SerialPrinter();
                StartMonitoringPrinterMessages(p);
                p.ComPort = port.Port;
                var node = new TreeNode(port.Description);
                node.Tag = p;
                treeView1.Nodes.Add(node);
                treeView1.SelectedNode = node;
                BeginRefreshPrinterNode(node);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            if (treeView1.SelectedNode?.Tag is Printer)
            {
                BeginRefreshPrinterNode(treeView1.SelectedNode);
            }
            else if (treeView1.SelectedNode?.Parent?.Tag is Printer)
            {
                BeginRefreshPrinterNode(treeView1.SelectedNode.Parent);
            }
        }

        private void btnAddUSBPrinter_Click(object sender, EventArgs e)
        {
            var p = new USBPrinter();
            StartMonitoringPrinterMessages(p);
            p.PrinterName = cbUSB.SelectedItem as string;
            var node = new TreeNode(p.PrinterName);
            node.Tag = p;
            treeView1.Nodes.Add(node);
            treeView1.SelectedNode = node;
            BeginRefreshPrinterNode(node);


        }



        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Node.Tag is Printer)
            {
                ctxPrinter.Tag = e.Node.Tag;
                ctxPrinter.Show(treeView1, e.X, e.Y);
            }
        }

        private void factoryResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = treeView1.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Tag == ctxPrinter.Tag);
            if (node == null)
                return;

            var printer = ctxPrinter.Tag as Printer;
            if (printer != null && MessageBox.Show("Are you sure you want to factory reset this printer?", "Reset", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                printer.FactoryReset();
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var node = treeView1.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Tag == ctxPrinter.Tag);
            if (node == null)
                return;

            var printer = ctxPrinter.Tag as Printer;
            StopMonitoringPrinterMessages(printer);
            printer.Dispose();
            treeView1.Nodes.Remove(node);

        }

        private void btnRefreshUSB_Click(object sender, EventArgs e)
        {
            RefreshUSBPorts();
        }

        private void btnRefreshBT_Click(object sender, EventArgs e)
        {
            RefreshBTPorts();
        }

        private void btnCopyFilesTo_Click(object sender, EventArgs e)
        {
            var files = new List<File>();
            Printer p = treeView1.SelectedNode.Parent?.Tag as Printer;
            if (p == null)
                return;

            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }
            files.AddRange(listView1.SelectedItems.OfType<ListViewItem>().Where(x => x.Tag is File).Select(x => x.Tag as File));

            var fd = new FolderPicker();
            if (fd.ShowDialog(this.Handle) == true)
            {
                var folder = fd.ResultPath;

                ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
                {
                    foreach (var file in files)
                    {
                        var targetPath = Path.Combine(folder, file.Filename);
                        var bits = p.GetFileContents(file.Path);
                        System.IO.File.WriteAllBytes(targetPath, bits);
                    }
                }));
            }
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            Printer p;
            if (treeView1.SelectedNode.Tag is Printer)
                p = treeView1.SelectedNode.Tag as Printer;
            else if (treeView1.SelectedNode.Parent?.Tag is Printer)
                p = treeView1.SelectedNode.Parent.Tag as Printer;
            else
                return;

            var fd = new OpenFileDialog()
            {
            };
            if ((fd.ShowDialog() == DialogResult.OK) && System.IO.File.Exists(fd.FileName))
            {
                var contents = System.IO.File.ReadAllBytes(fd.FileName);
                p.RunCommand(contents, false);
                MessageBox.Show("File Sent");
            }
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = treeView1.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Tag == ctxPrinter.Tag);
            if (node == null)
                return;

            var printer = ctxPrinter.Tag as Printer;
            if (printer != null)
            {
                printer.RestartPrinter();
            }
        }

        private void txtSendCommand_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                var printer = CurrentPrinter;
                if (printer != null)
                {
                    printer.RunCommand(txtSendCommand.Text + "\r\n");
                }
            }
        }

        #region PrinterSettings


        Dictionary<Printer, SortableBindingList<Setting>> printerSettings = new Dictionary<Printer, SortableBindingList<Setting>>();
        Dictionary<Printer, List<Setting>> changedPrinterSettings = new Dictionary<Printer, List<Setting>>();
        Dictionary<Printer, ListChangedEventHandler> printerSettingsEventHandlers = new Dictionary<Printer, ListChangedEventHandler>();

        private void BeginReloadSettings()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
            {
                try
                {
                    var printer = CurrentPrinter;
                    if (!printer.Connect())
                    {
                        return;
                    }

                    // Reset cache for this printer
                    if (printerSettings.ContainsKey(printer) && printerSettingsEventHandlers.ContainsKey(printer))
                    {
                        printerSettings[printer].ListChanged -= printerSettingsEventHandlers[printer];
                    }
                    if (changedPrinterSettings.ContainsKey(printer))
                    {
                        changedPrinterSettings[printer].Clear();
                        changedPrinterSettings.Remove(printer);
                    }


                    // Create the event handler and store it for when we need to remove it later.
                    printerSettingsEventHandlers[printer] = (sender, e) => Settings_ListChanged(printer, e);

                    // Retrieve the settings and attach our event handler
                    printerSettings[printer] = new SortableBindingList<Setting>(printer.GetSettings());
                    printerSettings[printer].ListChanged += printerSettingsEventHandlers[printer];

                    // Initialize the change tracking list for this printer.
                    changedPrinterSettings[printer] = new List<Setting>();

                    // If the user hasn't selected a different printer by now, refresh the list.
                    if (printer == CurrentPrinter)
                        Invoke(new Action(() => { Filter(); btnSave.Text = $"Save ({changedPrinterSettings[CurrentPrinter].Count})"; }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading settings: {ex.Message}");
                }

            }));
        }

        private void Settings_ListChanged(Printer printer, ListChangedEventArgs e)
        {
            // Two qualifiers here:
            // 1. Ensure the event comes from the currently selected printer. Wo want to ignore it otherwise
            // 2. Ensure it's the Value property that's changed
            if (CurrentPrinter == printer && e.ListChangedType == ListChangedType.ItemChanged && e.PropertyDescriptor.Name == "Value")
            {

                var setting = printerSettings[printer][e.OldIndex];
                var changedSettings = changedPrinterSettings[printer];
                if (!changedSettings.Contains(setting))
                {
                    changedSettings.Add(setting);
                    Invoke(new Action(() => btnSave.Text = $"Save ({changedSettings.Count})"));
                }
            }
        }

        private void btnRefreshSettings_Click(object sender, EventArgs e)
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
                            if (!range.Contains(setting.Value))
                                range.Add(setting.Value);

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

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            var printer = CurrentPrinter;
            if (printer == null)
                return;
            ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
            {
                foreach (var setting in changedPrinterSettings[printer])
                {
                    printer.SetSettingSGD(setting.Name, setting.Value);
                }
                changedPrinterSettings[printer].Clear();
                Invoke(new Action(() => btnSave.Text = $"Save ({changedPrinterSettings[printer].Count})"));
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
            var printer = CurrentPrinter;
            if (printer == null)
                return;

            if (changedPrinterSettings[printer].Count != 0)
            {
                var sb = new StringBuilder("! U ");
                foreach (var item in changedPrinterSettings[printer])
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
            var printer = CurrentPrinter;

            if (printer == null)
                return;

            var nonDefaultSettings = changedPrinterSettings[printer].Where(x => x.Access == SettingAccess.RW && x.Value != x.Default).ToList();
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
            var printer = CurrentPrinter;

            if (printer == null)
                return;

            lock (filterLock)
            {
                var oldList = dataGridView1.DataSource as SortableBindingList<Setting>;
                List<Setting> filteredList;

                // Access Filter
                SettingAccess? accessFilter = null;
                if (chkFilterR.Checked)
                    accessFilter = SettingAccess.R;
                else if (chkFilterRW.Checked)
                    accessFilter = SettingAccess.RW;
                else if (chkFilterW.Checked)
                    accessFilter = SettingAccess.W;

                if (accessFilter != null)
                    filteredList = printerSettings[printer].Where(x => x.Access == accessFilter.Value).ToList();
                else
                    filteredList = new List<Setting>(printerSettings[printer]);

                // Name filter
                if (!string.IsNullOrWhiteSpace(txtFilter.Text))
                    filteredList = filteredList.Where(x => x.Name.ToLower().Contains(txtFilter.Text.ToLower())).ToList();


                // Assign the new list to the DataGridView
                var filtered = new SortableBindingList<Setting>(filteredList);
                if (oldList != null)
                    filtered.CopySortingFrom(oldList);

                Invoke(new Action<SortableBindingList<Setting>>(x => dataGridView1.DataSource = x), filtered);

            }
        }

        bool chkChanging;
        private void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkChanging && sender is CheckBox)
            {
                chkChanging = true;

                var checkbox = sender as CheckBox;
                if (checkbox.Checked) // This box was checked. Need to ensure the others are unchecked
                {
                    var list = new List<CheckBox> { chkFilterR, chkFilterRW, chkFilterW };
                    list.Remove(checkbox);
                    foreach (var item in list)
                        item.Checked = false;
                }
                // else {} No need to handle anything if it was unchecked

                if (filterTimer == null)
                    filterTimer = new System.Threading.Timer(x => Filter());
                filterTimer.Change(50, Timeout.Infinite);

                chkChanging = false;
            }
        }

        #endregion
    }
}
