using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KagaIDE.Forms;
using KagaIDE.Module;
using KagaIDE.Enuming;

namespace KagaIDE
{
    public partial class MainForm : Form, IBasicInputForm
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
            this.addTabCard("main");
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
            FunctionForm addFunForm = new FunctionForm("新建函数");
            addFunForm.ShowDialog(this);
        }

        // 删除函数
        private void button17_Click(object sender, EventArgs e)
        {
            // 如果没选中就不管
            if (this.functionListBox.SelectedIndex == -1)
            {
                return;
            }
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
        /// 加载一个选项卡
        /// </summary>
        /// <param name="fcname">选项卡名称</param>
        public void addTabCard(string fcname)
        {
            if (this.existTab(fcname, this.tabControl1) == false)
            {
                TabPage ntab = new TabPage(fcname);
                ntab.Name = fcname;
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

        /// <summary>
        /// 关闭一个选项卡
        /// </summary>
        /// <param name="fcname">选项卡名称</param>
        public void closeTabCard(string fcname)
        {
            if (this.existTab(fcname, this.tabControl1) == true)
            {
                this.tabControl1.Controls.RemoveByKey(fcname);
            }
        }

        /// <summary>
        /// 是否已经打开了某个函数卡
        /// </summary>
        /// <param name="MainTabControlKey">选项卡的键值</param>
        /// <param name="objTabControl">要添加到的TabControl对象</param>
        /// <returns>是否存在这个选项卡</returns>
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

        // 双击函数列表时
        private void functionListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 获取选中的项目
            int index = this.functionListBox.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                string fcname = (string)this.functionListBox.Items[index];
                FunctionForm addFunForm = new FunctionForm("编辑函数", fcname);
                addFunForm.ShowDialog(this);
            }
        }

        // 代码片段按钮
        private void button14_Click(object sender, EventArgs e)
        {
            CodeInputForm cif = new CodeInputForm("代码片段");
            cif.ShowDialog(this);

        }

        // 宏定义按钮
        private void button3_Click(object sender, EventArgs e)
        {
            CodeInputForm cif = new CodeInputForm("宏定义", core.getMarcos());
            cif.ShowDialog(this);
        }

        // 添加全局变量按钮
        private void button15_Click(object sender, EventArgs e)
        {
            // 开启简单输入窗口并阻塞
            BasicInputForm rif = new BasicInputForm("新建全局变量", Consta.basicType);
            rif.ShowDialog(this);
            // 如果没有传回任何东西就结束
            if (passBuffer == null)
            {
                return;
            }
            // 把更新传到后台
            string[] splitItem = passBuffer.Split('@');
            if (core.addGlobalVar(splitItem[0], splitItem[1]) == false)
            {
                MessageBox.Show("变量名重复，请重新命名", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 阻塞结束后，更新窗体
            this.globalvarListBox.Items.Add(passBuffer.Replace("@", " @ "));
            passBuffer = null;
        }

        // 删除全局变量按钮
        private void button16_Click(object sender, EventArgs e)
        {
            // 如果没选中就不管
            if (this.globalvarListBox.SelectedIndex == -1)
            {
                return;
            }
            // 把更新传到后台
            string[] splitItem = ((string)this.globalvarListBox.Items[this.globalvarListBox.SelectedIndex]).Split('@');
            core.deleteGlobalVar(splitItem[0].TrimEnd(' '));
            // 从窗体删除这个项目
            this.globalvarListBox.Items.Remove(this.globalvarListBox.SelectedItem);
        }

        // 基础输入窗口接受函数
        public void passByBasicSubForm(string passer)
        {
            this.passBuffer = passer;
        }
        private string passBuffer = null;

    }
}
