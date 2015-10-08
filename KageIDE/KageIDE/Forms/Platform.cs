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
        public Platform(MainForm pf)
        {
            InitializeComponent();
            this.parentForm = pf;
        }

        // 父窗体引用
        MainForm parentForm = null;

        // 窗体加载时
        private void Platform_Load(object sender, EventArgs e)
        {
            // 展开所有树节点
            this.treeView1.ExpandAll();
        }

        // 单击节点时发生
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // 弹出右键菜单时
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.treeView1.SelectedNode = e.Node;
                //Point pos = new Point(e.Node.Bounds.X + e.Node.Bounds.Width, e.Node.Bounds.Y + e.Node.Bounds.Height / 2);
                Point pos = new Point(Cursor.Position.X, Cursor.Position.Y);
                //this.contextMenuStrip1.Show(this.treeView1, pos);
                this.contextMenuStrip1.Show(pos);
            }
        }

        // 右键插入时
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.parentForm.moveCursorToInsertPlane();
        }

        // 双击节点插入
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            e.Node.Expand();
            this.parentForm.moveCursorToInsertPlane();
        }
    }
}
