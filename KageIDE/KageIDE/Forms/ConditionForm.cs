using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KagaIDE.Module;

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
            this.radioButton3.Checked = true;
            this.radioButton9.Checked = true;
            this.comboBox2.Enabled = false;
            this.textBox3.Enabled = false;
            this.textBox2.Enabled = false;
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
                this.groupBox1.Enabled = this.groupBox2.Enabled = this.groupBox3.Enabled = true;
                this.textBox2.Enabled = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBox1.Enabled = !this.comboBox1.Enabled;
            this.textBox1.Enabled = !this.textBox1.Enabled;
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton9.Checked)
            {
                this.numericUpDown1.Enabled = true;
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
            }
            else
            {
                this.textBox3.Enabled = false;
            }
        }
    }
}
