using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KagaIDE
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            if (this.Height < 730)
            {
                this.Height = 730;
            }
            if (this.Width < 730)
            {
                this.Width = 730;
            }
        }


    }
}
