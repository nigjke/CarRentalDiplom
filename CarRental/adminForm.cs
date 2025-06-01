using CarRental.fullInfo;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;

namespace CarRental
{
    public partial class adminForm : Form
    {
        private FormBorderStyle _previousFormBorderStyle;
        private FormWindowState _previousWindowState;
        private bool _isFullscreen = false;

        private int currentPage = 1;
        private int totalRecords = 0;
        int pageSize = 10;
        private bool isHighlightEnabled = true; 
        private db db;
        private helper helper;

        private static string table = string.Empty;
        private DataTable customersTable;
        private DataGridViewRow selectedRow;
        private Timer inactivityTimer;
        private int inactivityTime;
        private DateTime lastActivityTime;
        private int selectedCarId = -1;
        private int selectedEmployeeId = -1;

        private string sortColumn = "";
        private string sortDirection = "";

        public adminForm(string labelLog)
        {
            InitializeComponent();
            helper = new helper();
            db = new db();
            this.label1.Text = $"{labelLog}";
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            InitializeInactivityTimer();
            dataGridView1.MultiSelect = false;
        }

        // Timer 
        private void InitializeInactivityTimer()
        {
            inactivityTime = int.Parse(ConfigurationManager.AppSettings["InactivityTime"]);
            inactivityTimer = new Timer();
            inactivityTimer.Interval = 1000;
            inactivityTimer.Tick += InactivityTimer_Tick;
            inactivityTimer.Start();
            lastActivityTime = DateTime.Now;
        }
        private void InactivityTimer_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now - lastActivityTime).TotalSeconds > inactivityTime)
            {
                inactivityTimer.Stop();
                loginForm loginForm = new loginForm();
                loginForm.Show();
                this.Hide();
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            lastActivityTime = DateTime.Now;
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            lastActivityTime = DateTime.Now;
        }
        private void SetButtonVisibility(bool btn7, bool btn8, bool btn9)
        {
            addBtn.Visible = btn7;
            editBtn.Visible = btn8;
            delBtn.Visible = btn9;
        }

        // Data Load
        private void adminForm_Load(object sender, EventArgs e)
        {
            SetButtonVisibility(true, true, true);
            helper.SetButtonColors(carBtn, customerBtn, rentalBtn, employeeBtn);
            LoadTable("cars", "Машины", new string[] { "По Марке", "По Модели", "По Статусу", "По Цене" });
        }

        private Dictionary<string, Dictionary<string, string>> columnMappings = new Dictionary<string, Dictionary<string, string>>
        {
            ["cars"] = new Dictionary<string, string>
            {
                ["Марка"] = "make",
                ["Модель"] = "model",
                ["Статус"] = "status",
                ["Цена за сутки"] = "price"
            },
            ["customers"] = new Dictionary<string, string>
            {
                ["Имя"] = "first_name",
                ["Фамилия"] = "last_name",
                ["Телефон"] = "phone",
                ["Вод.Удостоверение"] = "driver_license",
                ["Паспорт"] = "passport"
            },
            ["employee"] = new Dictionary<string, string>
            {
                ["Имя"] = "e.first_name",
                ["Фамилия"] = "e.last_name",
                ["Роль"] = "r.name"
            },
            ["rentals"] = new Dictionary<string, string>
            {
                ["Марка"] = "c.make",
                ["Модель"] = "c.model",
                ["Дата взятия"] = "r.rental_date",
                ["Дата возврата"] = "r.return_date",
                ["Сумма"] = "r.total_amount"
            }
        };


        private void LoadData()
        {
            using (MySqlConnection connection = new MySqlConnection(db.connect))
            {
                connection.Open();
                string query = "";
                int offset = (currentPage - 1) * pageSize;

                MySqlCommand counter;

                if (table == "customers")
                {
                    counter = new MySqlCommand("SELECT COUNT(*) FROM customers", connection);
                    totalRecords = Convert.ToInt32(counter.ExecuteScalar());

                    query = $@"
                SELECT 
                    customer_id, 
                    first_name AS 'Имя', 
                    last_name AS 'Фамилия', 
                    CONCAT(LEFT(phone, 2), REPEAT('*', CHAR_LENGTH(phone) - 6), RIGHT(phone, 4)) AS 'Телефон',
                    CONCAT(LEFT(driver_license, 2), REPEAT('*', CHAR_LENGTH(driver_license) - 6), RIGHT(driver_license, 4)) AS 'Вод.Удостоверение',
                    CONCAT(LEFT(passport, 2), REPEAT('*', CHAR_LENGTH(passport) - 6), RIGHT(passport, 4)) AS 'Паспорт'
                FROM customers";
                }
                else if (table == "cars")
                {
                    counter = new MySqlCommand("SELECT COUNT(*) FROM cars", connection);
                    totalRecords = Convert.ToInt32(counter.ExecuteScalar());

                    query = $@"
                    SELECT 
                        car_id, 
                        make AS 'Марка', 
                        model AS 'Модель', 
                        status AS 'Статус', 
                        price AS 'Цена за сутки' 
                    FROM cars";
                }
                else if (table == "employee")
                {
                    counter = new MySqlCommand("SELECT COUNT(*) FROM employee", connection);
                    totalRecords = Convert.ToInt32(counter.ExecuteScalar());

                    query = $@"
                SELECT 
                    e.employee_id,
                    e.first_name AS 'Имя',
                    e.last_name AS 'Фамилия',
                    r.name AS 'Роль'
                FROM employee e
                INNER JOIN role r ON r.role_id = e.role_id";
                }
                else if (table == "rentals")
                {
                    counter = new MySqlCommand("SELECT COUNT(*) FROM rentals", connection);
                    totalRecords = Convert.ToInt32(counter.ExecuteScalar());

                    query = $@"
                SELECT 
                    r.rental_id, 
                    c.make AS 'Марка', 
                    c.model AS 'Модель', 
                    r.rental_date AS 'Дата взятия', 
                    r.return_date AS 'Дата возврата', 
                    r.total_amount AS 'Сумма' 
                FROM rentals r
                INNER JOIN cars c ON c.car_id = r.car_id";
                }

                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
                {
                    if (columnMappings.ContainsKey(table) && columnMappings[table].ContainsKey(sortColumn))
                    {
                        string dbColumn = columnMappings[table][sortColumn];
                        query += $" ORDER BY {dbColumn} {sortDirection}";
                    }
                }

                query += $" LIMIT {pageSize} OFFSET {offset}";

                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;

                int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                labelInfo.Text = $"Страница {currentPage} из {totalPages} | Записи: {offset + 1} - {offset + dataTable.Rows.Count} из {totalRecords}";

                pictureBox3.Enabled = currentPage > 1;
                pictureBox2.Enabled = currentPage * pageSize < totalRecords;

                if (dataGridView1.Columns.Count > 0)
                    dataGridView1.Columns[0].Visible = false;

                if (isHighlightEnabled)
                    ApplyHighlighting();
            }
            CheckMaintenanceStatus();
        }

        private void LoadTable(string tableName, string labelText, string[] comboBoxItems)
        {
            label2.Text = labelText;
            searchBox.Text = "Поиск";
            table = tableName;
            UpdateSortComboBox();
            LoadData();
            if (table == "rentals")
            {
                addBtn.Text = "Справка";
                editBtn.Text = "Отключить подсветку";
            }
            else
            {
                addBtn.Text = "Добавить";
                editBtn.Text = "Редактировать";
            }
        }

        private void CheckMaintenanceStatus()
        {
            using (var con = new MySqlConnection(db.connect))
            {
                con.Open();
                string sql = @"
            UPDATE cars
            SET status = 'Свободная'
            WHERE car_id NOT IN (
                SELECT car_id
                FROM maintenance
                WHERE CURDATE() BETWEEN service_start_date AND service_end_date
            )
            AND status = 'На обслуживании'";
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }


        // Utils Functions
        private void UpdateSortComboBox()
        {
            comboBox1.Items.Clear();

            if (table == "cars")
                comboBox1.Items.AddRange(new string[] { "Марка", "Модель", "Статус", "Цена за сутки" });
            else if (table == "customers")
                comboBox1.Items.AddRange(new string[] { "Имя", "Фамилия", "Телефон", "Вод.Удостоверение", "Паспорт" });
            else if (table == "employee")
                comboBox1.Items.AddRange(new string[] { "Имя", "Фамилия", "Роль" });
            else if (table == "rentals")
                comboBox1.Items.AddRange(new string[] { "Марка", "Модель", "Дата взятия", "Дата возврата", "Сумма" });

            comboBox1.SelectedIndex = 0;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedRow = dataGridView1.Rows[e.RowIndex];
            }
        }
        private bool IsDataUsedInRentals(DataGridViewRow row)
        {
            string carId = row.Cells["Модель"].Value?.ToString();
            if (carId != null)
            {
                using (MySqlConnection conn = new MySqlConnection(db.connect))
                {
                    try
                    {
                        conn.Open();
                        string query = "SELECT COUNT(*) FROM carrentaldb.rentals INNER JOIN cars ON cars.car_id = rentals.car_id WHERE cars.model = @carId;";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@carId", carId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при проверке данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                }
            }
            return false;
        }
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (!isHighlightEnabled) return;
            if (table == "rentals")
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "Дата возврата" && e.Value != null)
                {
                    DateTime endDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells["Дата возврата"].Value);
                    DateTime startDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells["Дата взятия"].Value);
                    DateTime now = DateTime.Now;
                    if (endDate < now)
                    {
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                    }
                    else if (startDate > now)
                    {
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green;
                    }
                    else
                    {
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                    }
                }
            }
        }
        // Paginations
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (currentPage * pageSize < totalRecords)
            {
                currentPage++;
                LoadData();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadData();
            }
        }
        // Menu Buttons
        private void carBtn_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            SetButtonVisibility(true, true, true);
            helper.SetButtonColors(carBtn, rentalBtn, customerBtn, employeeBtn);
            LoadTable("cars", "Машины", new string[] { "По Марке", "По Модели", "По Статусу", "По Цене" });
        }
        private void employeeBtn_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            SetButtonVisibility(true, true, true);
            helper.SetButtonColors(employeeBtn, rentalBtn, carBtn, customerBtn);
            LoadTable("employee", "Сотрудники", new string[] { "По Имени", "По Фамилии", "По Роли" });
        }
        private void customerBtn_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            SetButtonVisibility(false, false, false);
            helper.SetButtonColors(customerBtn, rentalBtn, carBtn, employeeBtn);
            LoadTable("customers", "Клиенты", new string[] { "По Имени", "По Фамилии", "По Телефону", "По Вод.Удостоверению", "По Паспорту" });
        }
        private void rentalBtn_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            SetButtonVisibility(true, true, false);
            helper.SetButtonColors(rentalBtn, customerBtn, carBtn, employeeBtn);
            LoadTable("rentals", "Аренды", new string[] { "По Марке", "По Модели", "По Дате взятия", "По Дате возврата", "По Сумме" });
            
        }
        private void backBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            loginForm loginForm = new loginForm();
            loginForm.Show();
        }
        private void exitBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // Sort Buttons
        private void ascendingBtn_Click(object sender, EventArgs e)
        {
            sortColumn = comboBox1.SelectedItem?.ToString();
            sortDirection = "ASC";
            currentPage = 1;
            LoadData();
        }
        private void descendingBtn_Click(object sender, EventArgs e)
        {
            sortColumn = comboBox1.SelectedItem?.ToString();
            sortDirection = "DESC";
            currentPage = 1;
            LoadData();
        }
        // Event Buttons
        private void addBtn_Click(object sender, EventArgs e)
        {
            if (table == "rentals")
            {
                addBtn.Text = "Справка";
            }
            else
            {
                addBtn.Text = "Добавить";
            }

            if (table == "customers")
            {
                addCustomer addCustomer = new addCustomer();
                addCustomer.ShowDialog();
            }
            else if (table == "cars")
            {
                addCar addCar = new addCar();
                addCar.ShowDialog();
            }
            else if (table == "employee")
            {
                addEmployee addEmployee = new addEmployee();
                addEmployee.ShowDialog();
            }
            else if (table == "rentals")
            {
                fullInfoReference fullInfoReference = new fullInfoReference();
                fullInfoReference.ShowDialog();
            }
                LoadData();
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                bool isEmptyRow = true;
                foreach (DataGridViewCell cell in selectedRow.Cells)
                {
                    if (cell.Value != null && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
                    {
                        isEmptyRow = false;
                        break;
                    }
                }

                if (isEmptyRow)
                {
                    MessageBox.Show("Выбранная строка пуста и не может быть отредактирована.");
                    return;
                }
                if (table == "customers")
                {
                    editCustomer editCustomer = new editCustomer(selectedRow);
                    if (editCustomer.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
                else if (table == "cars")
                {
                    editCar editCar = new editCar(selectedRow);
                    if (editCar.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
                else if (table == "employee")
                {
                    editEmployee editEmployee = new editEmployee(selectedRow);
                    if (editEmployee.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
                else if (table == "rentals")
                {
                    isHighlightEnabled = !isHighlightEnabled;

                    if (isHighlightEnabled)
                    {
                        editBtn.Text = "Отключить подсветку";
                        ApplyHighlighting();
                    }
                    else
                    {
                        editBtn.Text = "Включить подсветку";
                        ClearHighlighting();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования");
            }
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                bool isEmpty = true;
                foreach (DataGridViewCell cell in selectedRow.Cells)
                {
                    if (cell.Value != null && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
                    {
                        isEmpty = false;
                        break;
                    }
                }
                if (isEmpty)
                {
                    MessageBox.Show("Выбранная строка пуста и не может быть удалена.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (table == "cars")
                {
                    if (IsDataUsedInRentals(selectedRow))
                    {
                        MessageBox.Show("Данные из выбранной строки используются в других таблицах и не могут быть удалены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                var result = MessageBox.Show("Вы уверены, что хотите удалить выбранную строку?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    dataGridView1.Rows.Remove(selectedRow);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void reportBtn_Click(object sender, EventArgs e)
        {
            importForm importForm = new importForm();
            importForm.ShowDialog();
        }

        // Search
        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            string query = "";
            string txt = searchBox.Text;
            using (MySqlConnection connection = new MySqlConnection(db.connect))
            {
                MySqlCommand command = connection.CreateCommand();
                if (table == "cars")
                {
                    query = @"
                SELECT 
                    car_id,
                    make AS 'Марка', 
                    model AS 'Модель', 
                    status AS 'Статус', 
                    price AS 'Цена за сутки' 
                FROM cars 
                WHERE 
                    make LIKE @txt OR 
                    model LIKE @txt OR 
                    status LIKE @txt OR 
                    CAST(price AS CHAR) LIKE @txt";
                }
                else if (table == "customers")
                {
                    query = @"
                SELECT 
                    customer_id,
                    first_name AS 'Имя', 
                    last_name AS 'Фамилия', 
                    CONCAT(LEFT(phone, 2), REPEAT('*', CHAR_LENGTH(phone) - 6), RIGHT(phone, 4)) AS 'Телефон', 
                    CONCAT(LEFT(driver_license, 2), REPEAT('*', CHAR_LENGTH(driver_license) - 6), RIGHT(driver_license, 4)) AS 'Вод.Удостоверение', 
                    CONCAT(LEFT(passport, 2), REPEAT('*', CHAR_LENGTH(passport) - 6), RIGHT(passport, 4)) AS 'Паспорт' 
                FROM customers 
                WHERE 
                    first_name LIKE @txt OR 
                    last_name LIKE @txt OR 
                    phone LIKE @txt OR 
                    driver_license LIKE @txt OR 
                    passport LIKE @txt";
                }
                else if (table == "employee")
                {
                    query = @"
                SELECT 
                    employee_id,
                    e.first_name AS 'Имя', 
                    e.last_name AS 'Фамилия', 
                    r.name AS 'Роль'
                FROM carrentaldb.employee e
                INNER JOIN role r ON e.role_id = r.role_id
                WHERE 
                    e.first_name LIKE @txt OR 
                    e.last_name LIKE @txt OR 
                    r.name LIKE @txt";
                }
                else if (table == "rentals")
                {
                    query = @"
                        SELECT 
                            rentals.rental_id, 
                            cars.make AS 'Марка', 
                            cars.model AS 'Модель', 
                            rentals.rental_date AS 'Дата взятия', 
                            rentals.return_date AS 'Дата возврата', 
                            rentals.total_amount AS 'Сумма' 
                        FROM carrentaldb.rentals 
                        INNER JOIN carrentaldb.cars ON cars.car_id = rentals.car_id 
                        WHERE 
                            cars.make LIKE @txt OR 
                            cars.model LIKE @txt OR 
                            rentals.rental_date LIKE @txt OR 
                            rentals.return_date LIKE @txt OR 
                            rentals.total_amount LIKE @txt";
                }
                else
                {
                    return;
                }

                command.CommandText = query;
                command.Parameters.AddWithValue("@txt", "%" + txt + "%");

                DataTable dt = new DataTable();

                try
                {
                    connection.Open();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }

                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при поиске: " + ex.Message);
                }
            }
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            searchBox.Text = string.Empty;
        }
        private void textBox1_Click(object sender, EventArgs e)
        {
            searchBox.Text = string.Empty;
        }

        private void viewCarMenuItem_Click(object sender, EventArgs e)
        {
            if (table == "cars")
            {
                if (selectedCarId == -1)
                {
                    MessageBox.Show("Машина не выбрана.");
                    return;
                }
                using (var carInfo = new fullInfoCar.fullInfoCar(selectedCarId))
                {
                    carInfo.ShowDialog();
                }
            }
            if (table == "employee")
            {
                if (selectedEmployeeId == -1)
                {
                    MessageBox.Show("Выберите сотрудника!");
                    return;
                }
                var employeeForm = new fullInfo.fullInfoEmployee(selectedEmployeeId);
                employeeForm.FormClosed += (s, args) => employeeForm.Dispose();
                employeeForm.ShowDialog();
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            contextMenuStrip1.Hide();
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[e.RowIndex].Selected = true;
                if (table == "cars")
                {
                    MaintencToolStripMenuItem.Visible = true;
                    selectedCarId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["car_id"].Value);
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (table == "employee")
                {
                    MaintencToolStripMenuItem.Visible = false;
                    selectedEmployeeId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["employee_id"].Value);
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }
        private void ApplyHighlighting()
        {
            if (table != "rentals") return;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                DateTime now = DateTime.Now;
                if (row.Cells["Дата возврата"].Value != null && row.Cells["Дата взятия"].Value != null)
                {
                    DateTime endDate = Convert.ToDateTime(row.Cells["Дата возврата"].Value);
                    DateTime startDate = Convert.ToDateTime(row.Cells["Дата взятия"].Value);

                    if (endDate < now)
                        row.DefaultCellStyle.BackColor = Color.Red;
                    else if (startDate > now)
                        row.DefaultCellStyle.BackColor = Color.Green;
                    else
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }
        private void ClearHighlighting()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (!_isFullscreen)
            {
                _previousFormBorderStyle = this.FormBorderStyle;
                _previousWindowState = this.WindowState;

                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;

                _isFullscreen = true;
            }
            else
            {
                this.FormBorderStyle = _previousFormBorderStyle;
                this.WindowState = _previousWindowState;

                _isFullscreen = false;
            }
            UpdatePageSizeAndReload();
        }

        private int CalculatePageSize()
        {
            int rowHeight = dataGridView1.RowTemplate.Height;
            int availableHeight = dataGridView1.Height;

            int rows = (availableHeight - dataGridView1.ColumnHeadersHeight) / rowHeight;

            return Math.Max(rows, 10);
        }

        private void UpdatePageSizeAndReload()
        {
            pageSize = CalculatePageSize();
            currentPage = 1;
            LoadData();
        }
        private void MaintencToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fullInfoMaintenance fullInfoMaintenance = new fullInfoMaintenance(selectedCarId);
            fullInfoMaintenance.ShowDialog();
        }
    }
}
