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
    public partial class ForLoopForm : Form
    {
        public ForLoopForm()
        {
            InitializeComponent();
        }

        private void ForLoopForm_Load(object sender, EventArgs e)
        {
            // 设置初始状态
            this.radioButton3.Checked = true;
            this.radioButton6.Checked = true;
            this.radioButton9.Checked = true;
            this.comboBox1.Enabled = false;
            this.comboBox2.Enabled = false;
            this.comboBox3.Enabled = false;
            this.textBox1.Enabled = false;
            this.textBox2.Enabled = false;
            this.textBox3.Enabled = false;
            this.checkBox1.Checked = true;
            // 加载全局变量表
            List<string> globalVarList = core.getGlobalVar();
            // 如果没有全局变量，那就封锁这个选项
            if (globalVarList.Count == 0)
            {
                this.radioButton2.Enabled = false;
                this.radioButton5.Enabled = false;
                this.radioButton10.Enabled = false;
            }
            else
            {
                foreach (string s in globalVarList)
                {
                    this.comboBox1.Items.Add(s);
                    this.comboBox2.Items.Add(s);
                    this.comboBox3.Items.Add(s);
                }
                this.comboBox1.SelectedIndex = 0;
                this.comboBox2.SelectedIndex = 0;
                this.comboBox3.SelectedIndex = 0;
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            this.numericUpDown1.Enabled = this.radioButton9.Checked;
            this.fltBegin = ForLoopType.FLT_CONSTANT;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.numericUpDown2.Enabled = this.radioButton3.Checked;
            this.fltStep = ForLoopType.FLT_CONSTANT;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            this.numericUpDown3.Enabled = this.radioButton6.Checked;
            this.fltEnd = ForLoopType.FLT_CONSTANT;
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBox2.Enabled = this.radioButton10.Checked;
            this.fltBegin = ForLoopType.FLT_GLOBAL;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBox1.Enabled = this.radioButton2.Checked;
            this.fltStep = ForLoopType.FLT_GLOBAL;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBox3.Enabled = this.radioButton5.Checked;
            this.fltEnd = ForLoopType.FLT_GLOBAL;
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox3.Enabled = this.radioButton11.Checked;
            this.fltBegin = ForLoopType.FLT_DEFVAR;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox1.Enabled = this.radioButton1.Checked;
            this.fltStep = ForLoopType.FLT_DEFVAR;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox2.Enabled = this.radioButton4.Checked;
            this.fltEnd = ForLoopType.FLT_DEFVAR;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.groupBox1.Enabled = this.groupBox2.Enabled = this.groupBox3.Enabled = false;
                this.numericUpDown4.Enabled = true;
            }
            else
            {
                this.groupBox1.Enabled = this.groupBox2.Enabled = this.groupBox3.Enabled = true;
                this.numericUpDown4.Enabled = false;
            }
        }

        // 确定
        private void button1_Click(object sender, EventArgs e)
        {
            // 处理参数
            string op1, op2, op3;
            // 简单模式下
            if (this.checkBox1.Checked)
            {
                this.fltBegin = ForLoopType.FLT_CONSTANT;
                this.fltEnd = ForLoopType.FLT_CONSTANT;
                this.fltStep = ForLoopType.FLT_CONSTANT;
                op1 = "0";
                op2 = "1";
                op3 = Convert.ToString(this.numericUpDown4.Value);
            }
            // 高级模式下
            else
            {
                switch (fltBegin)
                {
                    case ForLoopType.FLT_CONSTANT:
                        op1 = Convert.ToString(this.numericUpDown1.Value);
                        break;
                    case ForLoopType.FLT_GLOBAL:
                        op1 = (string)this.comboBox2.Items[this.comboBox2.SelectedIndex];
                        break;
                    case ForLoopType.FLT_DEFVAR:
                        op1 = this.textBox3.Text;
                        break;
                    default:
                        op1 = "";
                        break;
                }
                switch (fltStep)
                {
                    case ForLoopType.FLT_CONSTANT:
                        op2 = Convert.ToString(this.numericUpDown2.Value);
                        break;
                    case ForLoopType.FLT_GLOBAL:
                        op2 = (string)this.comboBox1.Items[this.comboBox1.SelectedIndex];
                        break;
                    case ForLoopType.FLT_DEFVAR:
                        op2 = this.textBox1.Text;
                        break;
                    default:
                        op2 = "";
                        break;
                }
                switch (fltEnd)
                {
                    case ForLoopType.FLT_CONSTANT:
                        op3 = Convert.ToString(this.numericUpDown3.Value);
                        break;
                    case ForLoopType.FLT_GLOBAL:
                        op3 = (string)this.comboBox3.Items[this.comboBox3.SelectedIndex];
                        break;
                    case ForLoopType.FLT_DEFVAR:
                        op3 = this.textBox2.Text;
                        break;
                    default:
                        op3 = "";
                        break;
                }
            }
            // 把数据传递给后台
            core.dash_forLoop(this.checkBox1.Checked, fltBegin, op1, fltEnd, op3, fltStep, op2);
            // 关闭
            this.Close();
        }

        private ForLoopType fltBegin = ForLoopType.FLT_CONSTANT;
        private ForLoopType fltStep = ForLoopType.FLT_CONSTANT;
        private ForLoopType fltEnd = ForLoopType.FLT_CONSTANT;
        private KagaController core = KagaController.getInstance();


    }
}
