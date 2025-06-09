using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CarRental
{
    public partial class addCar : Form
    {
        private db db;
        string connect = db.connect;
        private byte[] _imageBytes;
        private const int MaxImageSizeMB = 25;
        bool isUpload = false;
        public addCar()
        {
            db = new db();
            InitializeComponent();
            textBox4.MaxLength = 5;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;
            try
                {
                    AddCarToDatabase();
                    ClearForm();
                    MessageBox.Show("Машина добавлена");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
        }
        private void AddCarToDatabase()
        {
            using (var con = new MySqlConnection(db.connect))
            {
                con.Open();

                const string query = @"INSERT INTO cars 
                    (make, model, year, license_plate, status, price, photo) 
                    VALUES 
                    (@make, @model, @year, @license_plate, @status, @price, @photo)";

                using (var cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@make", textBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@model", textBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@year", int.Parse(textBox3.Text));
                    cmd.Parameters.AddWithValue("@license_plate", maskedTextBox1.Text);
                    cmd.Parameters.AddWithValue("@status", comboBox1.SelectedItem);
                    cmd.Parameters.AddWithValue("@price", decimal.Parse(textBox4.Text));
                    cmd.Parameters.Add("@photo", MySqlDbType.Blob).Value = _imageBytes;

                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void ClearForm()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            maskedTextBox1.Clear();
            textBox4.Clear();
            pictureBox1.Image = null;
            _imageBytes = null;
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                textBox1.Text = char.ToUpper(textBox1.Text[0]) + textBox1.Text.Substring(1);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
                textBox2.Text = char.ToUpper(textBox2.Text[0]) + textBox2.Text.Substring(1);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectNum(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectEng(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectEng(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectNum(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;

            }
        }
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                !maskedTextBox1.MaskCompleted ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                isUpload == false
                )
            {
                MessageBox.Show("Заполните все поля корректно");
                return false;
            }
            return true;
        }
        private void uploadBtn_Click(object sender, EventArgs e)
        {

            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                if (openFileDialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    var fileInfo = new FileInfo(openFileDialog.FileName);
                    if (fileInfo.Length > MaxImageSizeMB * 1024 * 1024)
                    {
                        MessageBox.Show($"Файл слишком большой! Максимум {MaxImageSizeMB} МБ.");
                        return;
                    }
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();
                        pictureBox1.Image = null;
                    }
                    byte[] fileBytes = File.ReadAllBytes(openFileDialog.FileName);
                    using (var ms = new MemoryStream(fileBytes))
                    {
                        var originalImage = Image.FromStream(ms);
                        var compressedImage = CompressImage(originalImage);
                        _imageBytes = ImageToByteArray(compressedImage);
                        pictureBox1.Image = compressedImage;
                    }

                    MessageBox.Show("Изображение загружено");
                    isUpload = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }
        private static byte[] ImageToByteArray(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private static Image CompressImage(Image image)
        {
            var jpegCodec = ImageCodecInfo.GetImageEncoders()
                .First(x => x.FormatID == ImageFormat.Jpeg.Guid);

            var encoderParams = new EncoderParameters(1)
            {
                Param = { [0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 70L) }
            };

            using (var ms = new MemoryStream())
            {
                image.Save(ms, jpegCodec, encoderParams);
                return Image.FromStream(new MemoryStream(ms.ToArray()));
            }
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            if (int.TryParse(textBox3.Text, out int year))
            {
                if (year < 2000 || year > 2025)
                {
                    MessageBox.Show("Допустимые значения: 2000–2025");
                    e.Cancel = true; 
                }
            }
            else if (!string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Введите число!");
                e.Cancel = true;
            }
        }
    }
}
