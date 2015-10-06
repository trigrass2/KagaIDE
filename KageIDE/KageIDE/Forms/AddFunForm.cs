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
    public partial class AddFunForm : Form, IBasicInputForm
    {
        public AddFunForm()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
        }

        // 取消
        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        // 添加参数
        private void button15_Click(object sender, EventArgs e)
        {
            
            BasicInputForm bif = new BasicInputForm("添加新参数", Consta.basicType);
            bif.ShowDialog(this);
            if (subParas == null)
            {
                return;
            }
        }

        // 通用子窗体消息传递函数
        public void passByBasicSubForm(string passer)
        {
            if (passer == null)
            {
                subParas = null;
            }
            else
            {
                subParas = passer;
            }
        }
        private string subParas = null;

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            int nrows = this.dataGridView2.Rows.Count - 1;
            for (int i = 0; i < nrows; i++)
            {
                sb.Append(String.Format(
                    "{0}@{1}\n",
                    this.dataGridView2.Rows[i].Cells[0].Value.ToString(),
                    this.dataGridView2.Rows[i].Cells[1].Value.ToString()));
            }
            string sr = sb.ToString();
        }
    }
}
