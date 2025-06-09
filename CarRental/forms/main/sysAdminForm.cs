using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace CarRental
    {
        public partial class sysAdminForm : Form
        {
            string connect = db.connect;
            public sysAdminForm()
            {
                InitializeComponent();
                FillTables();
            }

            private void button2_Click(object sender, EventArgs e)
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            private void btnImportData_Click(object sender, EventArgs e)
            {
            if (cmbTables.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите таблицу.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "CSV файлы (*.csv)|*.csv";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string path = dialog.FileName;
                string table = cmbTables.SelectedItem.ToString();

                try
                {
                    int count = ImportCSV(path, table);
                    MessageBox.Show($"Импортировано {count} записей.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при импорте: " + ex.Message);
                }
            }
        }

        private void FillTables()
        {
            cmbTables.Items.Clear();
            using (MySqlConnection connection = new MySqlConnection(connect))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SHOW TABLES", connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cmbTables.Items.Add(reader.GetString(0));
                }
                connection.Close();
            }
        }
        private void btnRestoreDatabase_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "SQL файлы (*.sql)|*.sql";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string script = File.ReadAllText(dialog.FileName);
                using (MySqlConnection con = new MySqlConnection(connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(script, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("База данных восстановлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private int ImportCSV(string path, string table)
        {
            int count = 0;
            var lines = File.ReadAllLines(path, Encoding.UTF8);
            if (lines.Length < 2) return 0;

            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                for (int i = 1; i < lines.Length; i++)
                {
                    var values = lines[i].Split(';');
                    string query = BuildInsertQuery(table, values);
                    if (string.IsNullOrEmpty(query)) continue;
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    count++;
                }
                con.Close();
            }
            return count;
        }

        private string BuildInsertQuery(string table, string[] v)
        {
            switch (table)
            {
                case "role":
                    return $"INSERT INTO role (name) VALUES ('{v[0]}')";
                case "employee":
                    return $"INSERT INTO employee (role_id, first_name, last_name, phone, email, employeeLogin, employeePass) VALUES ('{v[0]}','{v[1]}','{v[2]}','{v[3]}','{v[4]}','{v[5]}','{v[6]}')";
                case "cars":
                    return $"INSERT INTO cars (make, model, year, license_plate, status, price) VALUES ('{v[0]}','{v[1]}',{v[2]},'{v[3]}','{v[4]}',{v[5]})";
                case "customers":
                    return $"INSERT INTO customers (first_name, last_name, phone, driver_license, passport, email) VALUES ('{v[0]}','{v[1]}','{v[2]}','{v[3]}','{v[4]}','{v[5]}')";
                default:
                    return null;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            loginForm loginForm = new loginForm();
            loginForm.Show();
        }

        private void exportBtn_Click(object sender, EventArgs e)
        {
            if (cmbTables.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите таблицу.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV файлы (*.csv)|*.csv";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                string table = cmbTables.SelectedItem.ToString();

                using (MySqlConnection con = new MySqlConnection(connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT * FROM {table}", con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                    {
                        writer.WriteLine(string.Join(";", dt.Columns.Cast<DataColumn>().Select(col => col.ColumnName)));
                        foreach (DataRow row in dt.Rows)
                        {
                            writer.WriteLine(string.Join(";", row.ItemArray));
                        }
                    }
                }
                MessageBox.Show("Экспорт завершен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void backupData_Click(object sender, EventArgs e)
        {
            string fileName = $"backup_carrental_{DateTime.Now:yyyyMMdd_HHmmss}.sql";
            string path = Path.Combine(Application.StartupPath, "backup");
            Directory.CreateDirectory(path);
            string fullPath = Path.Combine(path, fileName);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connect))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.ExportToFile(fullPath);
                            conn.Close();
                        }
                    }
                }
                MessageBox.Show($"Резервная копия создана: {fullPath}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании резервной копии: " + ex.Message);
            }
        }
    }
}



