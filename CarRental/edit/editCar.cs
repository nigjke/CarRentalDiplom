using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace CarRental
{
    public partial class editCar : Form
    {
        private db db;
        private DataGridViewRow selectedRow;
        private byte[] imageBytes;
        private bool isImageUploaded = false;
        public editCar(DataGridViewRow row)
        {
            db = new db();
            selectedRow = row;
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            textBox1.Text = selectedRow.Cells["Марка"].Value.ToString();
            textBox2.Text = selectedRow.Cells["Модель"].Value.ToString();
            textBox3.Text = selectedRow.Cells["Год выпуска"].Value.ToString();
            maskedTextBox1.Text = selectedRow.Cells["Гос.Номер"].Value.ToString();
            textBox4.Text = selectedRow.Cells["Цена за сутки"].Value.ToString();
            pictureBox1.Image = null;
            imageBytes = null;
        }

        private void UpdateDatabase(string make, string model, string year, string new_license_plate, string price, string old_license_plate, byte[] imageData)
        {
            using (MySqlConnection connection = new MySqlConnection(db.connect))
            {
                connection.Open();
                string query = isImageUploaded
     ? "UPDATE cars SET make=@make, model=@model, year=@year, " +
       "license_plate=@newLicensePlate, price=@price, photo=@image " +
       "WHERE license_plate=@oldLicensePlate"
     : "UPDATE cars SET make=@make, model=@model, year=@year, " +
       "license_plate=@newLicensePlate, price=@price " +
       "WHERE license_plate=@oldLicensePlate";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@make", make);
                command.Parameters.AddWithValue("@model", model);
                command.Parameters.AddWithValue("@year", year);
                command.Parameters.AddWithValue("@new_license_plate", new_license_plate);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@old_license_plate", old_license_plate);
                command.Parameters.Add("@image", MySqlDbType.Blob).Value = imageData;
                if (isImageUploaded)
                {
                    command.Parameters.Add("@image", MySqlDbType.LongBlob).Value = imageData;
                }

                command.ExecuteNonQuery();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && maskedTextBox1.Text != "" && textBox4.Text != "")
            {
                string old_license_plate = selectedRow.Cells["Гос.Номер"].Value.ToString();
                UpdateDatabase(textBox1.Text, textBox2.Text, textBox3.Text, maskedTextBox1.Text, textBox4.Text, old_license_plate, imageBytes);

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectEng(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectNum(e.KeyChar) && !char.IsControl(e.KeyChar))
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
                    try
                    {
                        FileInfo fileInfo = new FileInfo(openFileDialog.FileName);

                        if (fileInfo.Length > 25 * 1024 * 1024)
                        {
                            MessageBox.Show("Файл слишком большой! Максимум 25 МБ.");
                            return;
                        }

                        using (Image originalImage = Image.FromFile(openFileDialog.FileName))
                        {
                            Image compressedImage = CompressImage(originalImage, 70L);
                            pictureBox1.Image = compressedImage;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                compressedImage.Save(ms, ImageFormat.Jpeg); 
                                imageBytes = ms.ToArray();
                            }
                        }

                        isImageUploaded = true;
                        MessageBox.Show("Изображение успешно загружено!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
                    }
            }
        }
        private Image CompressImage(Image image, long quality)
        {
            ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, jpegCodec, encoderParams);
                return Image.FromStream(ms);
            }
        }

        private ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
