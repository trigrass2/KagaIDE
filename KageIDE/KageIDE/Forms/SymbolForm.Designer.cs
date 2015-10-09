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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SymbolForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.funListBox = new System.Windows.Forms.ListBox();
            this.mainNotEditableLabel = new System.Windows.Forms.Label();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
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
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(571, 341);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "变量管理";
            this.tabPage2.UseVisualStyleBackColor = true;
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
    }
}