﻿using MySql.Data.MySqlClient;
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
    public partial class fullInfoCustomers : Form
    {
        private int _customerId;
        private bool _isClosing;
        public fullInfoCustomers(int selectedCustomerId)
        {
            InitializeComponent();
            _customerId = selectedCustomerId;
            this.FormClosing += (s, e) => _isClosing = true;
            LoadData(_customerId);
        }

        private void LoadData(int customerId)
        {
            using (MySqlConnection connection = new MySqlConnection(db.connect))
            {
                connection.Open();
                string query = "SELECT customer_id, first_name as 'Имя', last_name as 'Фамилия', phone as 'Телефон', driver_license as 'Лицензия', passport as 'Паспорт' FROM customers WHERE customer_id=@customerId ";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@customerId", customerId);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
                dataGridView1.Columns[0].Visible = false;
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            _isClosing = true;
            this.Close();
        }
    }
}
