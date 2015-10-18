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
        // 后台实例
        private KagaController core = KagaController.getInstance();

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
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.numericUpDown2.Enabled = this.radioButton3.Checked;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            this.numericUpDown3.Enabled = this.radioButton6.Checked;
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBox2.Enabled = this.radioButton10.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBox1.Enabled = this.radioButton2.Checked;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBox3.Enabled = this.radioButton5.Checked;
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox3.Enabled = this.radioButton11.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox1.Enabled = this.radioButton1.Checked;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox2.Enabled = this.radioButton4.Checked;
        }


    }
}
