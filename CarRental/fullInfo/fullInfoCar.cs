using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CarRental;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;

namespace CarRental.fullInfoCar
{
    public partial class fullInfoCar: Form
    {
        private int carId;
        private bool _isClosing;
        public fullInfoCar(int carId)
        {
            InitializeComponent();
            this.carId = carId;
            this.FormClosing += (s, e) => _isClosing = true;
            LoadCarDetailsAsync();
        }
        private async void LoadCarDetailsAsync()
        {
            try
            {
                using (var con = new MySqlConnection(db.connect))
                {
                    await con.OpenAsync();

                    const string query = @"SELECT 
                        make,
                        model,
                        year,
                        status,
                        price,
                        photo 
                    FROM cars 
                    WHERE car_id = @id";

                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", carId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync() && !_isClosing)
                            {
                                if (_isClosing) return;
                                this.Invoke((MethodInvoker)delegate
                                {
                                    if (_isClosing) return;
                                    carName.Text = $"{reader["make"]} {reader["model"]}";
                                    carYear.Text = $"Год выпуска: {reader["year"]}";
                                    carStatus.Text = $"Статус: {reader["status"]}";
                                    carPrice.Text = $"Цена за сутки: {Convert.ToDecimal(reader["price"]):N0} ₽";
                                });

                                this.Invoke((MethodInvoker)delegate
                                {
                                    if (_isClosing) return;
                                    if (!reader.IsDBNull(5))
                                    {
                                        LoadImageFromDatabase((byte[])reader["photo"]);
                                    }
                                    else
                                    {
                                        SetDefaultImage();
                                    }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!_isClosing)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
                        this.Close();
                    });
                }
            }
        }

        private void LoadImageFromDatabase(byte[] imgBytes)
        {
            try
            {
                using (var ms = new MemoryStream(imgBytes))
                {
                    var tempImage = Image.FromStream(ms);
                    photo.Image?.Dispose();
                    photo.Image = new Bitmap(tempImage);
                }
            }
            catch (Exception ex)
            {
                SetDefaultImage();
            }
        }

        private void SetDefaultImage()
        {
            try
            {
                photo.Image?.Dispose();
                photo.Image = Properties.Resources._1637827423_1_flomaster_club_p_mashina_risunok_karandashom_lyogkie_detski_1;
            }
            catch
            {
                photo.Image = null;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            photo.Image?.Dispose();
            _isClosing = true;
        }
        private void closeBtn_Click(object sender, EventArgs e)
        {
            _isClosing = true;
            this.Close();
        }
    }
}
