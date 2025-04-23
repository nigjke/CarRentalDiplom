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
        public fullInfoCar(int carId)
        {
            InitializeComponent();
            this.carId = carId;
            LoadCarDetails();
        }
        private void LoadCarDetails()
        {
            using (var con = new MySqlConnection(db.connect))
            {
                con.Open();  
                string query = "SELECT car_id, make as 'Марка', model as 'Модель', year as 'Год выпуска', license_plate as 'Гос.Номер', status as 'Статус', price as 'Цена за сутки', photo FROM cars WHERE car_id = @id";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", carId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string make = reader.GetString("Марка");
                        string model = reader.GetString("Модель");

                        carName.Text = $"{make} {model}";
                        carYear.Text = $"Год: {reader.GetInt32("Год выпуска")} км";
                        carStatus.Text = $"Статус: {reader.GetString("Статус")}";
                        carPrice.Text = $"Цена в день: {reader.GetDecimal("Цена за сутки"):N0} ₽";

                        //if (!reader.IsDBNull(reader.GetOrdinal("photo")))
                        //{
                        //    byte[] imgBytes = (byte[])reader["photo"];
                        //    using (var ms = new MemoryStream(imgBytes))
                        //    {
                        //        photo.Image = Image.FromStream(ms);
                        //    }
                        //}
                    }
                }
            }
        }
        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
