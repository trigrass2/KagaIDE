using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KagaIDE.Enuming;
using KagaIDE.Module;

namespace KagaIDE.Forms
{
    public partial class CondLoopForm : Form
    {
        public CondLoopForm()
        {
            InitializeComponent();
        }

        private void CondLoopForm_Load(object sender, EventArgs e)
        {
            // 设置初始状态
            this.radioButton1.Checked = true;
            this.comboBox1.Enabled = false;
            this.textBox1.Enabled = false;
            // 处理开关列表
            List<string> swList = core.getSwitchDescriptionVector();
            for (int i = 0; i < swList.Count; i++)
            {
                this.comboBox1.Items.Add(String.Format("{0}:{1}", i.ToString(), swList[i]));
            }
            this.comboBox1.SelectedIndex = 0;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                this.condLoopType = CondLoopType.CLT_FOREVER;
                this.operand = "___KAGA_FOREVER";
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked)
            {
                this.comboBox1.Enabled = true;
                this.condLoopType = CondLoopType.CLT_SWITCH;
            }
            else
            {
                this.comboBox1.Enabled = false;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton3.Checked)
            {
                this.textBox1.Enabled = true;
                this.condLoopType = CondLoopType.CLT_EXPRESSION;
            }
            else
            {
                this.textBox1.Enabled = false;
            }
        }

        // 确定
        private void button1_Click(object sender, EventArgs e)
        {
            if (condLoopType == CondLoopType.CLT_SWITCH)
            {
                this.operand = (string)this.comboBox1.Items[this.comboBox1.SelectedIndex];
            }
            else if (condLoopType == CondLoopType.CLT_EXPRESSION)
            {
                this.operand = this.textBox1.Text;
            }
            core.dash_condLoop(this.condLoopType, operand, this.checkBox1.Checked);
            this.Close();
        }

        private CondLoopType condLoopType = CondLoopType.CLT_FOREVER;
        private string operand = "___KAGA_FOREVER";
        private KagaController core = KagaController.getInstance();
    }
}
