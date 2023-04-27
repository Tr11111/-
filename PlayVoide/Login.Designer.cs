namespace PlayVideo
{
    partial class Login
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
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.IPtext = new System.Windows.Forms.TextBox();
            this.Porttext = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.Location = new System.Drawing.Point(221, 206);
            this.textBox2.Name = "textBox2";
            this.textBox2.PasswordChar = '·';
            this.textBox2.Size = new System.Drawing.Size(325, 44);
            this.textBox2.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(221, 321);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(221, 59);
            this.button1.TabIndex = 1;
            this.button1.Text = "登录";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel1.LinkColor = System.Drawing.Color.Blue;
            this.linkLabel1.Location = new System.Drawing.Point(460, 421);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(209, 20);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "还没有账号？点击注册";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            this.linkLabel1.MouseEnter += new System.EventHandler(this.linkLabel1_MouseEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(114, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 31);
            this.label1.TabIndex = 3;
            this.label1.Text = "用户名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(138, 211);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 31);
            this.label2.TabIndex = 3;
            this.label2.Text = "密码";
            // 
            // linkLabel2
            // 
            this.linkLabel2.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.BackColor = System.Drawing.Color.White;
            this.linkLabel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel2.LinkColor = System.Drawing.Color.Black;
            this.linkLabel2.Location = new System.Drawing.Point(496, 212);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(41, 31);
            this.linkLabel2.TabIndex = 10;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "👁️‍🗨️";
            this.linkLabel2.VisitedLinkColor = System.Drawing.Color.Silver;
            this.linkLabel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.linkLabel2_MouseDown);
            this.linkLabel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.linkLabel2_MouseUp);
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel3.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel3.LinkColor = System.Drawing.Color.Blue;
            this.linkLabel3.Location = new System.Drawing.Point(580, 388);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(89, 20);
            this.linkLabel3.TabIndex = 2;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "暂不登录";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            this.linkLabel3.MouseEnter += new System.EventHandler(this.linkLabel1_MouseEnter);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox1.Location = new System.Drawing.Point(221, 275);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(108, 28);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "记住密码";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(221, 119);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(325, 44);
            this.comboBox1.TabIndex = 13;
            this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedValueChanged);
            // 
            // linkLabel4
            // 
            this.linkLabel4.ActiveLinkColor = System.Drawing.SystemColors.ControlText;
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel4.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel4.LinkColor = System.Drawing.Color.Black;
            this.linkLabel4.Location = new System.Drawing.Point(552, 125);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(32, 31);
            this.linkLabel4.TabIndex = 14;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "×";
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
            // 
            // IPtext
            // 
            this.IPtext.Location = new System.Drawing.Point(-1, 420);
            this.IPtext.Name = "IPtext";
            this.IPtext.Size = new System.Drawing.Size(201, 28);
            this.IPtext.TabIndex = 15;
            // 
            // Porttext
            // 
            this.Porttext.Location = new System.Drawing.Point(231, 419);
            this.Porttext.Name = "Porttext";
            this.Porttext.Size = new System.Drawing.Size(79, 28);
            this.Porttext.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(206, 419);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 27);
            this.label3.TabIndex = 16;
            this.label3.Text = ":";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(318, 414);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(126, 34);
            this.button2.TabIndex = 17;
            this.button2.Text = "更换地址";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Login
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 450);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Porttext);
            this.Controls.Add(this.IPtext);
            this.Controls.Add(this.linkLabel4);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabel3);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Login_FormClosing);
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.LinkLabel linkLabel4;
        private System.Windows.Forms.TextBox IPtext;
        private System.Windows.Forms.TextBox Porttext;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
    }
}