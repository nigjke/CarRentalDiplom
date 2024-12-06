using MySql.Data.MySqlClient;
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

namespace CarRental
{
    public partial class importForm : Form
    {
        public importForm()
        {
            InitializeComponent();
            LoadTables();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LoadTables()
        {
            cmbTables.Items.Add("Role");
            cmbTables.Items.Add("Cars");
            cmbTables.Items.Add("Employee");
            cmbTables.Items.Add("Customers");
        }

        private void btnRestoreDatabase_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "SQL Files (*.sql)|*.sql";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    RestoreDatabaseStructure(filePath);
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

        public static void ImportData(string filePath, string tablename)
        {
            int counter = 0;
            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = db.connect;
                con.Open();

                bool isFirstStr = true;

                Encoding encoding = Encoding.GetEncoding(1251);

                using (StreamReader sr = new StreamReader(filePath, encoding))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (isFirstStr) { isFirstStr = false; }
                        else
                        {
                            var values = line.Split(',');

                            string query = string.Empty;

                            switch (tablename)
                            {
                                case "Role":
                                    query = $"INSERT INTO `{tablename}` (Role_id, name) VALUES ({values[0]}, {values[1]})";
                                    break;
                                case "Cars":
                                    query = $"INSERT INTO `{tablename}` (car_id, make, model, year, license_plate,status,price) VALUES ({values[0]}, {values[1]}, {values[2]}, {values[3]}, {values[4]}, {values[5]}, {values[6]})";
                                    break;
                                case "Customers":
                                    query = $"INSERT INTO `{tablename}` (customer_id, first_name, last_name, phone,driver_license,passport) VALUES ({values[0]}, {values[1]}, {values[2]}, {values[3]}, {values[4]}, {values[5]})";
                                    break;
                                case "Employee":
                                    query = $"INSERT INTO `{tablename}` (employee_id, Role_id, firstName, lastName, phone , employeeLogin, employeePass) VALUES ({values[0]}, {values[1]}, {values[2]}, {values[3]}, {values[4]}, {values[5]} , {values[6]})";
                                    break;

                            }

                            try
                            {
                                using (MySqlCommand command = new MySqlCommand(query, con))
                                {
                                    command.ExecuteNonQuery();
                                    counter++;
                                }
                            }
                            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
                        }
                    }
                }
                MessageBox.Show($"Данные успешно импортированы. Добавлено {counter} записей");
            }
        }
        private void btnImportData_Click(object sender, EventArgs e)
        {
            if (cmbTables.SelectedIndex != -1)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "CSV файлы с раделителем ',' (*.csv)|*.csv";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        ImportData(filePath, cmbTables.SelectedItem.ToString());
                    }
                }
            }
        }
    }
}
