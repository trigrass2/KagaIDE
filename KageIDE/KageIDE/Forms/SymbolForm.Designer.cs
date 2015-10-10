namespace KagaIDE.Forms
{
    partial class SymbolForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SymbolForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.mainNotEditableLabel = new System.Windows.Forms.Label();
            this.funListBox = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.globalvarDataGridView = new System.Windows.Forms.DataGridView();
            this.变量名 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.类型 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.switchDataGridView = new System.Windows.Forms.DataGridView();
            this.开关序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.描述 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.globalvarDataGridView)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.switchDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(579, 367);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.button17);
            this.tabPage1.Controls.Add(this.button18);
            this.tabPage1.Controls.Add(this.mainNotEditableLabel);
            this.tabPage1.Controls.Add(this.funListBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(571, 341);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "函数管理";
            // 
            // button17
            // 
            this.button17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button17.Location = new System.Drawing.Point(83, 315);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(60, 23);
            this.button17.TabIndex = 4;
            this.button17.Text = "删除";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button18
            // 
            this.button18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button18.Location = new System.Drawing.Point(13, 315);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(60, 23);
            this.button18.TabIndex = 3;
            this.button18.Text = "添加";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // mainNotEditableLabel
            // 
            this.mainNotEditableLabel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mainNotEditableLabel.Location = new System.Drawing.Point(161, 7);
            this.mainNotEditableLabel.Name = "mainNotEditableLabel";
            this.mainNotEditableLabel.Size = new System.Drawing.Size(404, 327);
            this.mainNotEditableLabel.TabIndex = 1;
            this.mainNotEditableLabel.Text = "主函数main的函数签名不可编辑";
            this.mainNotEditableLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // funListBox
            // 
            this.funListBox.FormattingEnabled = true;
            this.funListBox.ItemHeight = 12;
            this.funListBox.Location = new System.Drawing.Point(5, 7);
            this.funListBox.Name = "funListBox";
            this.funListBox.Size = new System.Drawing.Size(150, 304);
            this.funListBox.TabIndex = 0;
            this.funListBox.SelectedIndexChanged += new System.EventHandler(this.funListBox_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(571, 341);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "变量管理";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.globalvarDataGridView);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(253, 329);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "全局变量";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(172, 301);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "保存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label3.Location = new System.Drawing.Point(57, 309);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "右键选中行删除变量";
            // 
            // globalvarDataGridView
            // 
            this.globalvarDataGridView.AllowUserToResizeColumns = false;
            this.globalvarDataGridView.AllowUserToResizeRows = false;
            this.globalvarDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.globalvarDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.变量名,
            this.类型});
            this.globalvarDataGridView.Location = new System.Drawing.Point(7, 21);
            this.globalvarDataGridView.Name = "globalvarDataGridView";
            this.globalvarDataGridView.RowHeadersVisible = false;
            this.globalvarDataGridView.RowTemplate.Height = 23;
            this.globalvarDataGridView.Size = new System.Drawing.Size(240, 276);
            this.globalvarDataGridView.TabIndex = 0;
            this.globalvarDataGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.globalvarDataGridView_CellMouseClick);
            this.globalvarDataGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.globalvarDataGridView_EditingControlShowing);
            // 
            // 变量名
            // 
            this.变量名.HeaderText = "变量名";
            this.变量名.Name = "变量名";
            this.变量名.Width = 110;
            // 
            // 类型
            // 
            this.类型.HeaderText = "类型";
            this.类型.Items.AddRange(new object[] {
            "int",
            "char",
            "long",
            "float",
            "double",
            "unsigned int",
            "unsigned char"});
            this.类型.Name = "类型";
            this.类型.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.类型.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.类型.Width = 110;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.button2);
            this.tabPage3.Controls.Add(this.switchDataGridView);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(571, 341);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "开关管理";
            // 
            // switchDataGridView
            // 
            this.switchDataGridView.AllowUserToAddRows = false;
            this.switchDataGridView.AllowUserToDeleteRows = false;
            this.switchDataGridView.AllowUserToResizeRows = false;
            this.switchDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.switchDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.开关序号,
            this.描述});
            this.switchDataGridView.Location = new System.Drawing.Point(8, 12);
            this.switchDataGridView.Name = "switchDataGridView";
            this.switchDataGridView.RowHeadersVisible = false;
            this.switchDataGridView.RowTemplate.Height = 23;
            this.switchDataGridView.Size = new System.Drawing.Size(552, 297);
            this.switchDataGridView.TabIndex = 0;
            // 
            // 开关序号
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.开关序号.DefaultCellStyle = dataGridViewCellStyle1;
            this.开关序号.Frozen = true;
            this.开关序号.HeaderText = "序号";
            this.开关序号.Name = "开关序号";
            this.开关序号.ReadOnly = true;
            this.开关序号.Width = 40;
            // 
            // 描述
            // 
            this.描述.HeaderText = "描述";
            this.描述.Name = "描述";
            this.描述.Width = 490;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(485, 315);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "保存";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // SymbolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 365);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SymbolForm";
            this.Text = "符号管理器";
            this.Load += new System.EventHandler(this.SymbolForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.globalvarDataGridView)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.switchDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        public System.Windows.Forms.ListBox funListBox;
        private System.Windows.Forms.Label mainNotEditableLabel;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView globalvarDataGridView;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn 变量名;
        private System.Windows.Forms.DataGridViewComboBoxColumn 类型;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView switchDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn 开关序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 描述;
        private System.Windows.Forms.Button button2;
    }
}