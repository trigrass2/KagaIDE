using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KagaIDE.Forms
{
    public partial class Platform : Form
    {
        public Platform()
        {
            InitializeComponent();
        }

        // 窗体加载时
        private void Platform_Load(object sender, EventArgs e)
        {
            // 展开所有树节点
            this.treeView1.ExpandAll();
        }
    }
}
