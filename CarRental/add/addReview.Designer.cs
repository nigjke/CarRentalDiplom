namespace CarRental.add
{
    partial class addReview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(addReview));
            this.comboBoxRating = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxComment = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // comboBoxRating
            // 
            this.comboBoxRating.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRating.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxRating.FormattingEnabled = true;
            this.comboBoxRating.Location = new System.Drawing.Point(226, 33);
            this.comboBoxRating.Name = "comboBoxRating";
            this.comboBoxRating.Size = new System.Drawing.Size(242, 32);
            this.comboBoxRating.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(21, 466);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(451, 59);
            this.button2.TabIndex = 4;
            this.button2.Text = "Назад";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(96)))), ((int)(((byte)(255)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(21, 391);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(451, 59);
            this.button1.TabIndex = 3;
            this.button1.Text = "Добавить";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(15, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 32);
            this.label1.TabIndex = 186;
            this.label1.Text = "Рейтинг";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(15, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 32);
            this.label2.TabIndex = 187;
            this.label2.Text = "Описание";
            // 
            // textBoxComment
            // 
            this.textBoxComment.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxComment.Location = new System.Drawing.Point(226, 108);
            this.textBoxComment.Name = "textBoxComment";
            this.textBoxComment.Size = new System.Drawing.Size(242, 255);
            this.textBoxComment.TabIndex = 2;
            this.textBoxComment.Text = "";
            this.textBoxComment.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxComment_KeyPress);
            // 
            // addReview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(491, 545);
            this.Controls.Add(this.textBoxComment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxRating);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "addReview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "addReview";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxRating;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox textBoxComment;
    }
}