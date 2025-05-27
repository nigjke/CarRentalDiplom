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
        private int carId;
        public addMaintenance(int carId)
        {
            InitializeComponent();
            this.carId = carId;
            LoadForm();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (comboBoxType.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип обслуживания.");
                return;
            }

            if (!decimal.TryParse(textBoxCost.Text, out decimal cost))
            {
                MessageBox.Show("Введите корректную стоимость.");
                return;
            }

            int typeId = (int)comboBoxType.SelectedValue;
            DateTime start = dateStart.Value.Date;
            DateTime end = dateEnd.Value.Date;

            if (end < start)
            {
                MessageBox.Show("Дата окончания не может быть раньше даты начала.");
                return;
            }
            using (var con = new MySqlConnection(db.connect))
            {
                con.Open();
                string insertSql = @"
            INSERT INTO maintenance (car_id, maintenance_type_id, service_start_date, service_end_date, cost)
            VALUES (@carId, @typeId, @start, @end, @cost)";
                using (var cmd = new MySqlCommand(insertSql, con))
                {
                    cmd.Parameters.AddWithValue("@carId", carId);
                    cmd.Parameters.AddWithValue("@typeId", typeId);
                    cmd.Parameters.AddWithValue("@start", start);
                    cmd.Parameters.AddWithValue("@end", end);
                    cmd.Parameters.AddWithValue("@cost", cost);
                    cmd.ExecuteNonQuery();
                }
                if (DateTime.Today >= start && DateTime.Today <= end)
                {
                    string updateStatus = "UPDATE cars SET status = 'На обслуживании' WHERE car_id = @carId";
                    using (var cmd = new MySqlCommand(updateStatus, con))
                    {
                        cmd.Parameters.AddWithValue("@carId", carId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            MessageBox.Show("Обслуживание добавлено.");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadForm()
        {
            using (var con = new MySqlConnection(db.connect))
            {
                con.Open();
                string carSql = "SELECT make, model FROM cars WHERE car_id = @id";
                using (var cmd = new MySqlCommand(carSql, con))
                {
                    cmd.Parameters.AddWithValue("@id", carId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            labelCar.Text = $"Машина: {reader["make"]} {reader["model"]}";
                        }
                    }
                }

                string typeSql = "SELECT maintenance_type_id, name FROM maintenance_type";
                using (var adapter = new MySqlDataAdapter(typeSql, con))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboBoxType.DisplayMember = "name";
                    comboBoxType.ValueMember = "maintenance_type_id";
                    comboBoxType.DataSource = dt;
                }
            }
            dateStart.Value = DateTime.Today;
            dateEnd.Value = DateTime.Today.AddDays(1);
        }
    }
}
