namespace CarRental.fullInfo
{
    partial class fullInfoCarRental
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fullInfoCarRental));
            this.panel1 = new System.Windows.Forms.Panel();
            this.closeBtn = new System.Windows.Forms.Button();
            this.carStatus = new System.Windows.Forms.Label();
            this.carName = new System.Windows.Forms.Label();
            this.carPrice = new System.Windows.Forms.Label();
            this.photo = new System.Windows.Forms.PictureBox();
            this.addBtn = new System.Windows.Forms.Button();
            this.carNumber = new System.Windows.Forms.Label();
            this.carYear = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.photo)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.closeBtn);
            this.panel1.Controls.Add(this.carStatus);
            this.panel1.Controls.Add(this.carName);
            this.panel1.Controls.Add(this.carPrice);
            this.panel1.Controls.Add(this.photo);
            this.panel1.Controls.Add(this.addBtn);
            this.panel1.Controls.Add(this.carNumber);
            this.panel1.Controls.Add(this.carYear);
            this.panel1.Location = new System.Drawing.Point(34, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(564, 741);
            this.panel1.TabIndex = 21;
            // 
            // closeBtn
            // 
            this.closeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.closeBtn.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.closeBtn.ForeColor = System.Drawing.Color.White;
            this.closeBtn.Location = new System.Drawing.Point(68, 651);
            this.closeBtn.Margin = new System.Windows.Forms.Padding(2);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(426, 71);
            this.closeBtn.TabIndex = 23;
            this.closeBtn.Text = "Выход";
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
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
            // addBtn
            // 
            this.addBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(96)))), ((int)(((byte)(255)))));
            this.addBtn.FlatAppearance.BorderSize = 0;
            this.addBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addBtn.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.addBtn.ForeColor = System.Drawing.Color.White;
            this.addBtn.Location = new System.Drawing.Point(68, 562);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(426, 72);
            this.addBtn.TabIndex = 15;
            this.addBtn.Text = "Арендовать";
            this.addBtn.UseVisualStyleBackColor = false;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
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
            // fullInfoCarRental
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(633, 783);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fullInfoCarRental";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fullInfoCarRental";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.photo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label carStatus;
        private System.Windows.Forms.Label carName;
        private System.Windows.Forms.Label carPrice;
        private System.Windows.Forms.PictureBox photo;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Label carNumber;
        private System.Windows.Forms.Label carYear;
        private System.Windows.Forms.Button closeBtn;
    }
}