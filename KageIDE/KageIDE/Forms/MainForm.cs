using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using KagaIDE.Forms;
using KagaIDE.Module;
using KagaIDE.Enuming;

namespace KagaIDE
{
    public partial class MainForm : Form, IBasicInputForm
    {
        [DllImport("user32.dll")]
        private static extern int SetCursorPos(int x, int y);

        // 控制器变量
        private KagaController core = KagaController.getInstance();

        public MainForm()
        {
            InitializeComponent();
            core.setMainForm(this);
        }

        // 加载窗体时发生
        private void MainForm_Load(object sender, EventArgs e)
        {
            // 添加main函数
            this.addTabCard("main");
            // 放置焦点
            this.tabControl1.Focus();
            // 刷新
            core.refreshAll();
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
            // 提示警告
            DialogResult dr = MessageBox.Show(
                String.Format("删除函数是不可撤销的操作！{0}真的要继续吗？", Environment.NewLine),
                "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                System.Threading.Thread.Sleep(800);
                dr = MessageBox.Show(
                String.Format("再次警告！{0}删除函数是不可撤销的操作！{1}确认真的要继续吗？", Environment.NewLine, Environment.NewLine),
                "警告", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    // 调用控制器删除方法
                    core.deleteFunction((string)this.functionListBox.SelectedItem);
                    // 刷新前台
                    this.closeTabCard((string)this.functionListBox.SelectedItem);
                    this.functionListBox.Items.RemoveAt(this.functionListBox.SelectedIndex);
                }
            }
            return;
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
                Platform form = new Platform(this);
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
                // main函数不可编辑
                if (fcname == "main")
                {
                    MessageBox.Show("主函数main的函数签名不可编辑", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
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

        // 菜单栏：编辑->插入命令
        private void 插入命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.moveCursorToInsertPlane();
        }

        // 菜单栏：视图->函数窗体
        private void 函数窗体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.函数窗体ToolStripMenuItem.Checked = !this.函数窗体ToolStripMenuItem.Checked;
            if (this.函数窗体ToolStripMenuItem.Checked == true)
            {
                this.groupBox2.Visible = true;
                this.tabControl1.Location = new Point(161, 34);
                this.tabControl1.Size = new Size(629, 654);
            }
            else
            {
                this.groupBox2.Visible = false;
                if (groupBox1.Visible == false)
                {
                    this.tabControl1.Location = new Point(7, 34);
                    this.tabControl1.Size = new Size(783, 654);
                }
            }
        }

        // 菜单栏：视图->全局变量窗体
        private void 全局变量窗体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.全局变量窗体ToolStripMenuItem.Checked = !this.全局变量窗体ToolStripMenuItem.Checked;
            if (this.全局变量窗体ToolStripMenuItem.Checked == true)
            {
                this.groupBox1.Visible = true;
                this.tabControl1.Location = new Point(161, 34);
                this.tabControl1.Size = new Size(629, 654);
            }
            else
            {
                this.groupBox1.Visible = false;
                if (groupBox2.Visible == false)
                {
                    this.tabControl1.Location = new Point(7, 34);
                    this.tabControl1.Size = new Size(783, 654);
                }
            }
        }

        // 移动鼠标到插入命令处
        public void moveCursorToInsertPlane()
        {
            this.moveCursorToPoint(
                this.button1,
                this.button1.Location,
                Cursor.Position,
                this.button1.Size.Width / 3,
                this.button1.Size.Height / 4);
        }
        private double cur_dx = 0, cur_dy = 0;
        private int timerEncounter = 0;

        // 移动鼠标到目标点
        public void moveCursorToPoint(Control control, Point desPoint, Point curPoint, double offsetX, double offsetY, double div = 6.0)
        {
            try
            {
                Point pointToScreen = control.PointToScreen(desPoint);
                cur_dx = (pointToScreen.X - Cursor.Position.X + offsetX) / div;
                cur_dy = (pointToScreen.Y - Cursor.Position.Y + offsetY) / div;
                timer1.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 插入命令时动态移动指针
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Point currentPoint = Cursor.Position;
                SetCursorPos((int)(currentPoint.X + cur_dx), (int)(currentPoint.Y + cur_dy));
                timerEncounter++;
                if (timerEncounter >= 6)
                {
                    timerEncounter = 0;
                    this.timer1.Stop();
                }
            }
            catch (Exception ex)
            {
                this.timer1.Stop();
                throw ex;
            }
        }

        // 菜单->关于
        private void 关于KagaIDEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox af = new AboutBox();
            af.ShowDialog(this);
        }

        // 菜单->宏定义
        private void 宏定义ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeInputForm cif = new CodeInputForm("宏定义", core.getMarcos());
            cif.ShowDialog(this);
        }

        // 菜单->退出程序
        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                String.Format("真的要退出吗？{0}未保存的文件或修改将会丢失", Environment.NewLine),
                "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                System.Environment.Exit(0);
            }
        }

        // 菜单->函数管理器
        private void 函数管理器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SymbolForm sf = new SymbolForm(0);
            sf.ShowDialog(this);
        }

        // 菜单->变量管理器
        private void 变量管理器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SymbolForm sf = new SymbolForm(1);
            sf.ShowDialog(this);
        }

        // 菜单->开关管理器
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            SymbolForm sf = new SymbolForm(2);
            sf.ShowDialog(this);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            core.refreshAll();
        }

        // 插入指令：变量定义
        private void button1_Click(object sender, EventArgs e)
        {
            // 检查这个节点可否插入变量
            if (((TreeView)this.tabControl1.SelectedTab.Controls.Find("codeTreeView", true)[0]).SelectedNode == null)
            {
                return;
            }
            if (core.isAbleInsertDefineVar() == false)
            {
                MessageBox.Show("这个节点不可以插入变量定义", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.moveCursorToPoint(this, new Point(this.Size.Width / 2, this.Size.Height / 2), Cursor.Position, 0, 0);
            BasicInputForm bif = new BasicInputForm("定义变量", Consta.basicType);
            bif.ShowDialog(this);
            // 如果没有传回任何东西就结束
            if (passBuffer == null)
            {
                return;
            }
            // 插入定义变量操作
            core.dash_defineVariable(passBuffer);
            passBuffer = null;
        }

        // 插入指令：开关操作
        private void button4_Click(object sender, EventArgs e)
        {
            // 检查这个节点可否插入
            if (((TreeView)this.tabControl1.SelectedTab.Controls.Find("codeTreeView", true)[0]).SelectedNode == null)
            {
                return;
            }
            if (((TreeView)this.tabControl1.SelectedTab.Controls.Find("codeTreeView", true)[0]).SelectedNode.Level == 0)
            {
                MessageBox.Show("这个节点不可以插入开关操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.moveCursorToPoint(this, new Point(this.Size.Width / 2, this.Size.Height / 2), Cursor.Position, 0, 0);
            SwitchForm sf = new SwitchForm();
            sf.ShowDialog(this);
        }

        // 插入指令：变量操作
        private void button2_Click(object sender, EventArgs e)
        {
            // 检查这个节点可否插入
            if (((TreeView)this.tabControl1.SelectedTab.Controls.Find("codeTreeView", true)[0]).SelectedNode == null)
            {
                return;
            }
            if (((TreeView)this.tabControl1.SelectedTab.Controls.Find("codeTreeView", true)[0]).SelectedNode.Level == 0)
            {
                MessageBox.Show("这个节点不可以插入开关操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.moveCursorToPoint(this, new Point(this.Size.Width / 2, this.Size.Height / 2), Cursor.Position, 0, 0);
            VariableForm vf = new VariableForm();
            vf.ShowDialog(this);
        }

        // 插入指令：注释
        private void button9_Click(object sender, EventArgs e)
        {
            CodeInputForm cif = new CodeInputForm("注释");
            cif.ShowDialog(this);
        }

        // 插入指令：条件语句
        private void button5_Click(object sender, EventArgs e)
        {
            ConditionForm cf = new ConditionForm();
            cf.ShowDialog(this);
        }

    }
}
