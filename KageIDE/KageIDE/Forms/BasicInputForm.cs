using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
            ((IBasicInputForm)this.Owner).passByBasicSubForm(
                String.Format("{0}@{1}", this.textBox1.Text, this.comboBox1.SelectedItem.ToString()));
            this.Dispose();
        }

    }
}
