using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.add
{
    public partial class addMaintenance : Form
    {
        public addMaintenance()
        {
            InitializeComponent();
            LoadForm();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadForm()
        {
            using (MySqlConnection con = new MySqlConnection(db.connect))
            {
                con.Open();
                string sqlTypes = "SELECT maintenance_type_id, name FROM maintenance_type";
                using (MySqlCommand cmd = new MySqlCommand(sqlTypes, con))
                using (var reader = cmd.ExecuteReader())
                {
                    Dictionary<int, string> types = new Dictionary<int, string>();
                    while (reader.Read())
                        comboBoxType.Items.Add(new ComboBoxItem(reader["name"].ToString(), Convert.ToInt32(reader["maintenance_type_id"])));
                }
            }
        }

        private void comboBoxMake_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            string make = comboBoxMake.SelectedItem?.ToString();
            string model = comboBoxModel.SelectedItem?.ToString();
            ComboBoxItem selectedType = comboBoxType.SelectedItem as ComboBoxItem;

            if (make == null || model == null || selectedType == null || string.IsNullOrWhiteSpace(textBoxCost.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            DateTime start = dateTimePickerStart.Value.Date;
            DateTime end = dateTimePickerEnd.Value.Date;
            if (end < start)
            {
                MessageBox.Show("Дата окончания не может быть раньше начала.");
                return;
            }

            decimal cost;
            if (!decimal.TryParse(textBoxCost.Text, out cost))
            {
                MessageBox.Show("Введите корректную стоимость.");
                return;
            }

            using (MySqlConnection con = new MySqlConnection(db.connect))
            {
                con.Open();
                string sqlFindCar = "SELECT car_id FROM cars WHERE make = @make AND model = @model";
                int carId;
                using (MySqlCommand cmd = new MySqlCommand(sqlFindCar, con))
                {
                    cmd.Parameters.AddWithValue("@make", make);
                    cmd.Parameters.AddWithValue("@model", model);
                    object result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        MessageBox.Show("Машина не найдена.");
                        return;
                    }
                    carId = Convert.ToInt32(result);
                }

                string sqlInsert = @"INSERT INTO maintenance (car_id, maintenance_type_id, maintenance_date, return_date, cost)
                             VALUES (@carId, @typeId, @start, @end, @cost)";
                using (MySqlCommand cmd = new MySqlCommand(sqlInsert, con))
                {
                    cmd.Parameters.AddWithValue("@carId", carId);
                    cmd.Parameters.AddWithValue("@typeId", selectedType.Value);
                    cmd.Parameters.AddWithValue("@start", start);
                    cmd.Parameters.AddWithValue("@end", end);
                    cmd.Parameters.AddWithValue("@cost", cost);
                    cmd.ExecuteNonQuery();
                }
                string sqlUpdate = "UPDATE cars SET status = 'На обслуживании' WHERE car_id = @carId";
                using (MySqlCommand cmd = new MySqlCommand(sqlUpdate, con))
                {
                    cmd.Parameters.AddWithValue("@carId", carId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Машина успешно отправлена на обслуживание.");
                this.Close();
            }
        }
    }
    public class ComboBoxItem
    {
        public string Text { get; set; }
        public int Value { get; set; }

        public ComboBoxItem(string text, int value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
