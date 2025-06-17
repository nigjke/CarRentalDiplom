using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CarRental
{
    public partial class addRental : Form
    {
        private db db;
        private System.Windows.Forms.Timer rentalCheckTimer;
        public addRental(int carId)
        {
            db = new db();
            InitializeComponent();
            LoadComboBoxes();
            dateTimePickerReturnDate.MinDate = DateTime.Now.AddDays(1);
            dateTimePickerReturnDate.MaxDate = DateTime.Now.AddYears(2);

            rentalCheckTimer = new System.Windows.Forms.Timer();
            rentalCheckTimer.Interval = 60000;
            rentalCheckTimer.Tick += new EventHandler(CheckRentals);
            rentalCheckTimer.Start();
            button1.Enabled = false;
            label6.Text = 0.ToString("C");
        }
        string connectionString = db.connect;
        private void LoadComboBoxes()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string queryMake = "SELECT DISTINCT make FROM cars WHERE status = 'Свободная'";
                MySqlDataAdapter adapterMake = new MySqlDataAdapter(queryMake, connection);
                DataTable dataTableMake = new DataTable();
                adapterMake.Fill(dataTableMake);
                comboBoxMake.DataSource = dataTableMake;
                comboBoxMake.DisplayMember = "make";
                comboBoxMake.SelectedIndex = 0;
                comboBoxMake_SelectedIndexChanged_1(null, null);

                string queryCustomers = @"
    SELECT customer_id, last_name, phone 
    FROM customers 
    WHERE customer_id NOT IN (
        SELECT customer_id 
        FROM fines 
        WHERE is_paid = 0
    )";
                MySqlDataAdapter adapterCustomers = new MySqlDataAdapter(queryCustomers, connection);
                DataTable dataTableCustomers = new DataTable();
                adapterCustomers.Fill(dataTableCustomers);
                comboBoxCustomer.DataSource = dataTableCustomers;
                comboBoxCustomer.DisplayMember = "last_name";
                comboBoxCustomer.ValueMember = "customer_id";
            }
        }
        private void CheckRentals(object sender, EventArgs e)
        {
            string path = Path.Combine(Application.StartupPath, "log.txt");
            File.AppendAllText(path, $"[{DateTime.Now}] Выполняется CheckRentals\n");

            string query = @"
        UPDATE cars
        SET status = 'Свободная'
        WHERE car_id NOT IN (
            SELECT car_id
            FROM rentals
            WHERE NOW() BETWEEN rental_date AND return_date
        )
        AND status = 'Занята'";

            using (MySqlConnection connection = new MySqlConnection(db.connect))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                int affected = command.ExecuteNonQuery();
                File.AppendAllText(path, $"[{DateTime.Now}] Обновлено записей: {affected}\n");
            }
        }
        private void CalculateTotalAmount()
        {
            if (comboBoxModel.SelectedItem == null || dateTimePickerRentalDate.Value == null || dateTimePickerReturnDate.Value == null)
            {
                textBoxTotalAmount.Text = string.Empty;
                label8.Text = "Скидка: 0 - 0%";
                return;
            }

            DateTime rentalDate = dateTimePickerRentalDate.Value.Date;
            DateTime returnDate = dateTimePickerReturnDate.Value.Date;

            int days = (int)(returnDate - rentalDate).TotalDays;
            if (days < 1) days = 1;

            string selectedModel = comboBoxModel.Text;
            string connectionString = db.connect;
            string queryPrice = "SELECT price FROM cars WHERE model = @model";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(queryPrice, connection);
                command.Parameters.AddWithValue("@model", selectedModel);
                connection.Open();
                object result = command.ExecuteScalar();
                decimal pricePerDay = result != null && result != DBNull.Value
                    ? Convert.ToDecimal(result)
                    : 0;

                decimal baseAmount = pricePerDay * days;

                decimal discountPercentage = 0;
                if (days >= 14 && days <= 30)
                {
                    discountPercentage = 0.10m; 
                }
                else if (days >= 7 && days < 14)
                {
                    discountPercentage = 0.05m;
                }

                decimal discountAmount = baseAmount * discountPercentage;
                decimal finalAmount = baseAmount - discountAmount;

                string discountInfo = $"Скидка: {discountAmount:C} - {discountPercentage * 100}%";


                textBoxTotalAmount.Text = finalAmount.ToString("C");
                label6.Text = discountInfo;
            }
        }


        private int GetCarId(string make, string model)
        {
            string connectionString = db.connect;
            string query = "SELECT car_id FROM cars WHERE make = @make AND model = @model";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@make", make);
                command.Parameters.AddWithValue("@model", model);
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }
        private void comboBoxMake_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var selectedRow = comboBoxMake.SelectedItem as DataRowView;
            if (selectedRow == null) return;

            string selectedMake = selectedRow["make"].ToString();
            string queryModel = "SELECT model FROM cars WHERE make = @make AND status = 'Свободная'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(queryModel, connection);
                command.Parameters.AddWithValue("@make", selectedMake);
                MySqlDataAdapter adapterModel = new MySqlDataAdapter(command);
                DataTable dataTableModel = new DataTable();
                adapterModel.Fill(dataTableModel);
                comboBoxModel.DataSource = dataTableModel;
                comboBoxModel.DisplayMember = "model";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxCustomer.SelectedValue == null || string.IsNullOrWhiteSpace(comboBoxMake.Text) || string.IsNullOrWhiteSpace(comboBoxModel.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            var customerId = Convert.ToInt32(comboBoxCustomer.SelectedValue);
            var carId = GetCarId(comboBoxMake.Text, comboBoxModel.Text);
            var rentalDate = dateTimePickerRentalDate.Value;
            var returnDate = dateTimePickerReturnDate.Value;
            var totalAmount = decimal.Parse(textBoxTotalAmount.Text, NumberStyles.Currency);

            using (var conn = new MySqlConnection(db.connect))
            {
                conn.Open();

                var insertQuery = @"
                    INSERT INTO rentals (customer_id, car_id, rental_date, return_date, total_amount) 
                    VALUES (@customer_id, @car_id, @rental_date, @return_date, @total_amount)";
                var insertCmd = new MySqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@customer_id", customerId);
                insertCmd.Parameters.AddWithValue("@car_id", carId);
                insertCmd.Parameters.AddWithValue("@rental_date", rentalDate);
                insertCmd.Parameters.AddWithValue("@return_date", returnDate);
                insertCmd.Parameters.AddWithValue("@total_amount", totalAmount);
                insertCmd.ExecuteNonQuery();

                var updateCarStatus = new MySqlCommand("UPDATE cars SET status = 'Занята' WHERE car_id = @car_id", conn);
                updateCarStatus.Parameters.AddWithValue("@car_id", carId);
                updateCarStatus.ExecuteNonQuery();
            }

            MessageBox.Show("Аренда успешно добавлена!");
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dateTimePickerReturnDate_ValueChanged_1(object sender, EventArgs e)
        {
            DateTime rentalDate = dateTimePickerRentalDate.Value.Date;
            DateTime returnDate = dateTimePickerReturnDate.Value.Date;

            if ((returnDate - rentalDate).TotalDays > 30)
            {
                MessageBox.Show("Максимальный срок аренды — 30 дней.");
                dateTimePickerReturnDate.Value = rentalDate.AddDays(30);
            }
            else if ((returnDate - rentalDate).TotalDays < 1)
            {
                MessageBox.Show("Минимальный срок аренды — 1 день.");
                dateTimePickerReturnDate.Value = rentalDate.AddDays(1);
            }

            CalculateTotalAmount();
            button1.Enabled = true;
        }

        private void comboBoxCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomer.SelectedItem is DataRowView row)
            {
                textPhone.Text = row["phone"].ToString();
            }
        }
    }
}
   

