using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KagaIDE.Module;
using KagaIDE.Enuming;

namespace KagaIDE.Forms
{
    public partial class SymbolForm : Form
    {
        // 内核的唯一实例
        private KagaController core = KagaController.getInstance();

        public SymbolForm(int pageStop)
        {
            InitializeComponent();
            // 选中默认页面
            this.tabControl1.SelectedTab = this.tabControl1.TabPages[pageStop];
        }

        private void SymbolForm_Load(object sender, EventArgs e)
        {
            // 加载函数管理控件页面
            FunctionForm funform = new FunctionForm("管理函数");
            funform.TopLevel = false;
            funform.Location = new Point(165, 0);
            funform.Size = new System.Drawing.Size(500, 500);
            funform.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            funform.FormBorderStyle = FormBorderStyle.None;
            funform.Show();
            funform.Name = "FunForm";
            funform.setMainFormPointer((MainForm)this.Owner, this);
            this.tabControl1.TabPages[0].Controls.Add(funform);
            // 加载函数清单
            List<string> funList = core.getAllFunction();
            foreach (string s in funList)
            {
                this.funListBox.Items.Add(s);
            }
            this.funListBox.SelectedIndex = 0;
        }

        private void funListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 如果刷新动作被阻塞就返回
            if (Consta.refreshMutex == true)
            {
                return;
            }
            ((FunctionForm)this.tabControl1.TabPages[0].Controls["FunForm"]).refreshContext(
                (string)this.funListBox.SelectedItem);
            this.tabControl1.TabPages[0].Controls["FunForm"].Enabled = true;
            this.mainNotEditableLabel.Visible = false;
            // main函数不可编辑
            if ((string)this.funListBox.Items[this.funListBox.SelectedIndex] == "main")
            {
                this.tabControl1.TabPages[0].Controls["FunForm"].Enabled = false;
                this.mainNotEditableLabel.Visible = true;
            }
        }

        // 删除按钮
        private void button17_Click(object sender, EventArgs e)
        {
            // 如果没选中就不管
            if (this.funListBox.SelectedIndex == -1)
            {
                return;
            }
            // 如果是main函数就不可以删除
            if (((string)this.funListBox.SelectedItem) == "main")
            {
                MessageBox.Show("主函数main不可以被删除", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 否则，调用控制器删除方法
            core.deleteFunction((string)this.funListBox.SelectedItem);
            // 刷新前台
            ((MainForm)this.Owner).functionListBox.Items.Remove((string)this.funListBox.SelectedItem);
            ((MainForm)this.Owner).closeTabCard((string)this.funListBox.SelectedItem);
            ((MainForm)this.Owner).tabControl1.SelectedTab = ((MainForm)this.Owner).tabControl1.TabPages[0];
            Consta.refreshMutex = true;
            this.funListBox.Items.RemoveAt(this.funListBox.SelectedIndex);
            Consta.refreshMutex = false;
            this.funListBox.SelectedIndex = 0;
        }

        // 添加按钮
        private void button18_Click(object sender, EventArgs e)
        {
            FunctionForm addFunForm = new FunctionForm("从管理器新建函数");
            addFunForm.ShowDialog(this);
        }
    }
}
