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

namespace ZebraFileManager
{
    public partial class Form1 : Form
    {
        private ListViewColumnSorter lvwColumnSorter;
        public Form1()
        {
            InitializeComponent();

            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;

            var ports = RJCP.IO.Ports.SerialPortStream.GetPortDescriptions().Where(x => !x.Description.ToLower().StartsWith("standard serial over bluetooth link")).ToList();
            ports.AddRange(GetBTPorts());

            cbUSB.DataSource = USBPrinter2.UsbPrinterResolver.GetUSBPrinterPorts();

            comboBox1.DisplayMember = "Description";
            comboBox1.DataSource = ports;
        }

        List<PortDescription> GetBTPorts()
        {
            var ports = new List<PortDescription>();
            var regex = new Regex(@"^[a-fA-F0-9]&[a-fA-F0-9]{8}&[a-fA-F0-9]&(?<MAC>[a-fA-F0-9]{12})_[a-fA-F0-9]+$", RegexOptions.Compiled);
            using (var bthenum = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum\\BTHENUM"))
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

            return ports;
        }

        private void btnAddPrinterByIP_Click(object sender, EventArgs e)
        {
            var p = new IPPrinter();
            p.Host = txtIP.Text;
            var node = new TreeNode(p.Host);
            node.Tag = p;
            treeView1.Nodes.Add(node);
            BeginRefreshPrinterNode(node);
        }

        void BeginRefreshPrinterNode(TreeNode node, string drive = null)
        {
            ThreadPool.QueueUserWorkItem(x =>
            {

                var p = node.Tag as Printer;

                if (p.Connected || p.Connect())
                {
                    var fs = p.GetFileSystem(drive);
                    this.Invoke((Action<TreeNode, FileSystem>)EndRefreshPrinterNode, node, p.LastFileSystemResults);
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
                foreach (var file in fs.FileSystemEntries.Where(x => x.Path[0] == drive.Letter[0]))
                {
                    var lvItem = new ListViewItem(new[] { file.Path.Split(':')[1], BytesToString(file.Size), Path.GetExtension(file.Path) + " file", string.Join(", ", file.Attributes) });
                    lvItem.Tag = file;
                    listView1.Items.Add(lvItem);
                }
                listView1.EndUpdate();
            }
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
                    var targetPath = Path.Combine(tempDir, Path.GetFileName(file.Path));
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
                foreach (var file in filenames)
                {
                    var targetName = $"{drive.Letter}:{Path.GetFileName(file)}";
                    printer.SetFileContents(targetName, System.IO.File.ReadAllBytes(file));

                }
                BeginRefreshPrinterNode(treeView1.SelectedNode.Parent, drive.Letter);
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
                        BeginRefreshPrinterNode(treeView1.SelectedNode.Parent, file.Path[0].ToString());
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
                if (file != null)
                {
                    var newName = file.Path[0] + ":" + e.Label;
                    printer.RenameFile(file.Path, newName);
                    file.Path = newName;
                    BeginRefreshPrinterNode(treeView1.SelectedNode.Parent, file.Path[0].ToString());
                }
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            var printer = treeView1.SelectedNode?.Tag as Printer ?? treeView1.SelectedNode?.Parent?.Tag as Printer;

            if (printer != null)
            {
                using (var frm = new frmSettings(printer))
                    frm.ShowDialog();
            }
        }

        private void btnAddBT_Click(object sender, EventArgs e)
        {
            var port = comboBox1.SelectedValue as PortDescription;
            if (port != null)
            {

                var p = new SerialPrinter();
                p.ComPort = port.Port;
                var node = new TreeNode(port.Description);
                node.Tag = p;
                treeView1.Nodes.Add(node);
                BeginRefreshPrinterNode(node);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var p = new USBPrinter2();
            p.PrinterName = "ZDesigner QLn320 (ZPL)";
            var node = new TreeNode(p.PrinterName);
            node.Tag = p;
            treeView1.Nodes.Add(node);
            BeginRefreshPrinterNode(node);

        }

        private void btnAddUSBPrinter_Click(object sender, EventArgs e)
        {
            var p = new USBPrinter2();
            p.PrinterName = cbUSB.SelectedItem as string;
            var node = new TreeNode(p.PrinterName);
            node.Tag = p;
            treeView1.Nodes.Add(node);
            BeginRefreshPrinterNode(node);


        }
    }
}
