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
            private bool databaseExists;
            public sysAdminForm(bool dbExists)
            {
            InitializeComponent();
            databaseExists = dbExists;

            if (databaseExists)
            {
                FillTables();
            }
            else
            {
                MessageBox.Show("База данных не обнаружена. Используйте восстановление.",
                               "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTables.Enabled = false;
                btnImportData.Enabled = false;
                exportBtn.Enabled = false;
                backupData.Enabled = false;
            }
        }

            private void button2_Click(object sender, EventArgs e)
            {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти?",
                                                "Подтверждение выхода",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
            private void btnImportData_Click(object sender, EventArgs e)
            {
            if (cmbTables.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите таблицу.", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show($"Импортировано {count} записей.", "Успех",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FillTables();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при импорте: " + ex.Message,
                                   "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FillTables()
        {
            try
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

                if (cmbTables.Items.Count > 0)
                {
                    cmbTables.SelectedIndex = 0;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Ошибка подключения к базе: {ex.Message}",
                               "Ошибка базы данных",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnRestoreDatabase_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "SQL файлы (*.sql)|*.sql";
            dialog.Title = "Выберите файл резервной копии";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string script = File.ReadAllText(dialog.FileName);
                    string serverConnect = new MySqlConnectionStringBuilder(connect)
                    {
                        Database = ""
                    }.ToString();

                    using (MySqlConnection con = new MySqlConnection(serverConnect))
                    {
                        con.Open();
                        MySqlCommand createDbCmd = new MySqlCommand(
                            "DROP DATABASE IF EXISTS carrentaldb; " +
                            "CREATE DATABASE carrentaldb CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;",
                            con);
                        createDbCmd.ExecuteNonQuery();

                        MySqlCommand useDbCmd = new MySqlCommand("USE carrentaldb", con);
                        useDbCmd.ExecuteNonQuery();

                        MySqlScript sqlScript = new MySqlScript(con, script);
                        sqlScript.Delimiter = "$$";
                        sqlScript.Error += (senderObj, errorArgs) =>
                            throw new Exception($"Ошибка SQL: {errorArgs.Exception.Message}");

                        int count = sqlScript.Execute();
                        MessageBox.Show($"Выполнено {count} команд SQL.",
                                      "Восстановление",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);
                    }
                    databaseExists = true;
                    cmbTables.Enabled = true;
                    btnImportData.Enabled = true;
                    exportBtn.Enabled = true;
                    backupData.Enabled = true;
                    FillTables();

                    MessageBox.Show("База данных успешно восстановлена!",
                                  "Успех",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка восстановления: " + ex.Message,
                                   "Критическая ошибка",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
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
                using (MySqlTransaction transaction = con.BeginTransaction())
                {
                    int i = 1;

                    try
                    {
                        for (i = 1; i < lines.Length; i++)
                        {
                            var values = lines[i].Split(';');
                            using (MySqlCommand cmd = BuildInsertCommand(table, values, con))
                            {
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                                count++;
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Ошибка на строке {i}: {ex.Message}");
                    }
                }
            }
            return count;
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
                MessageBox.Show("Выберите таблицу.", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV файлы (*.csv)|*.csv";
            saveFileDialog.Title = "Экспорт данных";
            saveFileDialog.FileName = $"{cmbTables.SelectedItem}_export_{DateTime.Now:yyyyMMdd}.csv";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                string table = cmbTables.SelectedItem.ToString();

                try
                {
                    using (MySqlConnection con = new MySqlConnection(connect))
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand($"SELECT * FROM {table}", con);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (table.Equals("cars", StringComparison.OrdinalIgnoreCase))
                        {
                            dt.Columns.Remove("photo");
                        }

                        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                        {
                            writer.WriteLine(string.Join(";", dt.Columns.Cast<DataColumn>().Select(col => col.ColumnName)));
                            foreach (DataRow row in dt.Rows)
                            {
                                writer.WriteLine(string.Join(";",
                                    row.ItemArray.Select(x => x?.ToString() ?? "")));
                            }
                        }
                    }
                    MessageBox.Show($"Данные экспортированы в файл:\n{filePath}",
                                  "Экспорт завершен",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при экспорте: " + ex.Message,
                                   "Ошибка",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                }
            }
        }

        private MySqlCommand BuildInsertCommand(string table, string[] values, MySqlConnection con)
        {
            switch (table.ToLower())
            {
                case "role":
                    return new MySqlCommand(
                        "INSERT IGNORE INTO role (name) VALUES (@name)", con)
                    {
                        Parameters = { new MySqlParameter("@name", values[0]) }
                    };

                case "employee":
                    return new MySqlCommand(
                        "INSERT IGNORE INTO employee (role_id, first_name, last_name, phone, email, employeeLogin, employeePass) " +
                        "VALUES (@role_id, @first_name, @last_name, @phone, @email, @login, @pass)", con)
                    {
                        Parameters =
                        {
                            new MySqlParameter("@role_id", int.Parse(values[0])),
                            new MySqlParameter("@first_name", values[1]),
                            new MySqlParameter("@last_name", values[2]),
                            new MySqlParameter("@phone", values[3]),
                            new MySqlParameter("@email", values[4]),
                            new MySqlParameter("@login", values[5]),
                            new MySqlParameter("@pass", values[6])
                        }
                    };

                case "cars":
                    var cmd = new MySqlCommand(
                        "INSERT IGNORE INTO cars (make, model, year, license_plate, status, price) " +
                        "VALUES (@make, @model, @year, @license_plate, @status, @price)", con);

                    cmd.Parameters.AddWithValue("@make", values[0]);
                    cmd.Parameters.AddWithValue("@model", values[1]);
                    cmd.Parameters.AddWithValue("@year", int.Parse(values[2]));
                    cmd.Parameters.AddWithValue("@license_plate", values[3]);
                    cmd.Parameters.AddWithValue("@status", values[4]);
                    cmd.Parameters.AddWithValue("@price", decimal.Parse(values[5]));

                    return cmd;

                case "customers":
                    return new MySqlCommand(
                        "INSERT IGNORE INTO customers (first_name, last_name, phone, driver_license, passport, email) " +
                        "VALUES (@first_name, @last_name, @phone, @driver_license, @passport, @email)", con)
                    {
                        Parameters =
                        {
                            new MySqlParameter("@first_name", values[0]),
                            new MySqlParameter("@last_name", values[1]),
                            new MySqlParameter("@phone", values[2]),
                            new MySqlParameter("@driver_license", values[3]),
                            new MySqlParameter("@passport", values[4]),
                            new MySqlParameter("@email", values[5])
                        }
                    };

                default:
                    throw new ArgumentException($"Таблица {table} не поддерживается для импорта");
            }
        }


        private void backupData_Click(object sender, EventArgs e)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string backupDir = Path.Combine(documentsPath, "CarRental", "Backups");
            Directory.CreateDirectory(backupDir);

            string fileName = $"backup_carrentaldb_{DateTime.Now:yyyyMMdd_HHmmss}.sql";
            string fullPath = Path.Combine(backupDir, fileName);

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
                        }
                    }
                }

                MessageBox.Show($"Резервная копия успешно создана:\n{fullPath}",
                                "Успех",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании резервной копии:\n" + ex.Message,
                                "Ошибка",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
    }
}



