using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace CarRental
{
    public partial class managerForm : Form
    {
        private db db;
        private helper helper;
        private static string table = string.Empty;
        private DataGridViewRow selectedRow;
        private int selectedCarId = -1;
        private int selectedCustomerId = -1;
        private int selectedRentalId = -1;
        public managerForm(string labelLog)
        {
            db = new db();
            helper = new helper();
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

        private void UpdateComboBox(params string[] items)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(items);
        }

        private void LoadTable(string tableName, string labelText, string[] comboBoxItems)
        {
            label2.Text = labelText;
            searchBox.Text = "Поиск";
            table = tableName;
            UpdateComboBox(comboBoxItems);
            LoadData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
        }

        private void  LoadData()
        {
            using (MySqlConnection connection = new MySqlConnection(db.connect))
            {
                connection.Open();
                string query = string.Empty;
                if (table == "customers")
                {
                    query = "SELECT customer_id, first_name AS 'Имя', last_name AS 'Фамилия', CONCAT(LEFT(phone, 2), REPEAT('*', CHAR_LENGTH(phone) - 6), RIGHT(phone, 4)) AS 'Телефон', CONCAT(LEFT(driver_license, 2), REPEAT('*', CHAR_LENGTH(driver_license) - 6), RIGHT(driver_license, 4)) AS 'Вод.Удостоверение', CONCAT(LEFT(passport, 2), REPEAT('*', CHAR_LENGTH(passport) - 6), RIGHT(passport, 4)) AS 'Паспорт' FROM customers;";
                }
                else if (table == "cars")
                {
                    query = "SELECT car_id, make as 'Марка', model as 'Модель', status as 'Статус', price as 'Цена за сутки' FROM cars";
                }
                else if (table == "rentals")
                {
                    query = "SELECT rental_id, make as 'Марка', model as 'Модель', rental_date as 'Дата взятия', return_date as 'Дата возврата', total_amount as 'Сумма' FROM carrentaldb.rentals " +
                            "INNER JOIN cars ON cars.car_id = rentals.car_id;";
                }
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
                dataGridView1.Columns[0].Visible = false;
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
                Application.Exit();
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
            LoadTable("cars", "Машины", new string[] { "По Марке", "По Модели", "По Году выпуска", "По Гос.Номеру", "По Статусу", "По Цене" });
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
            SetButtonVisibility(true, true, true);
            helper.SetButtonColors(rentalBtn, customerBtn, carBtn);
            LoadTable("rentals", "Аренды", new string[] { "По Марке", "По Модели", "По Имени", "По Фамилии", "По Телефону", "По дате взятия", "По дате возврата", "По сумме" });
        }

        private void customerBtn_Click(object sender, EventArgs e)
        {
            SetButtonVisibility(true, true, false);
            helper.SetButtonColors(customerBtn, rentalBtn, carBtn);
            LoadTable("customers", "Клиенты", new string[] { "По Имени", "По Фамилии", "По Телефону", "По Вод.Удостоверению", "По Паспорту" });
        }

        private void carBtn_Click(object sender, EventArgs e)
        {
            SetButtonVisibility(false, false, false);
            helper.SetButtonColors(carBtn, rentalBtn, customerBtn);
            LoadTable("cars", "Машины", new string[] { "По Марке", "По Модели", "По Году выпуска", "По Гос.Номеру", "По Статусу", "По Цене" });
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            string txt = searchBox.Text;
            string query = string.Empty;
            if (table == "cars")
            {
                query = $"SELECT make as 'Марка', model as 'Модель', year as 'Год выпуска', license_plate as 'Гос.Номер', status as 'Статус', price as 'Цена за сутки' FROM cars WHERE make LIKE '%{txt}%' OR model LIKE '%{txt}%' OR license_plate LIKE '%{txt}%' OR status LIKE '%{txt}%' OR year LIKE '%{txt}%' OR price LIKE '%{txt}%'";
            }
            else if (table == "customers")
            {
                query = $"SELECT first_name as 'Имя', last_name as 'Фамилия', phone as 'Телефон', driver_license as 'Вод.Удостоверение', passport as 'Паспорт' FROM customers WHERE first_name LIKE '%{txt}%' OR last_name LIKE '%{txt}%' OR phone LIKE '%{txt}%' OR driver_license LIKE '%{txt}%' OR passport LIKE '%{txt}%'";
            }
            else if (table == "rentals")
            {
                query = $"SELECT customers.passport as 'Клиент', cars.license_plate as 'Машина', rentals.rental_date as 'Дата взятия', employee.employeeLogin as 'Менеджер', rentals.return_date as 'Дата возвращения', rentals.total_amount as 'Сумма' FROM rentals " +
                        $"JOIN customers ON rentals.customer_id = customers.customer_id " +
                        $"JOIN cars ON rentals.car_id = cars.car_id " +
                        $"JOIN employee ON rentals.employee_id = employee.employee_id;";
            }
            db.MySqlReturnData(query, dataGridView1);
        }

        private void ascendingBtn_Click(object sender, EventArgs e)
        {
            helper.SortDataGridViewAscending(table, comboBox1.SelectedIndex, dataGridView1);
        }

        private void descendingBtn_Click(object sender, EventArgs e)
        {
            helper.SortDataGridViewDescending(table, comboBox1.SelectedIndex, dataGridView1);
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
                else
                {
                    var selectedRows = ((DataRowView)dataGridView1.SelectedRows[0].DataBoundItem).Row;
                    var editRental = new editRental(selectedRows);
                    if (editRental.ShowDialog() == DialogResult.OK)
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
                if (table == "cars")
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[e.RowIndex].Selected = true;

                    selectedCarId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["car_id"].Value);
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (table == "customers")
                {
                    contextMenuStrip1.Hide();
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[e.RowIndex].Selected = true;

                    selectedCustomerId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["customer_id"].Value);
                    contextMenuStrip2.Show(Cursor.Position);
                }
                else if (table == "rentals")
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[e.RowIndex].Selected = true;

                    selectedRentalId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["rental_id"].Value);
                    selectedCarId = GetCarIdFromRental(selectedRentalId);
                    object returnDateObj = dataGridView1.Rows[e.RowIndex].Cells["Дата возврата"].Value;
                    bool isCompleted = returnDateObj != DBNull.Value && returnDateObj != null;

                    bool hasReview = CheckReviewExists(selectedRentalId);

                    reviewToolStripMenuItem.Visible = isCompleted && !hasReview;
                    reviewToolStripMenuItem.Text = "Добавить отзыв";

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
            var carInfo = new fullInfoCar.fullInfoCar(selectedCarId);
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
                        cmd.Parameters.AddWithValue("@carId", selectedCarId);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 0)
                        {
                            MessageBox.Show("Для этой машины ещё нет отзывов.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        var carReview = new fullInfo.carReview(selectedCarId);
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
                var fullCustomer = new fullInfo.fullInfoCustomers(selectedCustomerId);
                fullCustomer.ShowDialog();
            }
            else
            {
                var fullRental = new fullInfo.fullInfoRentals(selectedRentalId);
                fullRental.ShowDialog();
            }

        }
        private void finesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (table == "customers")
            {
                var fullInfoFines = new fullInfo.fullInfoFines(selectedCustomerId);
                fullInfoFines.ShowDialog();
            }
            else if (table == "rentals")
            {
                using (MySqlConnection conn = new MySqlConnection(db.connect))
                {
                    conn.Open();
                    string dateQuery = "SELECT return_date FROM rentals WHERE rental_id = @rentalId;";
                    MySqlCommand dateCmd = new MySqlCommand(dateQuery, conn);
                    dateCmd.Parameters.AddWithValue("@rentalId", selectedRentalId);
                    object endDateObj = dateCmd.ExecuteScalar();
                    DateTime endDate = Convert.ToDateTime(endDateObj);
                    if (endDate > DateTime.Now)
                    {
                        MessageBox.Show("Штраф можно оставить только после окончания аренды.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    var fullInfoFinesRental = new fullInfo.fullInfoFinesRental(selectedRentalId);
                    fullInfoFinesRental.ShowDialog();
                }
            }
        }

        private void отзывыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (table == "customers")
            {
                if (selectedCustomerId == -1)
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
                        cmd.Parameters.AddWithValue("@customerId", selectedCustomerId);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 0)
                        {
                            MessageBox.Show("У этого клиента нет отзывов.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        var customerReviewsForm = new fullInfo.fullInfoCustomerReviews(selectedCustomerId);
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
                if (selectedRentalId == -1)
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
                        dateCmd.Parameters.AddWithValue("@rentalId", selectedRentalId);
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
                        reviewCheckCmd.Parameters.AddWithValue("@rentalId", selectedRentalId);
                        int reviewCount = Convert.ToInt32(reviewCheckCmd.ExecuteScalar());

                        if (reviewCount > 0)
                        {
                            MessageBox.Show("Отзыв на эту аренду уже оставлен.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        var addReview = new add.addReview(selectedRentalId, selectedCarId);
                        addReview.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при проверке аренды или отзывов: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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
    }
}
