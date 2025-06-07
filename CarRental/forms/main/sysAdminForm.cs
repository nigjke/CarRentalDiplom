    using MySql.Data.MySqlClient;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    namespace CarRental
    {
        public partial class sysAdminForm : Form
        {
            string connect = db.connect;
            public sysAdminForm()
            {
                InitializeComponent();
                LoadTables();
            }

            private void button2_Click(object sender, EventArgs e)
            {
                this.Close();
                loginForm loginForm = new loginForm();
                loginForm.Show();
            }
            private void btnImportData_Click(object sender, EventArgs e)
            {
                if (cmbTables.SelectedIndex == -1)
                    return;

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "CSV файлы (*.csv)|*.csv";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        ImportData(openFileDialog.FileName, cmbTables.SelectedItem.ToString());
                    }
                }
            }

        private void LoadTables()
            {
                cmbTables.Items.AddRange(new object[]
                  {
                        "role", "cars", "customers", "employee",
                        "fines", "finestype", "maintenance", "maintenance_type",
                        "rentals", "reviews"
                  });
            }
            private void btnRestoreDatabase_Click(object sender, EventArgs e)
            {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "SQL Files (*.sql)|*.sql";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string sqlScript = File.ReadAllText(openFileDialog.FileName);
                    ExecuteSqlScript(sqlScript);
                }
            }
        }
            private void RestoreDatabaseStructure(string filePath)
            {
                string sqlScript = File.ReadAllText(filePath);

                InsertDataOnDb(sqlScript);
            }
            static public long InsertDataOnDb(string query)
            {
                using (MySqlConnection con = new MySqlConnection())
                {
                    con.ConnectionString = db.connect;
                    con.Open();

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();

                    con.Close();
                    return cmd.LastInsertedId;
                }
            }

        private void ImportData(string filePath, string tableName)
        {
            int counter = 0;
            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                Encoding encoding = Encoding.GetEncoding(1251);
                using (StreamReader reader = new StreamReader(filePath, encoding))
                {
                    string line;
                    bool isFirstLine = true;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        string[] values = line.Split(',');
                        string query = BuildInsertQuery(tableName, values);

                        if (!string.IsNullOrEmpty(query))
                        {
                            try
                            {
                                MySqlCommand cmd = new MySqlCommand(query, con);
                                cmd.ExecuteNonQuery();
                                counter++;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка: {ex.Message}");
                                return;
                            }
                        }
                    }
                }
                MessageBox.Show($"Успешно импортировано {counter} записей в таблицу {tableName}.");
            }
        }

        private string BuildInsertQuery(string table, string[] v)
        {
            switch (table)
            {
                case "role":
                    return $"INSERT INTO role (role_id, name) VALUES ({v[0]}, '{v[1]}')";

                case "cars":
                    return $"INSERT INTO cars (car_id, make, model, year, license_plate, status, price) " +
                           $"VALUES ({v[0]}, '{v[1]}', '{v[2]}', {v[3]}, '{v[4]}', '{v[5]}', {v[6]})";

                case "customers":
                    return $"INSERT INTO customers (customer_id, first_name, last_name, phone, driver_license, passport, email) " +
                           $"VALUES ({v[0]}, '{v[1]}', '{v[2]}', '{v[3]}', '{v[4]}', '{v[5]}', '{v[6]}')";

                case "employee":
                    return $"INSERT INTO employee (employee_id, role_id, first_name, last_name, phone, email, employeeLogin, employeePass) " +
                           $"VALUES ({v[0]}, {v[1]}, '{v[2]}', '{v[3]}', '{v[4]}', '{v[5]}', '{v[6]}', '{v[7]}')";

                case "fines":
                    return $"INSERT INTO fines (fine_id, rental_id, customer_id, fine_type_id, fine_amount, fine_date, is_paid) " +
                           $"VALUES ({v[0]}, {v[1]}, {v[2]}, {v[3]}, {v[4]}, '{v[5]}', {v[6]})";

                case "finestype":
                    return $"INSERT INTO finestype (fine_type_id, name, amount) " +
                           $"VALUES ({v[0]}, '{v[1]}', {v[2]})";

                case "maintenance":
                    return $"INSERT INTO maintenance (maintenance_id, car_id, maintenance_type_id, cost, service_start_date, service_end_date) " +
                           $"VALUES ({v[0]}, {v[1]}, {v[2]}, {v[3]}, '{v[4]}', '{v[5]}')";

                case "maintenance_type":
                    return $"INSERT INTO maintenance_type (maintenance_type_id, name) VALUES ({v[0]}, '{v[1]}')";

                case "rentals":
                    return $"INSERT INTO rentals (rental_id, customer_id, car_id, rental_date, return_date, employee_id, total_amount) " +
                           $"VALUES ({v[0]}, {v[1]}, {v[2]}, '{v[3]}', '{v[4]}', {v[5]}, {v[6]})";

                case "reviews":
                    return $"INSERT INTO reviews (review_id, customer_id, car_id, rating, comment, review_date, rental_id) " +
                           $"VALUES ({v[0]}, {v[1]}, {v[2]}, {v[3]}, '{v[4]}', '{v[5]}', {v[6]})";

                default:
                    return null;
            }
        }

            private void ExecuteSqlScript(string script)
            {
                using (MySqlConnection con = new MySqlConnection(connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(script, con);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }



