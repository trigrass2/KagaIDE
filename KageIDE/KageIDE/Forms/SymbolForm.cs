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
    public partial class SymbolForm : Form
    {
        // 内核的唯一实例
        private KagaController core = KagaController.getInstance();

        public SymbolForm(int pageStop)
        {
            InitializeComponent();
            // 选中默认页面
            this.tabControl1.SelectedTab = this.tabControl1.TabPages[pageStop];
        }

        private void SymbolForm_Load(object sender, EventArgs e)
        {
            // 加载函数管理控件页面
            FunctionForm funform = new FunctionForm("管理函数");
            funform.TopLevel = false;
            funform.Location = new Point(165, 0);
            funform.Size = new System.Drawing.Size(500, 500);
            funform.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            funform.FormBorderStyle = FormBorderStyle.None;
            funform.Show();
            funform.Name = "FunForm";
            funform.setMainFormPointer((MainForm)this.Owner, this);
            this.tabControl1.TabPages[0].Controls.Add(funform);
            // 加载函数清单
            List<string> funList = core.getAllFunction();
            foreach (string s in funList)
            {
                this.funListBox.Items.Add(s);
            }
            this.funListBox.SelectedIndex = 0;
        }

        private void funListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((FunctionForm)this.tabControl1.TabPages[0].Controls["FunForm"]).refreshContext(
                (string)this.funListBox.SelectedItem);
            this.tabControl1.TabPages[0].Controls["FunForm"].Enabled = true;
            // main函数不可编辑
            if ((string)this.funListBox.Items[this.funListBox.SelectedIndex] == "main")
            {
                this.tabControl1.TabPages[0].Controls["FunForm"].Enabled = false;
            }
        }
    }
}
