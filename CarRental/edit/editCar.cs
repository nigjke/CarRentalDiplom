using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CarRental
{
    public partial class editCar : Form
    {
        private db db;
        private DataGridViewRow selectedRow;
        private bool _isImageUploaded = false;
        private int car_id;
        private byte[] _imageBytes;
        private const int MaxImageSizeMB = 25;
        public editCar(DataGridViewRow row)
        {
            InitializeComponent();
            db = new db();
            selectedRow = row;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                textBox1.Text = GetCellValue("Марка");
                textBox2.Text = GetCellValue("Модель");
                textBox3.Text = GetCellValue("Год выпуска");
                maskedTextBox1.Text = GetCellValue("Гос.Номер");
                comboBox1.SelectedItem = GetCellValue("Статус");
                textBox4.Text = GetCellValue("Цена за сутки");
                car_id = Convert.ToInt32(selectedRow.Cells["car_id"].Value);

                LoadExistingImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
                Close();
            }
        }

        private void LoadExistingImage()
        {
            try
            {
                _imageBytes = helper.GetCarPhoto(car_id);

                if (_imageBytes != null && _imageBytes.Length > 0)
                {
                    using (var ms = new MemoryStream(_imageBytes))
                    {
                        var tempImage = Image.FromStream(ms);
                        pictureBox1.Image?.Dispose();
                        pictureBox1.Image = new Bitmap(tempImage);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки фото: {ex.Message}");
                SetDefaultImage();
            }
        }
        private string GetCellValue(string columnName)
        {
            return selectedRow.Cells[columnName].Value?.ToString() ?? string.Empty;
        }
        private void UpdateCar()
        {
            if (!ValidateInputs()) return;

            using (var connection = new MySqlConnection(db.connect))
            {
                connection.Open();

                var query = _isImageUploaded
                    ? @"UPDATE cars SET 
                        make = @make, 
                        model = @model, 
                        year = @year, 
                        license_plate = @license_plate, 
                        price = @price, 
                        status = @status, 
                        photo = @photo 
                      WHERE car_id = @carId"
                    : @"UPDATE cars SET 
                        make = @make, 
                        model = @model, 
                        year = @year, 
                        license_plate = @license_plate, 
                        price = @price, 
                        status = @status 
                      WHERE car_id = @carId";

                using (var command = new MySqlCommand(query, connection))
                {
                    AddCommonParameters(command);
                    if (_isImageUploaded)
                    {
                        command.Parameters.Add("@photo", MySqlDbType.LongBlob).Value = _imageBytes;
                    }

                    command.ExecuteNonQuery();
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        private void AddCommonParameters(MySqlCommand command)
        {
            command.Parameters.AddWithValue("@make", textBox1.Text.Trim());
            command.Parameters.AddWithValue("@model", textBox2.Text.Trim());
            command.Parameters.AddWithValue("@year", int.Parse(textBox3.Text));
            command.Parameters.AddWithValue("@license_plate", maskedTextBox1.Text);
            command.Parameters.AddWithValue("@price", decimal.Parse(textBox4.Text));
            command.Parameters.AddWithValue("@status", comboBox1.SelectedItem);
            command.Parameters.AddWithValue("@carId", car_id);
        }
        private void ValidateFileSize(FileInfo fileInfo)
        {
            if (fileInfo.Length > MaxImageSizeMB * 1024 * 1024)
            {
                throw new Exception($"Файл слишком большой! Максимум {MaxImageSizeMB} МБ.");
            }
        }
        private void ProcessImageUpload(Stream imageStream)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    imageStream.CopyTo(memoryStream);
                    memoryStream.Position = 0;

                    using (var originalImage = Image.FromStream(memoryStream))
                    using (var compressedImage = CompressImage(originalImage))
                    {
                        _imageBytes = ImageToByteArray(compressedImage);

                        var newImage = new Bitmap(compressedImage);
                        pictureBox1.Image?.Dispose();
                        pictureBox1.Image = newImage;
                    }
                }
                _isImageUploaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обработки изображения: {ex.Message}");
                _isImageUploaded = false;
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
            var encoder = ImageCodecInfo.GetImageEncoders()
                .First(c => c.FormatID == ImageFormat.Jpeg.Guid);

            var encoderParams = new EncoderParameters(1)
            {
                Param = { [0] = new EncoderParameter(Encoder.Quality, 70L) }
            };
            var resultStream = new MemoryStream();
            image.Save(resultStream, encoder, encoderParams);
            resultStream.Position = 0;

            return Image.FromStream(resultStream);
        }

        private void SetDefaultImage()
        {
            try
            {
                pictureBox1.Image?.Dispose();
            }
            catch
            {
                pictureBox1.Image = null;
            }
        }

        private bool ValidateInputs()
        {
            if (!ValidateTextBoxes() || !ValidateNumbers()) return false;
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус автомобиля");
                return false;
            }
            return true;
        }

        private bool ValidateTextBoxes()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                !maskedTextBox1.MaskCompleted ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Заполните все обязательные поля");
                return false;
            }
            return true;
        }

        private bool ValidateNumbers()
        {
            if (!int.TryParse(textBox3.Text, out _))
            {
                MessageBox.Show("Некорректный год выпуска");
                return false;
            }

            if (!decimal.TryParse(textBox4.Text, out _))
            {
                MessageBox.Show("Некорректная цена");
                return false;
            }
            return true;
        }

        private void UploadImage()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                if (openFileDialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    var fileInfo = new FileInfo(openFileDialog.FileName);
                    ValidateFileSize(fileInfo);

                    using (var imageStream = fileInfo.OpenRead())
                    {
                        ProcessImageUpload(imageStream);
                    }

                    _isImageUploaded = true;
                    MessageBox.Show("Изображение успешно загружено");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            UpdateCar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void uploadBtn_Click(object sender, EventArgs e)
        {
            UploadImage();
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
    }
}
