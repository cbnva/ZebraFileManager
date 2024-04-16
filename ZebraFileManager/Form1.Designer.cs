namespace ZebraFileManager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listView1 = new System.Windows.Forms.ListView();
            this.hdrName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrAttributes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddPrinterByIP = new System.Windows.Forms.Button();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.FileContainer = new System.Windows.Forms.SplitContainer();
            this.btnRefreshFiles = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnAddBT = new System.Windows.Forms.Button();
            this.btnAddUSBPrinter = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbUSB = new System.Windows.Forms.ComboBox();
            this.ctxPrinter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.factoryResetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRefreshBT = new System.Windows.Forms.Button();
            this.btnRefreshUSB = new System.Windows.Forms.Button();
            this.btnCopyFilesTo = new System.Windows.Forms.Button();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.CommandFileContainer = new System.Windows.Forms.SplitContainer();
            this.chkShowBinaryMessages = new System.Windows.Forms.CheckBox();
            this.txtSendCommand = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtHistory = new System.Windows.Forms.TextBox();
            this.MainContainer = new System.Windows.Forms.SplitContainer();
            this.chkFilterW = new System.Windows.Forms.CheckBox();
            this.chkFilterRW = new System.Windows.Forms.CheckBox();
            this.chkFilterR = new System.Windows.Forms.CheckBox();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnExportNonDefaults = new System.Windows.Forms.Button();
            this.btnExportChangedZPL = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRefreshSettings = new System.Windows.Forms.Button();
            this.settingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.accessDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.archiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cloneDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.rangeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.defaultDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.FileContainer)).BeginInit();
            this.FileContainer.Panel1.SuspendLayout();
            this.FileContainer.Panel2.SuspendLayout();
            this.FileContainer.SuspendLayout();
            this.ctxPrinter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CommandFileContainer)).BeginInit();
            this.CommandFileContainer.Panel1.SuspendLayout();
            this.CommandFileContainer.Panel2.SuspendLayout();
            this.CommandFileContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).BeginInit();
            this.MainContainer.Panel1.SuspendLayout();
            this.MainContainer.Panel2.SuspendLayout();
            this.MainContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settingBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.AllowDrop = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrName,
            this.hdrSize,
            this.hdrType,
            this.hdrAttributes});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.HideSelection = false;
            this.listView1.LabelEdit = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(765, 308);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listView1_AfterLabelEdit);
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView1_ItemDrag);
            this.listView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
            this.listView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView1_DragEnter);
            this.listView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyUp);
            // 
            // hdrName
            // 
            this.hdrName.Text = "Name";
            this.hdrName.Width = 139;
            // 
            // hdrSize
            // 
            this.hdrSize.Text = "Size";
            this.hdrSize.Width = 93;
            // 
            // hdrType
            // 
            this.hdrType.Text = "Type";
            this.hdrType.Width = 110;
            // 
            // hdrAttributes
            // 
            this.hdrAttributes.Text = "Attributes";
            this.hdrAttributes.Width = 77;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(381, 308);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "IP/Host:";
            // 
            // btnAddPrinterByIP
            // 
            this.btnAddPrinterByIP.Location = new System.Drawing.Point(466, 8);
            this.btnAddPrinterByIP.Name = "btnAddPrinterByIP";
            this.btnAddPrinterByIP.Size = new System.Drawing.Size(121, 23);
            this.btnAddPrinterByIP.TabIndex = 3;
            this.btnAddPrinterByIP.Text = "Add Net Printer";
            this.btnAddPrinterByIP.UseVisualStyleBackColor = true;
            this.btnAddPrinterByIP.Click += new System.EventHandler(this.btnAddPrinterByIP_Click);
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(64, 10);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(396, 20);
            this.txtIP.TabIndex = 4;
            // 
            // FileContainer
            // 
            this.FileContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FileContainer.Location = new System.Drawing.Point(0, 0);
            this.FileContainer.Name = "FileContainer";
            // 
            // FileContainer.Panel1
            // 
            this.FileContainer.Panel1.Controls.Add(this.treeView1);
            // 
            // FileContainer.Panel2
            // 
            this.FileContainer.Panel2.Controls.Add(this.listView1);
            this.FileContainer.Size = new System.Drawing.Size(1150, 308);
            this.FileContainer.SplitterDistance = 381;
            this.FileContainer.TabIndex = 5;
            // 
            // btnRefreshFiles
            // 
            this.btnRefreshFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshFiles.Location = new System.Drawing.Point(961, 12);
            this.btnRefreshFiles.Name = "btnRefreshFiles";
            this.btnRefreshFiles.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshFiles.TabIndex = 3;
            this.btnRefreshFiles.Text = "Refresh";
            this.btnRefreshFiles.UseVisualStyleBackColor = true;
            this.btnRefreshFiles.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.Location = new System.Drawing.Point(1042, 12);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(111, 23);
            this.btnSettings.TabIndex = 3;
            this.btnSettings.Text = "Printer Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(64, 39);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(367, 21);
            this.comboBox1.TabIndex = 6;
            // 
            // btnAddBT
            // 
            this.btnAddBT.Location = new System.Drawing.Point(466, 37);
            this.btnAddBT.Name = "btnAddBT";
            this.btnAddBT.Size = new System.Drawing.Size(121, 23);
            this.btnAddBT.TabIndex = 3;
            this.btnAddBT.Text = "Add BT/Serial Printer";
            this.btnAddBT.UseVisualStyleBackColor = true;
            this.btnAddBT.Click += new System.EventHandler(this.btnAddBT_Click);
            // 
            // btnAddUSBPrinter
            // 
            this.btnAddUSBPrinter.Location = new System.Drawing.Point(466, 66);
            this.btnAddUSBPrinter.Name = "btnAddUSBPrinter";
            this.btnAddUSBPrinter.Size = new System.Drawing.Size(121, 23);
            this.btnAddUSBPrinter.TabIndex = 3;
            this.btnAddUSBPrinter.Text = "Add USB Printer";
            this.btnAddUSBPrinter.UseVisualStyleBackColor = true;
            this.btnAddUSBPrinter.Click += new System.EventHandler(this.btnAddUSBPrinter_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "BT/Serial:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "USB:";
            // 
            // cbUSB
            // 
            this.cbUSB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUSB.FormattingEnabled = true;
            this.cbUSB.Location = new System.Drawing.Point(64, 68);
            this.cbUSB.Name = "cbUSB";
            this.cbUSB.Size = new System.Drawing.Size(367, 21);
            this.cbUSB.TabIndex = 6;
            // 
            // ctxPrinter
            // 
            this.ctxPrinter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem,
            this.toolStripSeparator1,
            this.factoryResetToolStripMenuItem,
            this.restartToolStripMenuItem});
            this.ctxPrinter.Name = "ctxPrinter";
            this.ctxPrinter.Size = new System.Drawing.Size(154, 76);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(150, 6);
            // 
            // factoryResetToolStripMenuItem
            // 
            this.factoryResetToolStripMenuItem.Name = "factoryResetToolStripMenuItem";
            this.factoryResetToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.factoryResetToolStripMenuItem.Text = "Factory Reset...";
            this.factoryResetToolStripMenuItem.Click += new System.EventHandler(this.factoryResetToolStripMenuItem_Click);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.restartToolStripMenuItem.Text = "Restart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.restartToolStripMenuItem_Click);
            // 
            // btnRefreshBT
            // 
            this.btnRefreshBT.Image = global::ZebraFileManager.Properties.Resources.Refresh;
            this.btnRefreshBT.Location = new System.Drawing.Point(437, 37);
            this.btnRefreshBT.Name = "btnRefreshBT";
            this.btnRefreshBT.Size = new System.Drawing.Size(23, 23);
            this.btnRefreshBT.TabIndex = 3;
            this.btnRefreshBT.UseVisualStyleBackColor = true;
            this.btnRefreshBT.Click += new System.EventHandler(this.btnRefreshBT_Click);
            // 
            // btnRefreshUSB
            // 
            this.btnRefreshUSB.Image = global::ZebraFileManager.Properties.Resources.Refresh;
            this.btnRefreshUSB.Location = new System.Drawing.Point(437, 66);
            this.btnRefreshUSB.Name = "btnRefreshUSB";
            this.btnRefreshUSB.Size = new System.Drawing.Size(23, 23);
            this.btnRefreshUSB.TabIndex = 3;
            this.btnRefreshUSB.UseVisualStyleBackColor = true;
            this.btnRefreshUSB.Click += new System.EventHandler(this.btnRefreshUSB_Click);
            // 
            // btnCopyFilesTo
            // 
            this.btnCopyFilesTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyFilesTo.Location = new System.Drawing.Point(1078, 66);
            this.btnCopyFilesTo.Name = "btnCopyFilesTo";
            this.btnCopyFilesTo.Size = new System.Drawing.Size(75, 23);
            this.btnCopyFilesTo.TabIndex = 7;
            this.btnCopyFilesTo.Text = "Copy To...";
            this.btnCopyFilesTo.UseVisualStyleBackColor = true;
            this.btnCopyFilesTo.Click += new System.EventHandler(this.btnCopyFilesTo_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendFile.Location = new System.Drawing.Point(997, 66);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(75, 23);
            this.btnSendFile.TabIndex = 8;
            this.btnSendFile.Text = "Send File";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // CommandFileContainer
            // 
            this.CommandFileContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CommandFileContainer.Location = new System.Drawing.Point(3, 95);
            this.CommandFileContainer.Name = "CommandFileContainer";
            this.CommandFileContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // CommandFileContainer.Panel1
            // 
            this.CommandFileContainer.Panel1.Controls.Add(this.FileContainer);
            // 
            // CommandFileContainer.Panel2
            // 
            this.CommandFileContainer.Panel2.Controls.Add(this.chkShowBinaryMessages);
            this.CommandFileContainer.Panel2.Controls.Add(this.txtSendCommand);
            this.CommandFileContainer.Panel2.Controls.Add(this.label4);
            this.CommandFileContainer.Panel2.Controls.Add(this.txtHistory);
            this.CommandFileContainer.Size = new System.Drawing.Size(1150, 521);
            this.CommandFileContainer.SplitterDistance = 308;
            this.CommandFileContainer.TabIndex = 9;
            // 
            // chkShowBinaryMessages
            // 
            this.chkShowBinaryMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowBinaryMessages.AutoSize = true;
            this.chkShowBinaryMessages.Location = new System.Drawing.Point(3, 188);
            this.chkShowBinaryMessages.Name = "chkShowBinaryMessages";
            this.chkShowBinaryMessages.Size = new System.Drawing.Size(85, 17);
            this.chkShowBinaryMessages.TabIndex = 3;
            this.chkShowBinaryMessages.Text = "Show Binary";
            this.chkShowBinaryMessages.UseVisualStyleBackColor = true;
            // 
            // txtSendCommand
            // 
            this.txtSendCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSendCommand.Location = new System.Drawing.Point(197, 186);
            this.txtSendCommand.Name = "txtSendCommand";
            this.txtSendCommand.Size = new System.Drawing.Size(1207, 20);
            this.txtSendCommand.TabIndex = 2;
            this.txtSendCommand.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSendCommand_KeyUp);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(109, 189);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Send Command";
            // 
            // txtHistory
            // 
            this.txtHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHistory.Location = new System.Drawing.Point(3, 3);
            this.txtHistory.Multiline = true;
            this.txtHistory.Name = "txtHistory";
            this.txtHistory.ReadOnly = true;
            this.txtHistory.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtHistory.Size = new System.Drawing.Size(1144, 177);
            this.txtHistory.TabIndex = 0;
            // 
            // MainContainer
            // 
            this.MainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainContainer.Location = new System.Drawing.Point(0, 0);
            this.MainContainer.Name = "MainContainer";
            // 
            // MainContainer.Panel1
            // 
            this.MainContainer.Panel1.Controls.Add(this.label1);
            this.MainContainer.Panel1.Controls.Add(this.label2);
            this.MainContainer.Panel1.Controls.Add(this.CommandFileContainer);
            this.MainContainer.Panel1.Controls.Add(this.label3);
            this.MainContainer.Panel1.Controls.Add(this.btnSendFile);
            this.MainContainer.Panel1.Controls.Add(this.btnAddPrinterByIP);
            this.MainContainer.Panel1.Controls.Add(this.btnCopyFilesTo);
            this.MainContainer.Panel1.Controls.Add(this.btnAddBT);
            this.MainContainer.Panel1.Controls.Add(this.cbUSB);
            this.MainContainer.Panel1.Controls.Add(this.btnAddUSBPrinter);
            this.MainContainer.Panel1.Controls.Add(this.comboBox1);
            this.MainContainer.Panel1.Controls.Add(this.btnRefreshUSB);
            this.MainContainer.Panel1.Controls.Add(this.txtIP);
            this.MainContainer.Panel1.Controls.Add(this.btnRefreshBT);
            this.MainContainer.Panel1.Controls.Add(this.btnSettings);
            this.MainContainer.Panel1.Controls.Add(this.btnRefreshFiles);
            // 
            // MainContainer.Panel2
            // 
            this.MainContainer.Panel2.Controls.Add(this.chkFilterW);
            this.MainContainer.Panel2.Controls.Add(this.chkFilterRW);
            this.MainContainer.Panel2.Controls.Add(this.chkFilterR);
            this.MainContainer.Panel2.Controls.Add(this.txtFilter);
            this.MainContainer.Panel2.Controls.Add(this.label5);
            this.MainContainer.Panel2.Controls.Add(this.btnExportNonDefaults);
            this.MainContainer.Panel2.Controls.Add(this.btnExportChangedZPL);
            this.MainContainer.Panel2.Controls.Add(this.btnSave);
            this.MainContainer.Panel2.Controls.Add(this.btnRefreshSettings);
            this.MainContainer.Panel2.Controls.Add(this.dataGridView1);
            this.MainContainer.Size = new System.Drawing.Size(1751, 619);
            this.MainContainer.SplitterDistance = 1156;
            this.MainContainer.TabIndex = 10;
            // 
            // chkFilterW
            // 
            this.chkFilterW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFilterW.AutoSize = true;
            this.chkFilterW.Location = new System.Drawing.Point(542, 16);
            this.chkFilterW.Name = "chkFilterW";
            this.chkFilterW.Size = new System.Drawing.Size(37, 17);
            this.chkFilterW.TabIndex = 13;
            this.chkFilterW.Text = "W";
            this.chkFilterW.UseVisualStyleBackColor = true;
            this.chkFilterW.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // chkFilterRW
            // 
            this.chkFilterRW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFilterRW.AutoSize = true;
            this.chkFilterRW.Location = new System.Drawing.Point(491, 16);
            this.chkFilterRW.Name = "chkFilterRW";
            this.chkFilterRW.Size = new System.Drawing.Size(45, 17);
            this.chkFilterRW.TabIndex = 14;
            this.chkFilterRW.Text = "RW";
            this.chkFilterRW.UseVisualStyleBackColor = true;
            this.chkFilterRW.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // chkFilterR
            // 
            this.chkFilterR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFilterR.AutoSize = true;
            this.chkFilterR.Location = new System.Drawing.Point(451, 16);
            this.chkFilterR.Name = "chkFilterR";
            this.chkFilterR.Size = new System.Drawing.Size(34, 17);
            this.chkFilterR.TabIndex = 15;
            this.chkFilterR.Text = "R";
            this.chkFilterR.UseVisualStyleBackColor = true;
            this.chkFilterR.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.Location = new System.Drawing.Point(122, 14);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(323, 20);
            this.txtFilter.TabIndex = 12;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(84, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Filter:";
            // 
            // btnExportNonDefaults
            // 
            this.btnExportNonDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportNonDefaults.Location = new System.Drawing.Point(124, 61);
            this.btnExportNonDefaults.Name = "btnExportNonDefaults";
            this.btnExportNonDefaults.Size = new System.Drawing.Size(184, 23);
            this.btnExportNonDefaults.TabIndex = 8;
            this.btnExportNonDefaults.Text = "Export non-default settings...";
            this.btnExportNonDefaults.UseVisualStyleBackColor = true;
            this.btnExportNonDefaults.Click += new System.EventHandler(this.btnExportNonDefaults_Click);
            // 
            // btnExportChangedZPL
            // 
            this.btnExportChangedZPL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportChangedZPL.Location = new System.Drawing.Point(314, 61);
            this.btnExportChangedZPL.Name = "btnExportChangedZPL";
            this.btnExportChangedZPL.Size = new System.Drawing.Size(184, 23);
            this.btnExportChangedZPL.TabIndex = 9;
            this.btnExportChangedZPL.Text = "Export unsaved changes as .zpl...";
            this.btnExportChangedZPL.UseVisualStyleBackColor = true;
            this.btnExportChangedZPL.Click += new System.EventHandler(this.btnExportChangedAsZPL_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(504, 61);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save (0)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnRefreshSettings
            // 
            this.btnRefreshSettings.Location = new System.Drawing.Point(3, 12);
            this.btnRefreshSettings.Name = "btnRefreshSettings";
            this.btnRefreshSettings.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshSettings.TabIndex = 7;
            this.btnRefreshSettings.Text = "Refresh";
            this.btnRefreshSettings.UseVisualStyleBackColor = true;
            this.btnRefreshSettings.Click += new System.EventHandler(this.btnRefreshSettings_Click);
            // 
            // settingBindingSource
            // 
            this.settingBindingSource.DataSource = typeof(ZebraFileManager.Zebra.Setting);
            // 
            // accessDataGridViewTextBoxColumn
            // 
            this.accessDataGridViewTextBoxColumn.DataPropertyName = "Access";
            this.accessDataGridViewTextBoxColumn.HeaderText = "Access";
            this.accessDataGridViewTextBoxColumn.Name = "accessDataGridViewTextBoxColumn";
            this.accessDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // archiveDataGridViewCheckBoxColumn
            // 
            this.archiveDataGridViewCheckBoxColumn.DataPropertyName = "Archive";
            this.archiveDataGridViewCheckBoxColumn.HeaderText = "Archive";
            this.archiveDataGridViewCheckBoxColumn.Name = "archiveDataGridViewCheckBoxColumn";
            this.archiveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.archiveDataGridViewCheckBoxColumn.Width = 5;
            // 
            // cloneDataGridViewCheckBoxColumn
            // 
            this.cloneDataGridViewCheckBoxColumn.DataPropertyName = "Clone";
            this.cloneDataGridViewCheckBoxColumn.HeaderText = "Clone";
            this.cloneDataGridViewCheckBoxColumn.Name = "cloneDataGridViewCheckBoxColumn";
            this.cloneDataGridViewCheckBoxColumn.ReadOnly = true;
            this.cloneDataGridViewCheckBoxColumn.Width = 5;
            // 
            // rangeDataGridViewTextBoxColumn
            // 
            this.rangeDataGridViewTextBoxColumn.DataPropertyName = "Range";
            this.rangeDataGridViewTextBoxColumn.HeaderText = "Range";
            this.rangeDataGridViewTextBoxColumn.Name = "rangeDataGridViewTextBoxColumn";
            this.rangeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // typeDataGridViewTextBoxColumn
            // 
            this.typeDataGridViewTextBoxColumn.DataPropertyName = "Type";
            this.typeDataGridViewTextBoxColumn.HeaderText = "Type";
            this.typeDataGridViewTextBoxColumn.Name = "typeDataGridViewTextBoxColumn";
            this.typeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // valueDataGridViewTextBoxColumn
            // 
            this.valueDataGridViewTextBoxColumn.DataPropertyName = "DisplayValue";
            this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
            this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // defaultDataGridViewTextBoxColumn
            // 
            this.defaultDataGridViewTextBoxColumn.DataPropertyName = "Default";
            this.defaultDataGridViewTextBoxColumn.HeaderText = "Default";
            this.defaultDataGridViewTextBoxColumn.Name = "defaultDataGridViewTextBoxColumn";
            this.defaultDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.valueDataGridViewTextBoxColumn,
            this.typeDataGridViewTextBoxColumn,
            this.rangeDataGridViewTextBoxColumn,
            this.cloneDataGridViewCheckBoxColumn,
            this.archiveDataGridViewCheckBoxColumn,
            this.accessDataGridViewTextBoxColumn,
            this.defaultDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.settingBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(3, 95);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(576, 512);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            this.dataGridView1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView1_RowsAdded);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1751, 619);
            this.Controls.Add(this.MainContainer);
            this.Name = "Form1";
            this.Text = "Zebra File Manager";
            this.FileContainer.Panel1.ResumeLayout(false);
            this.FileContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FileContainer)).EndInit();
            this.FileContainer.ResumeLayout(false);
            this.ctxPrinter.ResumeLayout(false);
            this.CommandFileContainer.Panel1.ResumeLayout(false);
            this.CommandFileContainer.Panel2.ResumeLayout(false);
            this.CommandFileContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CommandFileContainer)).EndInit();
            this.CommandFileContainer.ResumeLayout(false);
            this.MainContainer.Panel1.ResumeLayout(false);
            this.MainContainer.Panel1.PerformLayout();
            this.MainContainer.Panel2.ResumeLayout(false);
            this.MainContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).EndInit();
            this.MainContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.settingBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddPrinterByIP;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.SplitContainer FileContainer;
        private System.Windows.Forms.ColumnHeader hdrName;
        private System.Windows.Forms.ColumnHeader hdrSize;
        private System.Windows.Forms.ColumnHeader hdrAttributes;
        private System.Windows.Forms.ColumnHeader hdrType;
        private System.Windows.Forms.Button btnRefreshFiles;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnAddBT;
        private System.Windows.Forms.Button btnAddUSBPrinter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbUSB;
        private System.Windows.Forms.ContextMenuStrip ctxPrinter;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem factoryResetToolStripMenuItem;
        private System.Windows.Forms.Button btnRefreshUSB;
        private System.Windows.Forms.Button btnRefreshBT;
        private System.Windows.Forms.Button btnCopyFilesTo;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
        private System.Windows.Forms.SplitContainer CommandFileContainer;
        private System.Windows.Forms.TextBox txtSendCommand;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtHistory;
        private System.Windows.Forms.CheckBox chkShowBinaryMessages;
        private System.Windows.Forms.SplitContainer MainContainer;
        private System.Windows.Forms.CheckBox chkFilterW;
        private System.Windows.Forms.CheckBox chkFilterRW;
        private System.Windows.Forms.CheckBox chkFilterR;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnExportNonDefaults;
        private System.Windows.Forms.Button btnExportChangedZPL;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefreshSettings;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rangeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cloneDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn archiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accessDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn defaultDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource settingBindingSource;
    }
}

