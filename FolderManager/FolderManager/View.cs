using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FolderManager
{
    public class View
    {
        public GroupBox groupBox;
        public TableLayoutPanel panel1;
        public TableLayoutPanel panel2;

        public View()
        {
            groupBox = new GroupBox();
            panel1 = new TableLayoutPanel();
            panel2 = new TableLayoutPanel() { Top = 24, Left = 5 }; ;
        }

        public void InitPanel1()
        {
            panel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            panel1.RowCount = 2;
            panel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            panel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel1.AutoSize = true;
            panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        }

        public void InitPanel2()
        {
            panel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            panel2.AutoSize = true;
            panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        }

        public void InitGroupBox()
        {
            groupBox.AutoSize = true;
            groupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            groupBox.Size = new System.Drawing.Size(100,100);
        }
    }
}
