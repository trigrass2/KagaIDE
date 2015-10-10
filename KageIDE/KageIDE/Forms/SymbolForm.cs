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
            // 加载全局变量
            List<string> globalVector = core.getGlobalVar();
            this.globalvarDataGridView.Rows.Clear();
            for (int i = 0; i < globalVector.Count; i++)
            {
                this.globalvarDataGridView.Rows.Add();
                string[] splitItem = globalVector[i].Split('@');
                this.globalvarDataGridView.Rows[i].Cells[0].Value = splitItem[0];
                this.globalvarDataGridView.Rows[i].Cells[1].Value = splitItem[1];
            }
            // 加载开关列表
            for (int i = 0; i < Consta.switch_max; i++)
            {
                this.switchDataGridView.Rows.Add();
                this.switchDataGridView.Rows[i].Cells[0].Value = i;
                this.switchDataGridView.Rows[i].Cells[1].Value = core.getSwitchDescriptionVector()[i];
            }
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
            // 提示警告
            DialogResult dr = MessageBox.Show(
                String.Format("删除函数是不可撤销的操作！{0}真的要继续吗？", Environment.NewLine),
                "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                System.Threading.Thread.Sleep(1000);
                dr = MessageBox.Show(
                String.Format("再次警告！{0}删除函数是不可撤销的操作！{1}确认真的要继续吗？", Environment.NewLine, Environment.NewLine),
                "警告", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    // 调用控制器删除方法
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
            }
            return;
        }

        // 添加按钮
        private void button18_Click(object sender, EventArgs e)
        {
            FunctionForm addFunForm = new FunctionForm("从管理器新建函数");
            addFunForm.ShowDialog(this);
        }

        private void globalvarDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (this.globalvarDataGridView.CurrentCell.ColumnIndex == 1)
            {
                ComboBox cb = e.Control as ComboBox;
                if (cb != null)
                {
                    cb.SelectedIndex = 0;
                }
            }
        }

        private void globalvarDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // 单元格不存在时
                if (e.RowIndex == -1 || e.ColumnIndex == -1)
                {
                    return;
                }
                // 左键时为类型赋初值
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if ((this.globalvarDataGridView.Rows[e.RowIndex].Cells[0].Value) == null)
                    {
                        this.globalvarDataGridView.Rows[e.RowIndex].Cells[1].Value = Consta.basicType[0];
                    }
                }
                // 右键删除变量
                else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    // 边界要减一，因为最后一行是没有提交的
                    if (e.RowIndex != this.globalvarDataGridView.Rows.Count - 1)
                    {
                        this.globalvarDataGridView.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 全局变量保存
        private void button1_Click(object sender, EventArgs e)
        {
            // 处理参数列表
            List<string> argvList = new List<string>();
            int nrows = this.globalvarDataGridView.Rows.Count - 1;
            for (int i = 0; i < nrows; i++)
            {
                string varname = this.globalvarDataGridView.Rows[i].Cells[0].Value.ToString();
                // 类型不能为空
                if (this.globalvarDataGridView.Rows[i].Cells[1].Value == null)
                {
                    MessageBox.Show(String.Format("变量 {0} 类型不合法", varname), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string vartype = this.globalvarDataGridView.Rows[i].Cells[1].Value.ToString();
                // 符号合法性
                if (Consta.IsStdCSymbol(varname) == false)
                {
                    MessageBox.Show(String.Format("变量 {0} 命名不合法", varname), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // 符号唯一性
                if (argvList.Find((x) => x.Split('@')[0] == varname) != null)
                {
                    MessageBox.Show(String.Format("变量名 {0} 重复", varname), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // 类型合法性
                if (Consta.basicType.Find((x) => x == vartype) == null)
                {
                    MessageBox.Show(String.Format("变量 {0} 类型不合法", varname), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                argvList.Add(String.Format("{0}@{1}", varname, vartype));
            }
            // 把更改提交给后台
            core.setNewGlobalVar(argvList);
            // 刷新主窗体
            ((MainForm)this.Owner).globalvarListBox.Items.Clear();
            foreach (string s in core.getGlobalVar())
            {
                ((MainForm)this.Owner).globalvarListBox.Items.Add((string)s.Replace("@", " @ "));
            }
        }

        // 开关管理页保存
        private void button2_Click(object sender, EventArgs e)
        {
            // 处理参数列表
            List<string> argvList = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                if (this.switchDataGridView.Rows[i].Cells[1].Value == null)
                {
                    argvList.Add("");
                }
                else
                {
                    string descript = this.switchDataGridView.Rows[i].Cells[1].Value.ToString();
                    argvList.Add(descript);
                }
            }
            // 把更新提交到后台
            core.updateSwitchDescriptionVector(argvList);
        }
    }
}
