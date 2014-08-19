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
    public partial class FormInput : Form
    {
        /*      对象：字段      */

        FormMain _formMain;

        /*      对象：辅助方法      */

        //设定“进度条”的最大值
        public void SetProcessBarMaximum(Int32 value)
        {
            progressBar.Maximum = value;
        }

        //设定“进度条”的当前值
        public void SetProcessBarValue(Int32 value)
        {
            progressBar.Value = value;
        }

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

        public FormInput()
        {
            InitializeComponent();
        }

        public FormInput(FormMain formMain)
        {
            InitializeComponent();
            _formMain = formMain;
        }

        private void buttonInput_Click(object sender, EventArgs e)
        {
            if (buttonInput.Text == "开始导入")
            {
                buttonInput.Enabled = buttonNotInput.Enabled = false;
                buttonInput.Text = "返回";
                labelState.ForeColor = Color.Red;
                labelState.Text = "导入数据中！请稍候...";
                if (_formMain.ReadData() == 1)
                {
                    labelState.ForeColor = Color.Green;
                    labelState.Text = "导入完成！";
                    buttonInput.Enabled = true;
                    System.Media.SystemSounds.Beep.Play();
                }
                else
                {
                    this.Close();
                    Application.Exit();
                }
            }
            else
            {
                _formMain.SetButtonStart(true);
                _formMain.Enabled = true;
                this.Close();
            }
        }

        private void buttonNotInput_Click(object sender, EventArgs e)
        {
            _formMain.SetButtonStart(false);
            _formMain.SetButtonInput(true);
            _formMain.Enabled = true;
            this.Close();
        }
    }
}
