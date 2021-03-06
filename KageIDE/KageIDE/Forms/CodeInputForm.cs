﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KagaIDE.Module;

namespace KagaIDE.Forms
{
    public partial class CodeInputForm : Form
    {
        // 后台实例
        private KagaController core = KagaController.getInstance();

        // 构造器
        public CodeInputForm(string name, string preText = "")
        {
            InitializeComponent();
            this.Text = name;
            this.codeTextBox.Text = preText;
            this.label1.Visible = name == "宏定义";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 代码片段时
            if (this.Text == "代码片段")
            {
                core.dash_codeblock(this.codeTextBox.Text);
            }
            // 注释时
            else if (this.Text == "注释")
            {
                core.dash_notation(this.codeTextBox.Text);
            }
            // 宏定义时
            else
            {
                core.setMarcos(this.codeTextBox.Text);
            }
            this.Close();
        }
    }
}
