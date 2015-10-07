using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KagaIDE.Enuming;
using KagaIDE.Module;

namespace KagaIDE.Forms
{
    public partial class FunctionForm : Form
    {
        // 控制器实例
        private KagaController core = KagaController.getInstance();

        // 函数旧名字
        private string oldfcname = "";

        // 构造器
        public FunctionForm(string title, string fname = "")
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
            this.Text = title;
            this.oldfcname = fname;
            if (title == "编辑函数")
            {
                string rettype;
                List<string> args;
                core.getFunction(fname, out args, out rettype);
                // 处理函数头
                this.textBox1.Text = fname;
                // 处理返回类型
                this.comboBox1.SelectedIndex = this.comboBox1.Items.IndexOf(rettype);
                // 处理参数列表
                if (args != null)
                {
                    for (int i = 0; i < args.Count; i++)
                    {
                        this.argsGridDataView.Rows.Add();
                        string[] splitItem = args[i].Split('@');
                        this.argsGridDataView.Rows[i].Cells[0].Value = splitItem[0];
                        this.argsGridDataView.Rows[i].Cells[1].Value = splitItem[1];
                    }
                }
            }
        }

        // 取消
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 确定
        private void button1_Click(object sender, EventArgs e)
        {
            // 处理参数列表
            List<string> argvList = new List<string>();
            int nrows = this.argsGridDataView.Rows.Count - 1;
            for (int i = 0; i < nrows; i++)
            {
                string varname = this.argsGridDataView.Rows[i].Cells[0].Value.ToString();
                // 类型不能为空
                if (this.argsGridDataView.Rows[i].Cells[1].Value == null)
                {
                    MessageBox.Show(String.Format("变量{0}类型不合法", varname), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string vartype = this.argsGridDataView.Rows[i].Cells[1].Value.ToString();
                // 符号合法性
                if (Consta.IsStdCSymbol(varname) == false)
                {
                    MessageBox.Show(String.Format("变量{0}命名不合法", varname), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // 符号唯一性
                if (argvList.Find((x) => x.Split('@')[0] == varname) != null)
                {
                    MessageBox.Show(String.Format("变量名{0}重复", varname), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // 类型合法性
                if (Consta.basicType.Find((x) => x == vartype) == null)
                {
                    MessageBox.Show(String.Format("变量{0}类型不合法", varname), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                argvList.Add(String.Format("{0}@{1}", varname, vartype));
            }
            // 处理函数头部
            string callname = this.textBox1.Text;
            if (Consta.IsStdCSymbol(callname) == false)
            {
                MessageBox.Show(String.Format("函数命名不合法"), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 处理返回类型
            string rettype = (string)this.comboBox1.SelectedItem;
            // 如果是新建函数
            if (this.Text == "新建函数")
            {
                // 通过控制器传递给后台
                bool flag = core.addFunction(callname, argvList, rettype);
                // 重名错误时
                if (flag == false)
                {
                    MessageBox.Show("函数名已经存在，\n请修改函数名！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // 刷新前台
                MainForm father = (MainForm)(this.Owner);
                father.functionListBox.Items.Add(callname);
                father.addTabCard(callname);
                this.Close();
            }
            // 如果是编辑函数
            else
            {
                // 通过控制器传递给后台
                bool flag = core.editFunction(this.oldfcname, callname, argvList, rettype);
                // 空引用错误时
                if (flag == false)
                {
                    MessageBox.Show("未知错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // 刷新前台
                MainForm father = (MainForm)(this.Owner);
                father.closeTabCard(this.oldfcname);
                father.functionListBox.Items.Remove(this.oldfcname);
                father.functionListBox.Items.Add(callname);
                father.addTabCard(callname);
                this.Close();
            }

        }
        
        // 设置默认值
        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (this.argsGridDataView.CurrentCell.ColumnIndex == 1)
            {
                ComboBox cb = e.Control as ComboBox;
                if (cb != null)
                {
                    cb.SelectedIndex = 0;
                }
            }
        }

        // 点击某个单元格
        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // 左键时为类型赋初值
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.argsGridDataView.Rows[e.RowIndex].Cells[1].Value = Consta.basicType[0];
            }
            // 右键删除变量
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (e.RowIndex != this.argsGridDataView.Rows.Count - 1)
                {
                    this.argsGridDataView.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

    }
}
