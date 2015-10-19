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
    public partial class FunCallForm : Form
    {
        private KagaController core = KagaController.getInstance();
        private List<string> args = null;
        public FunCallForm()
        {
            InitializeComponent();
        }

        private void FunCallForm_Load(object sender, EventArgs e)
        {
            // 获取函数向量
            List<string> funList = core.getAllFunction();
            foreach (string s in funList)
            {
                if (s != "main")
                {
                    this.comboBox1.Items.Add(s);
                }
            }
            if (this.comboBox1.Items.Count != 0)
            {
                this.comboBox1.SelectedIndex = 0;
            }
            else
            {
                this.comboBox1.Enabled = false;
                this.argsGridDataView.Enabled = false;
                this.button1.Enabled = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fcname = (string)this.comboBox1.SelectedItem;
            string rettype;
            core.getFunction(fcname, out args, out rettype);
            this.label2.Text = rettype;
            // 处理参数列表
            this.argsGridDataView.Rows.Clear();
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

        // 确定
        private void button1_Click(object sender, EventArgs e)
        {
            // 检查空值并处理参数
            string argPairStr = "";
            for (int i = 0; i < this.argsGridDataView.Rows.Count; i++)
            {
                if ((string)(this.argsGridDataView.Rows[i].Cells[2].Value) == "" 
                    || this.argsGridDataView.Rows[i].Cells[2].Value == null)
                {
                    MessageBox.Show("请完整填写", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    argPairStr += args[i] + ":" + (string)this.argsGridDataView.Rows[i].Cells[2].Value + "//";
                }
            }
            // 提交给后台
            core.dash_funcall((string)this.comboBox1.SelectedItem, 
                argPairStr.Length > 2 ? argPairStr.Remove(argPairStr.Length - 2) : argPairStr);
            // 关闭
            this.Close();
        }
    }
}
