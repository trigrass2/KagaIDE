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
    public partial class VariableForm : Form
    {
        // 控制器核心
        private KagaController core = KagaController.getInstance();

        public VariableForm()
        {
            InitializeComponent();
        }

        private void VariableForm_Load(object sender, EventArgs e)
        {
            // 窗体加载时默认的选择情况
            this.radioButton1.Checked = true;
            this.radioButton3.Checked = true;
            this.radioButton9.Checked = true;
            this.textBox1.Enabled = false;
            this.textBox3.Enabled = false;
            this.numericUpDown2.Enabled = false;
            this.numericUpDown3.Enabled = false;
            this.comboBox2.Enabled = false;
            // 加载全局变量表
            List<string> globalVarList = core.getGlobalVar();
            // 如果没有全局变量，那就封锁这个选项
            if (globalVarList.Count == 0)
            {
                this.radioButton2.Checked = true;
                this.radioButton1.Enabled = false;
                this.radioButton10.Enabled = false;
            }
            else
            {
                foreach (string s in globalVarList)
                {
                    this.comboBox1.Items.Add(s);
                    this.comboBox2.Items.Add(s);
                }
                this.comboBox1.SelectedIndex = 0;
                this.comboBox2.SelectedIndex = 0;
            }
            // 放置焦点
            this.numericUpDown1.Focus();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBox1.Enabled = this.radioButton1.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox1.Enabled = this.radioButton2.Checked;
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            this.numericUpDown1.Enabled = this.radioButton9.Checked;
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBox2.Enabled = this.radioButton10.Checked;
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox3.Enabled = this.radioButton11.Checked;
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            this.numericUpDown2.Enabled = this.numericUpDown3.Enabled = this.radioButton12.Checked;
        }

        // 确定按钮
        private void button1_Click(object sender, EventArgs e)
        {
            // 正确性检查
            if (this.radioButton2.Checked && (this.textBox1.Text == null || this.textBox1.Text == ""))
            {
                MessageBox.Show("请完整填写");
                return;
            }
            if (this.radioButton11.Checked && (this.textBox3.Text == null || this.textBox3.Text == ""))
            {
                MessageBox.Show("请完整填写");
                return;
            }
            // 符号合法性
            if (this.radioButton2.Checked && Consta.IsStdCSymbol(this.textBox1.Text) == false)
            {
                MessageBox.Show(String.Format("变量 {0} 命名不合法", this.textBox1.Text), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (this.radioButton11.Checked && Consta.IsStdCSymbol(this.textBox3.Text) == false)
            {
                MessageBox.Show(String.Format("变量 {0} 命名不合法", this.textBox3.Text), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 把修改提交到后台
            int laid = 0;
            for (int i = 0; i < this.groupBox2.Controls.Count;)
            {
                if (this.groupBox2.Controls[i] is RadioButton)
                {
                    if (((RadioButton)this.groupBox2.Controls[i]).Checked == true)
                    {
                        string[] sname = ((RadioButton)this.groupBox2.Controls[i]).Name.Split('n');
                        laid = Convert.ToInt32(sname[1]) - 3;
                        break;
                    }
                    i++;
                }
            }
            OperandType raid = OperandType.VO_Constant;
            string rop1 = null, rop2 = null;
            if (this.radioButton9.Checked)
            {
                raid = OperandType.VO_Constant;
                rop1 = Convert.ToString(this.numericUpDown1.Value);
            }
            else if (this.radioButton10.Checked)
            {
                raid = OperandType.VO_GlobalVar;
                rop1 = ((string)(this.comboBox1.Items[this.comboBox1.SelectedIndex])).Split('@')[0];
            }
            else if (this.radioButton11.Checked)
            {
                raid = OperandType.VO_DefVar;
                rop1 = (string)this.textBox3.Text;
            }
            else if (this.radioButton12.Checked)
            {
                raid = OperandType.VO_Random;
                rop1 = Convert.ToString(this.numericUpDown2.Value);
                rop2 = Convert.ToString(this.numericUpDown3.Value);
            }
            core.dash_varOperator(
                (OperandType)(this.radioButton1.Checked ? OperandType.VO_GlobalVar : OperandType.VO_DefVar),
                this.radioButton1.Checked ? (string)this.comboBox1.Text.Split('@')[0] : this.textBox1.Text,
                (VarOperateType)(laid), raid, rop1, rop2);
            this.Close();
        }

        // 随机数下界检查
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (this.numericUpDown2.Value >= this.numericUpDown3.Value)
            {
                MessageBox.Show("随机数下界不可以超过或等于上界");
                this.numericUpDown2.Value = this.numericUpDown3.Value - 1;
                return;
            }
        }

        // 随机数上界检查
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (this.numericUpDown3.Value <= this.numericUpDown2.Value)
            {
                MessageBox.Show("随机数上界不可以低于或等于下界");
                this.numericUpDown3.Value = this.numericUpDown2.Value + 1;
                return;
            }
        }
    }
}
