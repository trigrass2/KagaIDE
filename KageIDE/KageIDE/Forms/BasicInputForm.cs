using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KagaIDE.Enuming;

namespace KagaIDE.Forms
{
    public partial class BasicInputForm : Form
    {
        public BasicInputForm(string title = "Kaga IDE", List<string> comboList = null)
        {
            InitializeComponent();
            this.Text = title;
            if (comboList != null)
            {
                foreach(string s in comboList)
                {
                    this.comboBox1.Items.Add(s);
                }
            }
            else
            {
                comboBox1.Visible = false;
            }
        }

        // 取消
        private void button2_Click(object sender, EventArgs e)
        {
            ((IBasicInputForm)this.Owner).passByBasicSubForm(null);
            this.Dispose();
        }

        // 确定
        private void button1_Click(object sender, EventArgs e)
        {
            if ((comboBox1.Visible == true && comboBox1.SelectedIndex == -1) 
                || textBox1.Text == null || textBox1.Text == "")
            {
                MessageBox.Show("请完整填写", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 变量名合法性检查
            if (this.Text.Contains("变量") == true && Consta.IsStdCSymbol(this.textBox1.Text) == false)
            {
                MessageBox.Show(
                    String.Format("变量 {0} 命名不合法", this.textBox1.Text), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ((IBasicInputForm)this.Owner).passByBasicSubForm(
                String.Format("{0}@{1}", this.textBox1.Text, this.comboBox1.SelectedItem.ToString()));
            this.Dispose();
        }

    }
}
