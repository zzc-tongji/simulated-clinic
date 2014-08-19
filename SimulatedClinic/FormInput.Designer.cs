namespace SimulatedClinic
{
    partial class FormInput
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param _name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInput));
            this.labelNote = new System.Windows.Forms.Label();
            this.buttonInput = new System.Windows.Forms.Button();
            this.buttonNotInput = new System.Windows.Forms.Button();
            this.labelState = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // labelNote
            // 
            this.labelNote.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelNote.Location = new System.Drawing.Point(12, 14);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(360, 75);
            this.labelNote.TabIndex = 1;
            this.labelNote.Text = "请确保下列文件与本程序位于同一个目录下：\r\n\r\n    PatientInfo.xls\r\n    ClinicInfo.xls";
            // 
            // buttonInput
            // 
            this.buttonInput.Location = new System.Drawing.Point(12, 154);
            this.buttonInput.Name = "buttonInput";
            this.buttonInput.Size = new System.Drawing.Size(170, 50);
            this.buttonInput.TabIndex = 2;
            this.buttonInput.Text = "开始导入";
            this.buttonInput.UseVisualStyleBackColor = true;
            this.buttonInput.Click += new System.EventHandler(this.buttonInput_Click);
            // 
            // buttonNotInput
            // 
            this.buttonNotInput.Location = new System.Drawing.Point(202, 154);
            this.buttonNotInput.Name = "buttonNotInput";
            this.buttonNotInput.Size = new System.Drawing.Size(170, 50);
            this.buttonNotInput.TabIndex = 3;
            this.buttonNotInput.Text = "不要导入";
            this.buttonNotInput.UseVisualStyleBackColor = true;
            this.buttonNotInput.Click += new System.EventHandler(this.buttonNotInput_Click);
            // 
            // labelState
            // 
            this.labelState.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelState.ForeColor = System.Drawing.Color.Green;
            this.labelState.Location = new System.Drawing.Point(12, 89);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(360, 26);
            this.labelState.TabIndex = 4;
            this.labelState.Text = "准备就绪";
            this.labelState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 114);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(360, 23);
            this.progressBar.TabIndex = 5;
            // 
            // FormInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 222);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.buttonNotInput);
            this.Controls.Add(this.buttonInput);
            this.Controls.Add(this.labelNote);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FormInput";
            this.Text = "模拟诊所 - 导入数据";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelNote;
        private System.Windows.Forms.Button buttonInput;
        private System.Windows.Forms.Button buttonNotInput;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}

