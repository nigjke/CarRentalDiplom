using CarRental.fullInfo;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Word = Microsoft.Office.Interop.Word;

namespace CarRental
{
    public partial class managerForm : Form
    {
        private FormBorderStyle _previousFormBorderStyle;
        private FormWindowState _previousWindowState;
        private bool _isFullscreen = false;

        private int currentPage = 1;
        private int totalRecords = 0;
        int pageSize = 10;
        private bool isHighlightEnabled = true;
        private helper helper;
        private db db;
        private static string table = string.Empty;
        private DataGridViewRow selectedRow;

        private string sortColumn = "";
        private string sortDirection = "";
        public managerForm(string labelLog)
        {
            helper = new helper();
            db = new db();
            InitializeComponent();
            label1.Text = $"{labelLog}";
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellMouseDown += dataGridView1_CellMouseDown;
            dataGridView1.ContextMenuStrip = null;
        }

        private void SetButtonVisibility(bool btn7, bool btn8, bool btn9)
        {
            addBtn.Visible = btn7;
            editBtn.Visible = btn8;
            checkBtn.Visible = btn9;
        }

        private void LoadTable(string tableName, string labelText, string[] comboBoxItems)
        {
            label2.Text = labelText;
            searchBox.Text = "Поиск";
            table = tableName;
            UpdateSortComboBox();
            LoadData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
        }
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
                chkFreeOnly.Visible = false;
                string query = "";
                int offset = (currentPage - 1) * pageSize;

                MySqlCommand counter;

                referenceBtn.Visible = false;
                ReferenceOnOffBtn.Visible = false;

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
                    chkFreeOnly.Visible = true;
                    if (chkFreeOnly.Checked)
                    {
                        counter = new MySqlCommand("SELECT COUNT(*) FROM cars WHERE status = 'Свободная'", connection);
                        totalRecords = Convert.ToInt32(counter.ExecuteScalar());

                        query = $@"
                    SELECT 
                        car_id, 
                        make AS 'Марка', 
                        model AS 'Модель', 
                        status AS 'Статус', 
                        price AS 'Цена за сутки' 
                    FROM cars 
                    WHERE status = 'Свободная'";
                    }
                    else
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

                    referenceBtn.Visible = true;
                    ReferenceOnOffBtn.Visible = true;
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

                ResetContextMenu();
            }
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            Close();
            var loginForm = new loginForm();
            loginForm.Show();
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                System.Windows.Forms.Application.Exit();
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

        private void managerForm_Load(object sender, EventArgs e)
        {
            SetButtonVisibility(false, false, false);
            helper.SetButtonColors(carBtn, rentalBtn, customerBtn);
            LoadTable("cars", "Машины", new string[] { "По Марке", "По Модели", "По Статусу", "По Цене" });
        }
        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedRow = dataGridView1.Rows[e.RowIndex];
            }
        }

        private void rentalBtn_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            SetButtonVisibility(true, false, true);
            helper.SetButtonColors(rentalBtn, customerBtn, carBtn);
            LoadTable("rentals", "Аренды", new string[] { "По Марке", "По Модели", "По дате взятия", "По дате возврата", "По сумме" });
        }

        private void customerBtn_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            SetButtonVisibility(true, true, false);
            helper.SetButtonColors(customerBtn, rentalBtn, carBtn);
            LoadTable("customers", "Клиенты", new string[] { "По Имени", "По Фамилии", "По Телефону", "По Вод.Удостоверению", "По Паспорту" });
        }

        private void carBtn_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            SetButtonVisibility(false, false, false);
            helper.SetButtonColors(carBtn, rentalBtn, customerBtn);
            LoadTable("cars", "Машины", new string[] { "По Марке", "По Модели", "По Статусу", "По Цене" });
        }

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

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (table == "customers")
            {
                var addCustomer = new addCustomer();
                addCustomer.ShowDialog();
            }
            else if (table == "rentals")
            {
                var addRental = new addRental();
                addRental.ShowDialog();
            }
            LoadData();
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
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
                    var editCustomer = new editCustomer(selectedRow);
                    if (editCustomer.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования");
            }
        }

        private void checkBtn_Click(object sender, EventArgs e)
        {
            if (selectedRow != null)
            {
                helper.CreateWordReport(selectedRow);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку.");
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[e.RowIndex].Selected = true;
                if (table == "cars")
                {
                    helper.selectedCarId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["car_id"].Value);
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (table == "customers")
                {
                    contextMenuStrip1.Hide();
                    отзывыToolStripMenuItem.Text = "Отзыв";

                    helper.selectedCustomerId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["customer_id"].Value);
                    contextMenuStrip2.Show(Cursor.Position);
                }
                else if (table == "rentals")
                {
                    helper.selectedRentalId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["rental_id"].Value);
                    helper.selectedCarId = GetCarIdFromRental(helper.selectedRentalId);
                    object returnDateObj = dataGridView1.Rows[e.RowIndex].Cells["Дата возврата"].Value;
                    bool isCompleted = returnDateObj != DBNull.Value && returnDateObj != null;

                    bool hasReview = CheckReviewExists(helper.selectedRentalId);

                    reviewToolStripMenuItem.Visible = isCompleted && !hasReview;
                    отзывыToolStripMenuItem.Text = "Добавить отзыв";

                    contextMenuStrip2.Show(Cursor.Position);
                }
                else
                {
                    contextMenuStrip1.Hide();
                    contextMenuStrip2.Hide();
                }
            }
        }

        private void viewCarMenuItem_Click(object sender, EventArgs e)
        {
            var carInfo = new fullInfoCar.fullInfoCar(helper.selectedCarId);
            carInfo.ShowDialog();
        }

        private void reviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (table == "cars")
            {
                using (MySqlConnection conn = new MySqlConnection(db.connect))
                {
                    try
                    {
                        conn.Open();

                        string checkQuery = "SELECT COUNT(*) FROM reviews WHERE car_id = @carId;";
                        MySqlCommand cmd = new MySqlCommand(checkQuery, conn);
                        cmd.Parameters.AddWithValue("@carId", helper.selectedCarId);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 0)
                        {
                            MessageBox.Show("Для этой машины ещё нет отзывов.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        var carReview = new fullInfo.carReview(helper.selectedCarId);
                        carReview.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при проверке отзывов: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void fullInfo_Click(object sender, EventArgs e)
        {
            if (table == "customers")
            {
                var fullCustomer = new fullInfo.fullInfoCustomers(helper.selectedCustomerId);
                fullCustomer.ShowDialog();
            }
            else
            {
                var fullRental = new fullInfo.fullInfoRentals(helper.selectedRentalId);
                fullRental.ShowDialog();
            }

        }
        private void finesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (table == "customers")
            {
                var fullInfoFines = new fullInfo.fullInfoFines(helper.selectedCustomerId);
                fullInfoFines.ShowDialog();
            }
            else if (table == "rentals")
            {
                using (MySqlConnection conn = new MySqlConnection(db.connect))
                {
                    conn.Open();
                    string dateQuery = "SELECT return_date FROM rentals WHERE rental_id = @rentalId;";
                    MySqlCommand dateCmd = new MySqlCommand(dateQuery, conn);
                    dateCmd.Parameters.AddWithValue("@rentalId", helper.selectedRentalId);
                    object endDateObj = dateCmd.ExecuteScalar();
                    DateTime endDate = Convert.ToDateTime(endDateObj);
                    if (endDate > DateTime.Now)
                    {
                        MessageBox.Show("Штраф можно оставить только после окончания аренды.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    var fullInfoFinesRental = new fullInfo.fullInfoFinesRental(helper.selectedRentalId);
                    fullInfoFinesRental.ShowDialog();
                }
            }
        }

        private void отзывыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (table == "customers")
            {
                if (helper.selectedCustomerId == -1)
                {
                    MessageBox.Show("Пожалуйста, выберите клиента.");
                    return;
                }
                using (MySqlConnection conn = new MySqlConnection(db.connect))
                {
                    try
                    {
                        conn.Open();

                        string checkQuery = "SELECT COUNT(*) FROM reviews WHERE customer_id = @customerId;";
                        MySqlCommand cmd = new MySqlCommand(checkQuery, conn);
                        cmd.Parameters.AddWithValue("@customerId", helper.selectedCustomerId);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 0)
                        {
                            MessageBox.Show("У этого клиента нет отзывов.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        var customerReviewsForm = new fullInfo.fullInfoCustomerReviews(helper.selectedCustomerId);
                        customerReviewsForm.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при проверке отзывов: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (table == "rentals")
            {
                if (helper.selectedRentalId == -1)
                {
                    MessageBox.Show("Пожалуйста, выберите аренду.");
                    return;
                }

                using (MySqlConnection conn = new MySqlConnection(db.connect))
                {
                    try
                    {
                        conn.Open();
                        string dateQuery = "SELECT return_date FROM rentals WHERE rental_id = @rentalId;";
                        MySqlCommand dateCmd = new MySqlCommand(dateQuery, conn);
                        dateCmd.Parameters.AddWithValue("@rentalId", helper.selectedRentalId);
                        object endDateObj = dateCmd.ExecuteScalar();

                        if (endDateObj == null)
                        {
                            MessageBox.Show("Аренда не найдена.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        DateTime endDate = Convert.ToDateTime(endDateObj);
                        if (endDate > DateTime.Now)
                        {
                            MessageBox.Show("Отзыв можно оставить только после окончания аренды.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        string reviewCheckQuery = "SELECT COUNT(*) FROM reviews WHERE rental_id = @rentalId;";
                        MySqlCommand reviewCheckCmd = new MySqlCommand(reviewCheckQuery, conn);
                        reviewCheckCmd.Parameters.AddWithValue("@rentalId", helper.selectedRentalId);
                        int reviewCount = Convert.ToInt32(reviewCheckCmd.ExecuteScalar());

                        if (reviewCount > 0)
                        {
                            MessageBox.Show("Отзыв на эту аренду уже оставлен.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        var addReview = new add.addReview(helper.selectedRentalId, helper.selectedCarId);
                        addReview.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при проверке аренды или отзывов: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void ResetContextMenu()
        {
            if (table == "cars")
            {
                dataGridView1.ContextMenuStrip = contextMenuStrip1;
            }
            else if (table == "customers" || table == "rentals")
            {
                dataGridView1.ContextMenuStrip = contextMenuStrip2;
            }
            else
            {
                dataGridView1.ContextMenuStrip = null;
            }
        }


        private bool CheckReviewExists(int rentalId)
        {
            using (var conn = new MySqlConnection(db.connect))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT COUNT(*) FROM reviews WHERE rental_id = @rentalId", conn);
                cmd.Parameters.AddWithValue("@rentalId", rentalId);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
        private int GetCarIdFromRental(int rentalId)
        {
            using (var conn = new MySqlConnection(db.connect))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT car_id FROM rentals WHERE rental_id = @rentalId", conn);
                cmd.Parameters.AddWithValue("@rentalId", rentalId);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }
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

        private void referenceBtn_Click(object sender, EventArgs e)
        {
            fullInfoReference fullInfoReference = new fullInfoReference();
            fullInfoReference.ShowDialog();
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

        private void ReferenceOnOffBtn_Click(object sender, EventArgs e)
        {
            isHighlightEnabled = !isHighlightEnabled;
            if (isHighlightEnabled)
            {
                ReferenceOnOffBtn.Text = "Отключить подсветку";
                ApplyHighlighting();
            }
            else
            {
                ReferenceOnOffBtn.Text = "Включить подсветку";
                ClearHighlighting();
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
        }

        private void chkFreeOnly_CheckedChanged(object sender, EventArgs e)
        {
            LoadData(); 
        }
    }
}
