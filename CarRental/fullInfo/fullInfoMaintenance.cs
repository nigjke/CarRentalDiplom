using CarRental.add;
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

namespace CarRental.fullInfo
{
    public partial class fullInfoMaintenance : Form
    {
        private int carId;
        public fullInfoMaintenance(int carId)
        {
            InitializeComponent();
            this.carId = carId;
            LoadMaintenanceData();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadMaintenanceData()
        {
            using (MySqlConnection con = new MySqlConnection(db.connect))
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
                string maintSql = @"
            SELECT 
                mt.name AS `Тип обслуживания`, 
                m.service_start_date AS `Дата начала`, 
                m.service_end_date AS `Дата окончания`, 
                m.cost AS `Стоимость`
            FROM maintenance m
            JOIN maintenance_type mt ON m.maintenance_type_id = mt.maintenance_type_id
            WHERE m.car_id = @id
            ORDER BY m.service_start_date DESC";

                using (var cmd = new MySqlCommand(maintSql, con))
                {
                    cmd.Parameters.AddWithValue("@id", carId);
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                        dataGridView1.Columns["Тип обслуживания"].HeaderText = "Тип обслуживания";
                        dataGridView1.Columns["Дата начала"].HeaderText = "Дата начала";
                        dataGridView1.Columns["Дата окончания"].HeaderText = "Дата окончания";
                        dataGridView1.Columns["Стоимость"].HeaderText = "Стоимость";

                        dataGridView1.Columns["Дата начала"].DefaultCellStyle.Format = "dd.MM.yyyy";
                        dataGridView1.Columns["Дата окончания"].DefaultCellStyle.Format = "dd.MM.yyyy";
                        dataGridView1.Columns["Стоимость"].DefaultCellStyle.Format = "N2";
                        dataGridView1.Columns["Стоимость"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dataGridView1.ReadOnly = true;
                        dataGridView1.AllowUserToAddRows = false;
                        dataGridView1.AllowUserToDeleteRows = false;
                        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10);
                        dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                    }
                }
            }
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            addMaintenance addMaintenance = new addMaintenance();
            if (addMaintenance.ShowDialog() == DialogResult.OK)
            {
                LoadMaintenanceData(); 
            }
        }
    }
}
