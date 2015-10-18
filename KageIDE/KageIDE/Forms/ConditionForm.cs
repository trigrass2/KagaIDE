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
    public partial class ConditionForm : Form
    {
        // 控制器核心
        private KagaController core = KagaController.getInstance();

        public ConditionForm()
        {
            InitializeComponent();
        }

        private void ConditionForm_Load(object sender, EventArgs e)
        {
            // 单选的初始状态
            this.radioButton3.Checked = true;
            this.radioButton9.Checked = true;
            this.comboBox2.Enabled = false;
            this.textBox3.Enabled = false;
            this.textBox2.Enabled = false;
            this.comboBox3.Enabled = false;
            this.comboBox4.Enabled = false;
            this.comboBox4.Items.Add("关闭");
            this.comboBox4.Items.Add("打开");
            this.comboBox4.SelectedIndex = 0;
            // 加载全局变量表
            List<string> globalVarList = core.getGlobalVar();
            // 如果没有全局变量，那就封锁这个选项
            if (globalVarList.Count == 0)
            {
                this.radioButton1.Enabled = false;
                this.radioButton2.Checked = true;
                this.textBox1.Enabled = true;
                this.comboBox1.Enabled = false;
                this.radioButton10.Enabled = false;
            }
            else
            {
                this.radioButton1.Checked = true;
                this.textBox1.Enabled = false;
                this.comboBox1.Enabled = true;
                this.radioButton10.Enabled = true;
                foreach (string s in globalVarList)
                {
                    this.comboBox1.Items.Add(s);
                    this.comboBox2.Items.Add(s);
                }
                this.comboBox1.SelectedIndex = 0;
                this.comboBox2.SelectedIndex = 0;
            }
            // 处理开关列表
            List<string> swList = core.getSwitchDescriptionVector();
            for (int i = 0; i < swList.Count; i++)
            {
                this.comboBox3.Items.Add(String.Format("{0}:{1}", i.ToString(), swList[i]));
            }
        }

        // 条件表达式
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked == true)
            {
                this.groupBox1.Enabled = this.groupBox2.Enabled = this.groupBox3.Enabled = false;
                this.textBox2.Enabled = true;
            }
            else
            {
                this.groupBox1.Enabled = this.groupBox3.Enabled = true;
                this.textBox2.Enabled = false;
                if (this.radioButton12.Checked == false)
                {
                    this.groupBox2.Enabled = true;
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                this.comboBox1.Enabled = true;
                this.Lot = OperandType.VO_GlobalVar;
            }
            else
            {
                this.comboBox1.Enabled = false;
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton9.Checked)
            {
                this.numericUpDown1.Enabled = true;
                this.Rot = OperandType.VO_Constant;
            }
            else
            {
                this.numericUpDown1.Enabled = false;
            }
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton10.Checked)
            {
                this.comboBox2.Enabled = true;
                this.Rot = OperandType.VO_GlobalVar;
            }
            else
            {
                this.comboBox2.Enabled = false;
            }
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton11.Checked)
            {
                this.textBox3.Enabled = true;
                this.Rot = OperandType.VO_DefVar;
            }
            else
            {
                this.textBox3.Enabled = false;
            }
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton13.Checked)
            {
                this.Rot = OperandType.VO_Switch;
            }
        }

        // 操作是开关时
        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton12.Checked)
            {
                this.comboBox3.Enabled = true;
                this.radioButton13.Enabled = true;
                this.radioButton13.Checked = true;
                this.comboBox4.Enabled = true;
                this.radioButton9.Enabled = this.radioButton10.Enabled = this.radioButton11.Enabled = false;
                this.radioButton3.Checked = true;
                this.groupBox2.Enabled = false;
                this.Lot = OperandType.VO_Switch;
            }
            else
            {
                this.comboBox3.Enabled = false;
                this.radioButton13.Enabled = false;
                this.radioButton13.Checked = false;
                this.comboBox4.Enabled = false;
                this.radioButton9.Enabled = this.radioButton10.Enabled = this.radioButton11.Enabled = true;
                this.groupBox2.Enabled = true;
                this.radioButton9.Checked = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked)
            {
                this.textBox1.Enabled = true;
                this.Lot = OperandType.VO_DefVar;
            }
            else
            {
                this.textBox1.Enabled = false;
            }
        }

        // 确定按钮
        private void button1_Click(object sender, EventArgs e)
        {
            // 正确性检查
            if (this.checkBox2.Checked == false)
            {
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
            }
            if (this.checkBox2.Checked && (this.textBox2.Text == "" || this.textBox2.Text == null))
            {
                MessageBox.Show("请完整填写");
                return;
            }
            // 处理要传递给后台的参数列表
            string condEx = this.checkBox2.Checked ? this.textBox2.Text : "";
            string lop, rop;
            switch (Lot)
            {
                case OperandType.VO_GlobalVar:
                    lop = (string)this.comboBox1.Items[this.comboBox1.SelectedIndex];
                    break;
                case OperandType.VO_DefVar:
                    lop = this.textBox1.Text;
                    break;
                case OperandType.VO_Switch:
                    lop = (string)this.comboBox3.Items[this.comboBox3.SelectedIndex];
                    break;
                default:
                    lop = null;
                    break;
            }
            switch (Rot)
            {
                case OperandType.VO_Constant:
                    rop = Convert.ToString(this.numericUpDown1.Value);
                    break;
                case OperandType.VO_GlobalVar:
                    rop = (string)this.comboBox2.Items[this.comboBox2.SelectedIndex];
                    break;
                case OperandType.VO_DefVar:
                    rop = this.textBox3.Text;
                    break;
                case OperandType.VO_Switch:
                    rop = (string)this.comboBox4.Items[this.comboBox4.SelectedIndex];
                    break;
                default:
                    rop = null;
                    break;
            }
            int laid = 0;
            for (int i = 0; i < this.groupBox2.Controls.Count; )
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
            // 把修改提交到后台
            core.dash_condition(this.checkBox2.Checked, condEx, this.checkBox1.Checked,
                this.Lot, lop, (CondOperatorType)laid,  this.Rot, rop);
            this.Close();
        }

        private OperandType Lot = OperandType.VO_DefVar;
        private OperandType Rot = OperandType.VO_Constant;
    }
}
