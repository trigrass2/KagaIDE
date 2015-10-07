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
    public partial class AddFunForm : Form
    {
        // 控制器实例
        private KagaController core = KagaController.getInstance();

        public AddFunForm()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
        }

        // 取消
        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        // 确定
        private void button1_Click(object sender, EventArgs e)
        {
            // 处理参数列表
            List<string> argvList = new List<string>();
            int nrows = this.dataGridView2.Rows.Count - 1;
            for (int i = 0; i < nrows; i++)
            {
                string varname = this.dataGridView2.Rows[i].Cells[0].Value.ToString();
                // 类型不能为空
                if (this.dataGridView2.Rows[i].Cells[1].Value == null)
                {
                    MessageBox.Show(String.Format("变量{0}类型不合法", varname), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string vartype = this.dataGridView2.Rows[i].Cells[1].Value.ToString();
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
            string rettype = (string)this.comboBox1.SelectedItem;
            // 通过控制器传递给后台
            bool flag = core.addFunction(callname, argvList, rettype);
            // 重名错误时
            if (flag == false)
            {
                MessageBox.Show("函数名已经存在，\n请修改函数名！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // 刷新前台
            ((MainForm)(this.Owner)).functionListBox.Items.Add(callname);
            this.Close();
        }

        // 设置默认值
        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (this.dataGridView2.CurrentCell.ColumnIndex == 1)
            {
                ComboBox cb = e.Control as ComboBox;
                if (cb != null)
                {
                    cb.SelectedIndex = 0;
                }
            }
        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.dataGridView2.Rows[e.RowIndex].Cells[1].Value = Consta.basicType[0];
        }

    }
}
