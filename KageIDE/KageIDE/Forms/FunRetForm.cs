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
    }
}
