﻿namespace CarRental
{
    partial class loginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(loginForm));
            this.loginField = new System.Windows.Forms.TextBox();
            this.pwdField = new System.Windows.Forms.TextBox();
            this.sendBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel = new System.Windows.Forms.Panel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.captchaImage = new System.Windows.Forms.PictureBox();
            this.inputcaptcha = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.updatecaptch = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.captchaImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // loginField
            // 
            this.loginField.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.loginField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.loginField.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.loginField.ForeColor = System.Drawing.Color.White;
            this.loginField.HideSelection = false;
            this.loginField.Location = new System.Drawing.Point(78, 220);
            this.loginField.Margin = new System.Windows.Forms.Padding(2);
            this.loginField.Name = "loginField";
            this.loginField.Size = new System.Drawing.Size(343, 37);
            this.loginField.TabIndex = 2;
            this.loginField.TabStop = false;
            this.loginField.Text = "Login";
            this.loginField.Click += new System.EventHandler(this.loginField_Click);
            this.loginField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.loginField_KeyPress);
            // 
            // pwdField
            // 
            this.pwdField.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.pwdField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pwdField.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pwdField.ForeColor = System.Drawing.Color.White;
            this.pwdField.HideSelection = false;
            this.pwdField.Location = new System.Drawing.Point(78, 319);
            this.pwdField.Margin = new System.Windows.Forms.Padding(2);
            this.pwdField.Name = "pwdField";
            this.pwdField.Size = new System.Drawing.Size(312, 37);
            this.pwdField.TabIndex = 4;
            this.pwdField.TabStop = false;
            this.pwdField.Text = "Password";
            this.pwdField.Click += new System.EventHandler(this.pwdField_Click);
            this.pwdField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.pwdField_KeyPress);
            // 
            // sendBtn
            // 
            this.sendBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(96)))), ((int)(((byte)(255)))));
            this.sendBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sendBtn.FlatAppearance.BorderSize = 0;
            this.sendBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sendBtn.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.sendBtn.ForeColor = System.Drawing.Color.White;
            this.sendBtn.Location = new System.Drawing.Point(28, 403);
            this.sendBtn.Margin = new System.Windows.Forms.Padding(2);
            this.sendBtn.Name = "sendBtn";
            this.sendBtn.Size = new System.Drawing.Size(393, 64);
            this.sendBtn.TabIndex = 5;
            this.sendBtn.Text = "Войти";
            this.sendBtn.UseVisualStyleBackColor = false;
            this.sendBtn.Click += new System.EventHandler(this.sendBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.closeBtn.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.closeBtn.ForeColor = System.Drawing.Color.White;
            this.closeBtn.Location = new System.Drawing.Point(28, 533);
            this.closeBtn.Margin = new System.Windows.Forms.Padding(2);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(393, 71);
            this.closeBtn.TabIndex = 6;
            this.closeBtn.Text = "Выход";
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.close_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(28, 361);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(393, 1);
            this.panel1.TabIndex = 10;
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.Color.White;
            this.panel.Location = new System.Drawing.Point(28, 262);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(393, 1);
            this.panel.TabIndex = 11;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImage = global::CarRental.Properties.Resources.show;
            this.pictureBox4.Location = new System.Drawing.Point(389, 319);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(32, 32);
            this.pictureBox4.TabIndex = 12;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox4_MouseDown);
            this.pictureBox4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox4_MouseUp);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImage = global::CarRental.Properties.Resources.padlock;
            this.pictureBox3.Location = new System.Drawing.Point(28, 319);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(32, 32);
            this.pictureBox3.TabIndex = 9;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::CarRental.Properties.Resources.avatar;
            this.pictureBox2.Location = new System.Drawing.Point(28, 220);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 32);
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::CarRental.Properties.Resources.car;
            this.pictureBox1.Location = new System.Drawing.Point(155, 33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 128);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // captchaImage
            // 
            this.captchaImage.Location = new System.Drawing.Point(453, 33);
            this.captchaImage.Name = "captchaImage";
            this.captchaImage.Size = new System.Drawing.Size(393, 224);
            this.captchaImage.TabIndex = 18;
            this.captchaImage.TabStop = false;
            // 
            // inputcaptcha
            // 
            this.inputcaptcha.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.inputcaptcha.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.inputcaptcha.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.inputcaptcha.ForeColor = System.Drawing.Color.White;
            this.inputcaptcha.Location = new System.Drawing.Point(452, 322);
            this.inputcaptcha.MaxLength = 20;
            this.inputcaptcha.Name = "inputcaptcha";
            this.inputcaptcha.Size = new System.Drawing.Size(393, 37);
            this.inputcaptcha.TabIndex = 19;
            this.inputcaptcha.Click += new System.EventHandler(this.inputcaptcha_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(96)))), ((int)(((byte)(255)))));
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(452, 533);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(394, 71);
            this.button1.TabIndex = 21;
            this.button1.Text = "Проверить";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // updatecaptch
            // 
            this.updatecaptch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.updatecaptch.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.updatecaptch.ForeColor = System.Drawing.Color.White;
            this.updatecaptch.Location = new System.Drawing.Point(453, 403);
            this.updatecaptch.Margin = new System.Windows.Forms.Padding(2);
            this.updatecaptch.Name = "updatecaptch";
            this.updatecaptch.Size = new System.Drawing.Size(393, 71);
            this.updatecaptch.TabIndex = 22;
            this.updatecaptch.Text = "Обновить";
            this.updatecaptch.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Location = new System.Drawing.Point(453, 361);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(393, 1);
            this.panel2.TabIndex = 11;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::CarRental.Properties.Resources.icons8_settings_50;
            this.pictureBox5.Location = new System.Drawing.Point(28, 33);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(50, 50);
            this.pictureBox5.TabIndex = 23;
            this.pictureBox5.TabStop = false;
            this.pictureBox5.Click += new System.EventHandler(this.pictureBox5_Click);
            // 
            // loginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.ClientSize = new System.Drawing.Size(857, 643);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.updatecaptch);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.inputcaptcha);
            this.Controls.Add(this.captchaImage);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.sendBtn);
            this.Controls.Add(this.pwdField);
            this.Controls.Add(this.loginField);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "loginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CarRental";
            this.Load += new System.EventHandler(this.loginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.captchaImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox loginField;
        private System.Windows.Forms.TextBox pwdField;
        private System.Windows.Forms.Button sendBtn;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox captchaImage;
        private System.Windows.Forms.TextBox inputcaptcha;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button updatecaptch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox5;
    }
}

