namespace ZebraFileManager
{
    partial class frmSettings
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rangeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cloneDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.archiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.accessDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.defaultDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.settingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExportChangedZPL = new System.Windows.Forms.Button();
            this.btnExportNonDefaults = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.chkFilterR = new System.Windows.Forms.CheckBox();
            this.chkFilterRW = new System.Windows.Forms.CheckBox();
            this.chkFilterW = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingBindingSource)).BeginInit();
            this.SuspendLayout();
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
            this.dataGridView1.Location = new System.Drawing.Point(12, 41);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(975, 397);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            this.dataGridView1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView1_RowsAdded);
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // valueDataGridViewTextBoxColumn
            // 
            this.valueDataGridViewTextBoxColumn.DataPropertyName = "DisplayValue";
            this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
            this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
            // 
            // typeDataGridViewTextBoxColumn
            // 
            this.typeDataGridViewTextBoxColumn.DataPropertyName = "Type";
            this.typeDataGridViewTextBoxColumn.HeaderText = "Type";
            this.typeDataGridViewTextBoxColumn.Name = "typeDataGridViewTextBoxColumn";
            this.typeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // rangeDataGridViewTextBoxColumn
            // 
            this.rangeDataGridViewTextBoxColumn.DataPropertyName = "Range";
            this.rangeDataGridViewTextBoxColumn.HeaderText = "Range";
            this.rangeDataGridViewTextBoxColumn.Name = "rangeDataGridViewTextBoxColumn";
            this.rangeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cloneDataGridViewCheckBoxColumn
            // 
            this.cloneDataGridViewCheckBoxColumn.DataPropertyName = "Clone";
            this.cloneDataGridViewCheckBoxColumn.HeaderText = "Clone";
            this.cloneDataGridViewCheckBoxColumn.Name = "cloneDataGridViewCheckBoxColumn";
            this.cloneDataGridViewCheckBoxColumn.ReadOnly = true;
            this.cloneDataGridViewCheckBoxColumn.Width = 5;
            // 
            // archiveDataGridViewCheckBoxColumn
            // 
            this.archiveDataGridViewCheckBoxColumn.DataPropertyName = "Archive";
            this.archiveDataGridViewCheckBoxColumn.HeaderText = "Archive";
            this.archiveDataGridViewCheckBoxColumn.Name = "archiveDataGridViewCheckBoxColumn";
            this.archiveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.archiveDataGridViewCheckBoxColumn.Width = 5;
            // 
            // accessDataGridViewTextBoxColumn
            // 
            this.accessDataGridViewTextBoxColumn.DataPropertyName = "Access";
            this.accessDataGridViewTextBoxColumn.HeaderText = "Access";
            this.accessDataGridViewTextBoxColumn.Name = "accessDataGridViewTextBoxColumn";
            this.accessDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // defaultDataGridViewTextBoxColumn
            // 
            this.defaultDataGridViewTextBoxColumn.DataPropertyName = "Default";
            this.defaultDataGridViewTextBoxColumn.HeaderText = "Default";
            this.defaultDataGridViewTextBoxColumn.Name = "defaultDataGridViewTextBoxColumn";
            this.defaultDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // settingBindingSource
            // 
            this.settingBindingSource.DataSource = typeof(ZebraFileManager.Zebra.Setting);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(12, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(912, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save (0)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExportChangedZPL
            // 
            this.btnExportChangedZPL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportChangedZPL.Location = new System.Drawing.Point(722, 12);
            this.btnExportChangedZPL.Name = "btnExportChangedZPL";
            this.btnExportChangedZPL.Size = new System.Drawing.Size(184, 23);
            this.btnExportChangedZPL.TabIndex = 2;
            this.btnExportChangedZPL.Text = "Export unsaved changes as .zpl...";
            this.btnExportChangedZPL.UseVisualStyleBackColor = true;
            this.btnExportChangedZPL.Click += new System.EventHandler(this.btnExportChangedAsZPL_Click);
            // 
            // btnExportNonDefaults
            // 
            this.btnExportNonDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportNonDefaults.Location = new System.Drawing.Point(532, 12);
            this.btnExportNonDefaults.Name = "btnExportNonDefaults";
            this.btnExportNonDefaults.Size = new System.Drawing.Size(184, 23);
            this.btnExportNonDefaults.TabIndex = 2;
            this.btnExportNonDefaults.Text = "Export non-default settings...";
            this.btnExportNonDefaults.UseVisualStyleBackColor = true;
            this.btnExportNonDefaults.Click += new System.EventHandler(this.btnExportNonDefaults_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Filter:";
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.Location = new System.Drawing.Point(131, 14);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(240, 20);
            this.txtFilter.TabIndex = 4;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // chkFilterR
            // 
            this.chkFilterR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFilterR.AutoSize = true;
            this.chkFilterR.Location = new System.Drawing.Point(377, 16);
            this.chkFilterR.Name = "chkFilterR";
            this.chkFilterR.Size = new System.Drawing.Size(34, 17);
            this.chkFilterR.TabIndex = 5;
            this.chkFilterR.Text = "R";
            this.chkFilterR.UseVisualStyleBackColor = true;
            this.chkFilterR.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // chkFilterRW
            // 
            this.chkFilterRW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFilterRW.AutoSize = true;
            this.chkFilterRW.Location = new System.Drawing.Point(417, 16);
            this.chkFilterRW.Name = "chkFilterRW";
            this.chkFilterRW.Size = new System.Drawing.Size(45, 17);
            this.chkFilterRW.TabIndex = 5;
            this.chkFilterRW.Text = "RW";
            this.chkFilterRW.UseVisualStyleBackColor = true;
            this.chkFilterRW.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // chkFilterW
            // 
            this.chkFilterW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFilterW.AutoSize = true;
            this.chkFilterW.Location = new System.Drawing.Point(468, 16);
            this.chkFilterW.Name = "chkFilterW";
            this.chkFilterW.Size = new System.Drawing.Size(37, 17);
            this.chkFilterW.TabIndex = 5;
            this.chkFilterW.Text = "W";
            this.chkFilterW.UseVisualStyleBackColor = true;
            this.chkFilterW.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(999, 450);
            this.Controls.Add(this.chkFilterW);
            this.Controls.Add(this.chkFilterRW);
            this.Controls.Add(this.chkFilterR);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExportNonDefaults);
            this.Controls.Add(this.btnExportChangedZPL);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dataGridView1);
            this.Name = "frmSettings";
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.BindingSource settingBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rangeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cloneDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn archiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accessDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn defaultDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExportChangedZPL;
        private System.Windows.Forms.Button btnExportNonDefaults;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.CheckBox chkFilterR;
        private System.Windows.Forms.CheckBox chkFilterRW;
        private System.Windows.Forms.CheckBox chkFilterW;
    }
}