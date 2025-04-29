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
        private byte[] imageBytes;
        public addCar()
        {
            db = new db();
            InitializeComponent();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != ""
                    && maskedTextBox1.Text != "" && textBox4.Text != "")
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(connect))
                    {
                        con.Open();

                        string query = @"INSERT INTO cars 
                    (make, model, year, license_plate, status, price, photo) 
                    VALUES 
                    (@make, @model, @year, @license_plate, @status, @price, @photo)";

                        MySqlCommand cmd = new MySqlCommand(query, con);

                        // Преобразование данных
                        int year = Convert.ToInt32(textBox3.Text);
                        decimal price = Convert.ToDecimal(textBox4.Text);
                        string status = comboBox1.SelectedItem.ToString();

                        // Добавление параметров
                        cmd.Parameters.AddWithValue("@make", textBox1.Text);
                        cmd.Parameters.AddWithValue("@model", textBox2.Text);
                        cmd.Parameters.AddWithValue("@year", year);
                        cmd.Parameters.AddWithValue("@license_plate", maskedTextBox1.Text);
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.Add("@photo", MySqlDbType.Blob).Value = imageBytes ?? (object)DBNull.Value;

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Машина добавлена");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
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

        private void uploadBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                        if (fileInfo.Length > 25 * 1024 * 1024)
                        {
                            MessageBox.Show("Файл слишком большой! Максимум 25 МБ.");
                            return;
                        }
                        try
                        {
                            pictureBox1.Image = new System.Drawing.Bitmap(openFileDialog.FileName);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                                imageBytes = ms.ToArray();
                            }
                            MessageBox.Show("Изображение успешно загружено!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка: {ex.Message}");
                        }
                    }
                }
            }    
        }
        private Image CompressImage(Image image)
        {
            ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageEncoders().First(x => x.FormatID == ImageFormat.Jpeg.Guid);
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 70L);

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, jpegCodec, encoderParams);
                return Image.FromStream(ms);
            }
        }
    }
}
