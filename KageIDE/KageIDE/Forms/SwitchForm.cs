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
    public partial class SwitchForm : Form
    {
        // 控制器核心
        private KagaController core = KagaController.getInstance();

        // 构造器
        public SwitchForm()
        {
            InitializeComponent();
        }

        // 窗体载入函数
        private void SwitchForm_Load(object sender, EventArgs e)
        {
            // 加载所有开关
            List<string> switchVector = core.getSwitchDescriptionVector();
            // 加载开关列表
            for (int i = 0; i < Consta.switch_max; i++)
            {
                this.switchDataGridView.Rows.Add();
                this.switchDataGridView.Rows[i].Cells[0].Value = i;
                this.switchDataGridView.Rows[i].Cells[1].Value = core.getSwitchDescriptionVector()[i];
            }
            // 选中默认开关值
            this.comboBox1.SelectedIndex = 0;
        }

        // 确定按钮
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.switchDataGridView.SelectedCells.Count < 1)
            {
                return;
            }
            core.dash_switchOperate(this.switchDataGridView.SelectedCells[0].RowIndex, this.comboBox1.SelectedIndex == 1);
            this.Close();
        }
    }
}
