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
    public partial class FormOutput : Form
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

        public FormOutput()
        {
            InitializeComponent();
        }

        public FormOutput(FormMain formMain)
        {
            InitializeComponent();
            _formMain = formMain;
        }

        private void buttonOutput_Click(object sender, EventArgs e)
        {
            if (buttonOutput.Text == "开始导出")
            {
                buttonOutput.Enabled = buttonNotOutput.Enabled = false;
                buttonOutput.Text = "返回";
                labelState.ForeColor = Color.Red;
                labelState.Text = "导出数据中！请稍候...";
                if (_formMain.WriteData() == 1)
                {
                    labelState.ForeColor = Color.Green;
                    labelState.Text = "导出完成！";
                    buttonOutput.Enabled = true;
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
                _formMain.DisenableAll(false);
                _formMain.Enabled = true;
                this.Close();
            }
        }

        private void buttonNotOutput_Click(object sender, EventArgs e)
        {
            _formMain.SetButtonOutput(true);
            _formMain.Enabled = true;
            this.Close();
        }
    }
}
