using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental
{
    public partial class addCustomer : Form
    {
        private db db;
        string connect = db.connect;
        public addCustomer()
        {
            db = new db();
            InitializeComponent();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (IsFormValid())
            {
                AddCustomerToDatabase(
                    textBox1.Text,
                    textBox2.Text,
                    maskedTextBox1.Text,
                    maskedTextBox2.Text,
                    maskedTextBox3.Text
                );

                MessageBox.Show("Клиент добавлен");

                ClearForm();

                DialogResult = DialogResult.OK;
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

        // Utils Func()

        private void AddCustomerToDatabase(string firstName, string lastName, string phone, string driverLicense, string passport)
        {
            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();

                string sql = @"INSERT INTO customers (first_name, last_name, phone, driver_license, passport)
                               VALUES (@firstName, @lastName, @phone, @driverLicense, @passport)";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@firstName", firstName);
                    cmd.Parameters.AddWithValue("@lastName", lastName);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@driverLicense", driverLicense);
                    cmd.Parameters.AddWithValue("@passport", passport);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ClearForm()
        {
            textBox1.Clear();
            textBox2.Clear();
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
            maskedTextBox3.Clear();
        }

        private bool IsFormValid()
        {
            return !string.IsNullOrWhiteSpace(textBox1.Text)
                && !string.IsNullOrWhiteSpace(textBox2.Text)
                && !string.IsNullOrWhiteSpace(maskedTextBox1.Text)
                && !string.IsNullOrWhiteSpace(maskedTextBox2.Text)
                && !string.IsNullOrWhiteSpace(maskedTextBox3.Text);
        }

        // Validation KeyPress
        private void textBox1_KeyPress_2(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectRus(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress_2(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectRus(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_Leave_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                textBox1.Text = char.ToUpper(textBox1.Text[0]) + textBox1.Text.Substring(1);
        }

        private void textBox2_Leave_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
                textBox2.Text = char.ToUpper(textBox2.Text[0]) + textBox2.Text.Substring(1);
        }
    }
}
