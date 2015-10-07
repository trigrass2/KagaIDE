using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KagaIDE.Forms;
using KagaIDE.Module;

namespace KagaIDE
{
    public partial class MainForm : Form
    {
        // 控制器变量
        private KagaController core = KagaController.getInstance();

        public MainForm()
        {
            InitializeComponent();
        }

        // 加载窗体时发生
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (this.existTab("main", this.tabControl1) == false)
            {
                TabPage ntab = new TabPage("main");
                ntab.Name = "tbmobile";
                tabControl1.Controls.Add(ntab);
                Platform form = new Platform();
                form.TopLevel = false;
                form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Show();
                ntab.Controls.Add(form);
                tabControl1.SelectedTab = ntab;
            }
        }

        // 保证最小窗体尺寸
        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            if (this.Height < 730)
            {
                this.Height = 730;
            }
            if (this.Width < 730)
            {
                this.Width = 730;
            }
        }

        // 添加函数
        private void button18_Click(object sender, EventArgs e)
        {
            AddFunForm addFunForm = new AddFunForm();
            addFunForm.ShowDialog(this);
        }

        // 删除函数
        private void button17_Click(object sender, EventArgs e)
        {
            // 如果是main函数就不可以删除
            if (((string)this.functionListBox.SelectedItem) == "main")
            {
                MessageBox.Show("主函数main不可以被删除", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 否则，调用控制器删除方法
            core.deleteFunction((string)this.functionListBox.SelectedItem);
            // 刷新前台
            this.functionListBox.Items.RemoveAt(this.functionListBox.SelectedIndex);
        }

        // 点选某个项目
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //MessageBox.Show( e.Node.Index.ToString() + " " + e.Node.Level.ToString());
        }

        /// <summary>
        /// <param name="MainTabControlKey">选项卡的键值</param>
        /// <param name="objTabControl">要添加到的TabControl对象</param>
        /// <returns>是否存在</returns>
        /// </summary>
        private bool existTab(string MainTabControlKey, TabControl objTabControl)
        {
            //遍历选项卡判断是否存在该子窗体
            foreach (Control con in objTabControl.Controls)
            {
                TabPage tab = (TabPage)con;
                if (tab.Name == MainTabControlKey)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
