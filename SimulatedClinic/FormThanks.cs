using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimulatedClinic
{
    public partial class FormThanks : Form
    {
        /*      对象：字段      */

        FormMain _formMain;

        /*      对象：辅助方法      */

        //禁用窗体的关闭按钮
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | 0x200;
                return myCp;
            }
        }

        //取消程序对键盘上一些按键的响应
        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Left:
                case Keys.Down:
                case Keys.Right:
                case Keys.Tab:
                case Keys.Control:
                case Keys.Alt:
                    return true;
                default:
                    return base.ProcessDialogKey(keyData);
            }
        }

        /*      对象：构造方法和事件      */

        public FormThanks()
        {
            InitializeComponent();
        }

        public FormThanks(FormMain formMain)
        {
            InitializeComponent();
            _formMain = formMain;
            _formMain.DisenableAll(true);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore.exe", "https://github.com/tonyqus/npoi");
        }
    }
}
