namespace KagaIDE.Forms
{
    partial class Platform
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("◆ 定义变量：Guangzhi");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("◆ 变量操作：Guangzhi = LB()");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("◆ 函数：QianZhuiQiu()");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("当 变量Guanzhi 大于 30 时：", new System.Windows.Forms.TreeNode[] {
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("◆ 变量操作：flag = 1");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("◆ 函数：FindQiu()");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("除此以外的情况下：", new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode6});
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("◆ 条件分支", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("◆ 库函数：等待(10 ms)");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("◆ void main()", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode8,
            treeNode9});
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.treeView1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            treeNode1.Name = "节点1";
            treeNode1.Text = "◆ 定义变量：Guangzhi";
            treeNode2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            treeNode2.Name = "节点3";
            treeNode2.Text = "◆ 变量操作：Guangzhi = LB()";
            treeNode3.ForeColor = System.Drawing.Color.Fuchsia;
            treeNode3.Name = "节点10";
            treeNode3.Text = "◆ 函数：QianZhuiQiu()";
            treeNode4.ForeColor = System.Drawing.Color.Silver;
            treeNode4.Name = "节点8";
            treeNode4.Text = "当 变量Guanzhi 大于 30 时：";
            treeNode5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            treeNode5.Name = "节点14";
            treeNode5.Text = "◆ 变量操作：flag = 1";
            treeNode6.ForeColor = System.Drawing.Color.Fuchsia;
            treeNode6.Name = "节点11";
            treeNode6.Text = "◆ 函数：FindQiu()";
            treeNode7.ForeColor = System.Drawing.Color.Silver;
            treeNode7.Name = "节点9";
            treeNode7.Text = "除此以外的情况下：";
            treeNode8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            treeNode8.Name = "节点7";
            treeNode8.Text = "◆ 条件分支";
            treeNode9.ForeColor = System.Drawing.Color.Fuchsia;
            treeNode9.Name = "节点12";
            treeNode9.Text = "◆ 库函数：等待(10 ms)";
            treeNode10.ForeColor = System.Drawing.Color.DarkMagenta;
            treeNode10.Name = "节点0";
            treeNode10.Text = "◆ void main()";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode10});
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(621, 628);
            this.treeView1.TabIndex = 1;
            // 
            // Platform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 628);
            this.Controls.Add(this.treeView1);
            this.Name = "Platform";
            this.Text = "Platform";
            this.Load += new System.EventHandler(this.Platform_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;

    }
}