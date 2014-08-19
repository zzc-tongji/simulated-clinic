namespace SimulatedClinic
{
    partial class FormOutput
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOutput));
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelState = new System.Windows.Forms.Label();
            this.buttonNotOutput = new System.Windows.Forms.Button();
            this.buttonOutput = new System.Windows.Forms.Button();
            this.labelNote = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 114);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(360, 23);
            this.progressBar.TabIndex = 10;
            // 
            // labelState
            // 
            this.labelState.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelState.ForeColor = System.Drawing.Color.Green;
            this.labelState.Location = new System.Drawing.Point(12, 89);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(360, 26);
            this.labelState.TabIndex = 9;
            this.labelState.Text = "准备就绪";
            this.labelState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonNotOutput
            // 
            this.buttonNotOutput.Location = new System.Drawing.Point(202, 154);
            this.buttonNotOutput.Name = "buttonNotOutput";
            this.buttonNotOutput.Size = new System.Drawing.Size(170, 50);
            this.buttonNotOutput.TabIndex = 8;
            this.buttonNotOutput.Text = "不要导出";
            this.buttonNotOutput.UseVisualStyleBackColor = true;
            this.buttonNotOutput.Click += new System.EventHandler(this.buttonNotOutput_Click);
            // 
            // buttonOutput
            // 
            this.buttonOutput.Location = new System.Drawing.Point(12, 154);
            this.buttonOutput.Name = "buttonOutput";
            this.buttonOutput.Size = new System.Drawing.Size(170, 50);
            this.buttonOutput.TabIndex = 7;
            this.buttonOutput.Text = "开始导出";
            this.buttonOutput.UseVisualStyleBackColor = true;
            this.buttonOutput.Click += new System.EventHandler(this.buttonOutput_Click);
            // 
            // labelNote
            // 
            this.labelNote.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelNote.Location = new System.Drawing.Point(12, 14);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(360, 75);
            this.labelNote.TabIndex = 6;
            this.labelNote.Text = "将更新桌面上的下列文件：\r\n\r\n    DoctorTreat.xls";
            // 
            // FormOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 222);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.buttonNotOutput);
            this.Controls.Add(this.buttonOutput);
            this.Controls.Add(this.labelNote);
            this.Font = new System.Drawing.Font("宋体", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FormOutput";
            this.Text = "模拟诊所 - 导出数据";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.Button buttonNotOutput;
        private System.Windows.Forms.Button buttonOutput;
        private System.Windows.Forms.Label labelNote;
    }
}