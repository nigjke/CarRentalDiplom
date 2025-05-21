namespace CarRental.fullInfoCar
{
    partial class fullInfoCar
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
            this.photo = new System.Windows.Forms.PictureBox();
            this.closeBtn = new System.Windows.Forms.Button();
            this.carYear = new System.Windows.Forms.Label();
            this.carNumber = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.carStatus = new System.Windows.Forms.Label();
            this.carName = new System.Windows.Forms.Label();
            this.carPrice = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.photo)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // photo
            // 
            this.photo.Image = global::CarRental.Properties.Resources._1637827423_1_flomaster_club_p_mashina_risunok_karandashom_lyogkie_detski_1;
            this.photo.Location = new System.Drawing.Point(68, 28);
            this.photo.Name = "photo";
            this.photo.Size = new System.Drawing.Size(426, 255);
            this.photo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.photo.TabIndex = 0;
            this.photo.TabStop = false;
            // 
            // closeBtn
            // 
            this.closeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(96)))), ((int)(((byte)(255)))));
            this.closeBtn.FlatAppearance.BorderSize = 0;
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.closeBtn.ForeColor = System.Drawing.Color.White;
            this.closeBtn.Location = new System.Drawing.Point(68, 561);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(426, 72);
            this.closeBtn.TabIndex = 15;
            this.closeBtn.Text = "Назад";
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // carYear
            // 
            this.carYear.AutoSize = true;
            this.carYear.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.carYear.ForeColor = System.Drawing.Color.White;
            this.carYear.Location = new System.Drawing.Point(63, 372);
            this.carYear.Name = "carYear";
            this.carYear.Size = new System.Drawing.Size(57, 32);
            this.carYear.TabIndex = 18;
            this.carYear.Text = "Год";
            // 
            // carNumber
            // 
            this.carNumber.AutoSize = true;
            this.carNumber.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.carNumber.ForeColor = System.Drawing.Color.White;
            this.carNumber.Location = new System.Drawing.Point(62, 456);
            this.carNumber.Name = "carNumber";
            this.carNumber.Size = new System.Drawing.Size(151, 32);
            this.carNumber.TabIndex = 19;
            this.carNumber.Text = "Гос. номер";
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.carStatus);
            this.panel1.Controls.Add(this.carName);
            this.panel1.Controls.Add(this.carPrice);
            this.panel1.Controls.Add(this.photo);
            this.panel1.Controls.Add(this.closeBtn);
            this.panel1.Controls.Add(this.carNumber);
            this.panel1.Controls.Add(this.carYear);
            this.panel1.Location = new System.Drawing.Point(35, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(564, 661);
            this.panel1.TabIndex = 20;
            // 
            // carStatus
            // 
            this.carStatus.AutoSize = true;
            this.carStatus.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.carStatus.ForeColor = System.Drawing.Color.White;
            this.carStatus.Location = new System.Drawing.Point(62, 414);
            this.carStatus.Name = "carStatus";
            this.carStatus.Size = new System.Drawing.Size(100, 32);
            this.carStatus.TabIndex = 22;
            this.carStatus.Text = "Статус";
            // 
            // carName
            // 
            this.carName.AutoSize = true;
            this.carName.Font = new System.Drawing.Font("Arial", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.carName.ForeColor = System.Drawing.Color.White;
            this.carName.Location = new System.Drawing.Point(62, 320);
            this.carName.Name = "carName";
            this.carName.Size = new System.Drawing.Size(160, 42);
            this.carName.TabIndex = 21;
            this.carName.Text = "Машина";
            // 
            // carPrice
            // 
            this.carPrice.AutoSize = true;
            this.carPrice.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.carPrice.ForeColor = System.Drawing.Color.White;
            this.carPrice.Location = new System.Drawing.Point(63, 498);
            this.carPrice.Name = "carPrice";
            this.carPrice.Size = new System.Drawing.Size(79, 32);
            this.carPrice.TabIndex = 20;
            this.carPrice.Text = "Цена";
            // 
            // fullInfoCar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(633, 721);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "fullInfoCar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fullInfoCar";
            ((System.ComponentModel.ISupportInitialize)(this.photo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox photo;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Label carYear;
        private System.Windows.Forms.Label carNumber;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label carName;
        private System.Windows.Forms.Label carPrice;
        private System.Windows.Forms.Label carStatus;
    }
}