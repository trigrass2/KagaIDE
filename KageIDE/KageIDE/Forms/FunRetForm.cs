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
    public partial class FunRetForm : Form
    {
        // 控制器
        private KagaController core = KagaController.getInstance();
        private string frname = null;
        private VarType frtype = VarType.VOID;

        public FunRetForm(string frname)
        {
            InitializeComponent();
            this.frname = frname;
            this.Text += " [" + frname + "]";
        }

        private void FunRetForm_Load(object sender, EventArgs e)
        {
            // 读取这个函数的状态
            List<string> args;
            string retType;
            core.getFunction(this.frname, out args, out retType);
            this.frtype = Consta.parseCTypeToVarType(retType);
            if (this.frtype == VarType.VOID)
            {
                this.radioButton2.Enabled = this.radioButton3.Enabled = this.radioButton4.Enabled = false;
                this.textBox2.Enabled = this.textBox1.Enabled = this.comboBox1.Enabled = false;
                this.radioButton1.Checked = true;
            }
            else
            {
                this.radioButton1.Enabled = false;
                this.radioButton2.Checked = true;
                this.textBox2.Enabled = true;
                this.textBox1.Enabled = this.comboBox1.Enabled = false;
            }
            // 加载全局变量表
            List<string> globalVarList = core.getGlobalVar();
            if (globalVarList.Count == 0)
            {
                this.radioButton4.Enabled = false;
            }
            else
            {
                foreach (string s in globalVarList)
                {
                    this.comboBox1.Items.Add(s);
                }
                this.comboBox1.SelectedIndex = 0;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox2.Enabled = this.radioButton2.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox1.Enabled = this.radioButton3.Checked;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBox1.Enabled = this.radioButton4.Checked;
        }

        // 确定
        private void button1_Click(object sender, EventArgs e)
        {
            OperandType opt = OperandType.VO_VOID;
            string opr = null;
            if (this.radioButton2.Checked)
            {
                opt = OperandType.VO_Constant;
                opr = this.textBox2.Text;
            }
            else if (this.radioButton3.Checked)
            {
                opt = OperandType.VO_DefVar;
                opr = this.textBox1.Text;
            }
            else if (this.radioButton4.Checked)
            {
                opt = OperandType.VO_GlobalVar;
                opr = (string)this.comboBox1.Items[this.comboBox1.SelectedIndex];
            }
            core.dash_return(opt, opr);
            this.Close();
        }
    }
}
