using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CarRental
{
    public partial class editCustomer : Form
    {
        private db db;
        string connect = db.connect;
        private DataGridViewRow selectedRow;
        private int customerId;
        public editCustomer(DataGridViewRow row)
        {
            db = new db();
            selectedRow = row;
            InitializeComponent();
            LoadData();
        }
   
        private void button1_Click(object sender, EventArgs e)
        {
            if (IsFormValid())
            {
                UpdateDatabase(
                    textBox1.Text,
                    textBox2.Text,
                    maskedTextBox1.Text,
                    maskedTextBox2.Text,
                    maskedTextBox3.Text
                );

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

        // Utils Func

        private void LoadData()
        {
            customerId = Convert.ToInt32(selectedRow.Cells["customer_id"].Value);
            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                string sql = "SELECT first_name, last_name, phone, driver_license, passport FROM customers WHERE customer_id = @id";
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", customerId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox1.Text = reader["first_name"].ToString();
                            textBox2.Text = reader["last_name"].ToString();
                            maskedTextBox1.Text = reader["phone"].ToString();
                            maskedTextBox2.Text = reader["driver_license"].ToString();
                            maskedTextBox3.Text = reader["passport"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Клиент не найден");
                            this.Close();
                        }
                    }
                }
            }
        }
        private void UpdateDatabase(string first_name, string last_name, string phone, string driverLicense, string passport)
        {
            using (MySqlConnection connection = new MySqlConnection(connect))
            {
                connection.Open();

                string sql = @"UPDATE customers 
                               SET first_name = @first_name, last_name = @last_name, phone = @phone, driver_license = @driverLicense, passport = @passport 
                               WHERE customer_id = @customerId";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@first_name", first_name);
                    command.Parameters.AddWithValue("@last_name", last_name);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@driverLicense", driverLicense);
                    command.Parameters.AddWithValue("@passport", passport);
                    command.Parameters.AddWithValue("@customerId", customerId);

                    command.ExecuteNonQuery();
                }
            }
        }

        private bool IsFormValid()
        {
            return !string.IsNullOrWhiteSpace(textBox1.Text) &&
                   !string.IsNullOrWhiteSpace(textBox2.Text) &&
                   !string.IsNullOrWhiteSpace(maskedTextBox1.Text) &&
                   !string.IsNullOrWhiteSpace(maskedTextBox2.Text) &&
                   !string.IsNullOrWhiteSpace(maskedTextBox3.Text);
        }

        // Validation keyPress

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectRus(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectRus(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
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
    }
}
