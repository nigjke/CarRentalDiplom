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
        private int currentPage = 1;
        private int totalRecords = 0;
        private db db;
        private helper helper;

        private static string table = string.Empty;
        int pageSize = 20;
        private DataTable customersTable;
        private DataGridViewRow selectedRow;
        private Timer inactivityTimer;
        private int inactivityTime;
        private DateTime lastActivityTime;
        private int selectedCarId = -1;
        private int selectedEmployeeId = -1;
        private bool isFullScreen = false;
        private FormWindowState previousWindowState;
        private Rectangle previousBounds;
        private float originalWidth;
        private float originalHeight;
        private Size originalFormSize;
        private Size referenceSize = new Size(1322, 949);
        private Dictionary<Control, Rectangle> controlRatios = new Dictionary<Control, Rectangle>();

        public adminForm(string labelLog)
        {
            InitializeComponent();
            helper = new helper();
            db = new db();
            this.label1.Text = $"{labelLog}";
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            InitializeInactivityTimer();
            dataGridView1.MultiSelect = false;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            dataGridView1.CellMouseDown += dataGridView1_CellMouseDown;
            originalWidth = this.Width;
            originalHeight = this.Height;
            InitControlRatios();
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
            helper.SetButtonColors(rentalBtn, customerBtn, carBtn, employeeBtn);
            LoadTable("rentals", "Аренды", new string[] { "По Марке", "По Модели", "По Имени", "По Фамилии", "По Телефону", "По дате взятия", "По дате возврата", "По сумме" });
        }
        private string MaskData(string data, int visibleDigits = 2)
        {
            if (data.Length > visibleDigits)
            {
                return new string('*', data.Length - visibleDigits) + data.Substring(data.Length - visibleDigits);
            }
            return data;
        }
        private void LoadMaskedData()
        {
            int offset = 0;
            using (MySqlConnection connection = new MySqlConnection(db.connect))
            {
                connection.Open();
                offset = (currentPage - 1) * pageSize;
                string query = $"SELECT first_name as 'Имя', last_name as 'Фамилия', phone as 'Телефон', driver_license as 'Вод.Удостоверение', passport as 'Паспорт' FROM customers LIMIT {(currentPage - 1) * pageSize}, {pageSize}";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                customersTable = new DataTable();
                adapter.Fill(customersTable);
                foreach (DataRow row in customersTable.Rows)
                {
                    row["Телефон"] = MaskData(row["Телефон"].ToString());
                    row["Вод.Удостоверение"] = MaskData(row["Вод.Удостоверение"].ToString());
                    row["Паспорт"] = MaskData(row["Паспорт"].ToString());
                }
                dataGridView1.DataSource = customersTable;
                string countQuery = "SELECT COUNT(*) FROM customers";
                MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                totalRecords = Convert.ToInt32(countCommand.ExecuteScalar());
                connection.Close();
                labelInfo.Text = $"{currentPage} - {offset + customersTable.Rows.Count} из {totalRecords}";
                pictureBox2.Enabled = currentPage > 1;
                pictureBox3.Enabled = currentPage * pageSize < totalRecords;
            }
        }
        private void LoadData()
        {
            using (MySqlConnection connection = new MySqlConnection(db.connect))
            {
                connection.Open();
                string query = "";
                int pageSize = 20;
                int offset = 0;
                if (table == "customers")
                {
                    offset = (currentPage - 1) * pageSize;
                    MySqlCommand counter = new MySqlCommand("SELECT COUNT(*) FROM customers", connection);
                    totalRecords = Convert.ToInt32(counter.ExecuteScalar());
                    query = $"SELECT customer_id, first_name as 'Имя', last_name as 'Фамилия', phone as 'Телефон', driver_license as 'Вод.Удостоверение', passport as 'Паспорт' FROM customers LIMIT {pageSize} OFFSET {offset}";
                }
                else if (table == "cars")
                {
                    offset = (currentPage - 1) * pageSize;
                    MySqlCommand counter = new MySqlCommand("SELECT COUNT(*) FROM cars", connection);
                    totalRecords = Convert.ToInt32(counter.ExecuteScalar());
                    query = $"SELECT car_id, make as 'Марка', model as 'Модель', year as 'Год выпуска', license_plate as 'Гос.Номер', status as 'Статус' , price 'Цена за сутки' FROM cars LIMIT {pageSize} OFFSET {offset}";
                }
                else if (table == "employee")
                {
                    offset = (currentPage - 1) * pageSize;
                    MySqlCommand counter = new MySqlCommand("SELECT COUNT(*) FROM employee", connection);
                    totalRecords = Convert.ToInt32(counter.ExecuteScalar());
                    query = $"SELECT employee_id, employee.first_name as 'Имя', employee.last_name as 'Фамилия', employee.phone as 'Телефон', role.name as 'Роль', employee.employeeLogin as 'Логин', employee.employeePass as 'Пароль' FROM carrentaldb.employee JOIN role ON employee.Role_id=role.Role_id LIMIT {pageSize} OFFSET {offset}";
                }
                else
                {
                    offset = (currentPage - 1) * pageSize;
                    MySqlCommand counter = new MySqlCommand("SELECT COUNT(*) FROM rentals", connection);
                    totalRecords = Convert.ToInt32(counter.ExecuteScalar());
                    query = $@"
                SELECT 
                    r.car_id, 
                    c.make as 'Марка', 
                    c.model as 'Модель', 
                    cust.first_name as 'Имя', 
                    cust.last_name as 'Фамилия', 
                    cust.phone as 'Телефон', 
                    r.rental_date as 'Дата взятия', 
                    r.return_date as 'Дата возврата', 
                    r.total_amount as 'Сумма' 
                FROM 
                    carrentaldb.rentals r
                    INNER JOIN customers cust ON r.customer_id = cust.customer_id
                    INNER JOIN cars c ON c.car_id = r.car_id 
                LIMIT {pageSize} OFFSET {offset}";
                }
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
                labelInfo.Text = $"{currentPage} - {offset + dataTable.Rows.Count} из {totalRecords}";
                pictureBox2.Enabled = currentPage > 1;
                pictureBox3.Enabled = currentPage * pageSize < totalRecords;
                dataGridView1.Columns[0].Visible = false;
            }
        }
        private void LoadTable(string tableName, string labelText, string[] comboBoxItems)
        {
            label2.Text = labelText;
            searchBox.Text = "Поиск";
            table = tableName;
            UpdateComboBox(comboBoxItems);
            LoadData();
        }

        // Utils Functions
        private void ShowFullInfo(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                using (MySqlConnection connection = new MySqlConnection(db.connect))
                {
                    int selectedIndex = dataGridView1.SelectedRows[0].Index;
                    DataRow selectedRow = customersTable.Rows[selectedIndex];
                    string first_name = selectedRow["Имя"].ToString();
                    string last_name = selectedRow["Фамилия"].ToString();
                    string query = $"SELECT first_name as 'Имя', last_name as 'Фамилия', phone as 'Телефон', driver_license as 'Вод.Удостоверение', passport as 'Паспорт' FROM customers WHERE first_name = '{first_name}' AND last_name = '{last_name}'";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable fullInfoTable = new DataTable();
                    adapter.Fill(fullInfoTable);
                    fullInformation fullInfoForm = new fullInformation(fullInfoTable);
                    fullInfoForm.Show();
                }
            }
        }
        private void UpdateComboBox(params string[] items)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(items);
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

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && table == "customers")
            {
                ContextMenuStrip menu = new ContextMenuStrip();
                menu.Items.Add("Открыть дополнительную информацию", null, ShowFullInfo);
                menu.Show(dataGridView1, new System.Drawing.Point(e.X, e.Y));
            }
        }
        // Paginations
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (currentPage > 0)
            {
                currentPage--;
                if (table == "customers")
                {
                    LoadMaskedData();
                }
                else
                {
                    LoadData();
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (currentPage * pageSize < totalRecords)
            {
                currentPage++;
                if (table == "customers")
                {
                    LoadMaskedData();
                }
                else
                {
                    LoadData();
                }
            }
        }
        // Menu Buttons
        private void carBtn_Click(object sender, EventArgs e)
        {
            SetButtonVisibility(true, true, true);
            helper.SetButtonColors(carBtn, rentalBtn, customerBtn, employeeBtn);
            LoadTable("cars", "Машины", new string[] { "По Марке", "По Модели", "По Году выпуска", "По Гос.Номеру", "По Статусу", "По Цене" });
        }

        private void employeeBtn_Click(object sender, EventArgs e)
        {
            SetButtonVisibility(false, false, false);
            helper.SetButtonColors(employeeBtn, rentalBtn, carBtn, customerBtn);
            LoadTable("employee", "Сотрудники", new string[] { "По Имени", "По Фамилии", "По Телефону", "По Вод.Удостоверению", "По Паспорту" });
        }

        private void customerBtn_Click(object sender, EventArgs e)
        {
            SetButtonVisibility(true, true, false);
            helper.SetButtonColors(customerBtn, rentalBtn, carBtn, employeeBtn);
            LoadTable("customers", "Клиенты", new string[] { "По Имени", "По Фамилии", "По Телефону", "По Вод.Удостоверению", "По Паспорту" });
        }

        private void rentalBtn_Click(object sender, EventArgs e)
        {
            SetButtonVisibility(true, true, true);
            helper.SetButtonColors(rentalBtn, customerBtn, carBtn, employeeBtn);
            LoadTable("rentals", "Аренды", new string[] { "По Марке", "По Модели", "По Имени", "По Фамилии", "По Телефону", "По дате взятия", "По дате возврата", "По сумме" });
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
            helper.SortDataGridViewAscending(table, comboBox1.SelectedIndex, dataGridView1);
        }

        private void descendingBtn_Click(object sender, EventArgs e)
        {
            helper.SortDataGridViewDescending(table, comboBox1.SelectedIndex, dataGridView1);
        }

        // Event Buttons
        private void addBtn_Click(object sender, EventArgs e)
        {
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
                MessageBox.Show("Нет прав доступа!");
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
            string txt = searchBox.Text;
            string query = "";
            if (table == "cars") { query = $"SELECT make as 'Марка', model as 'Модель', year as 'Год выпуска', license_plate as 'Гос.Номер', status as 'Статус', price 'Цена за сутки' FROM cars WHERE make LIKE '%{txt}%' OR model LIKE '%{txt}%' OR license_plate LIKE '%{txt}%' OR status LIKE '%{txt}%' OR year LIKE '%{txt}%' OR price LIKE '%{txt}%'"; }
            else if (table == "customers") { query = $"SELECT first_name as 'Имя', last_name as 'Фамилия', phone as 'Телефон', driver_license as 'Вод.Удостоверение', passport as 'Паспорт' FROM customers WHERE first_name LIKE '%{txt}%' OR last_name LIKE '%{txt}%' OR phone LIKE '%{txt}%' OR driver_license LIKE '%{txt}%' OR passport LIKE '%{txt}%'"; }
            else if (table == "employee") { query = $"SELECT first_name as 'Имя', last_name as 'Фамилия', phone as 'Телефон', Role_id as 'Роль', employeeLogin as 'Логин', employeePass as 'Пароль' FROM employee WHERE first_name LIKE '%{txt}%' OR LIKE '%{txt}%' OR phone LIKE '%{txt}%' OR employeeLogin LIKE '%{txt}%' OR employeePass LIKE '%{txt}%' OR Role_id LIKE '%{txt}%'"; }
            else if (table == "rentals") { query = $"SELECT customers.passport as 'Клиент', cars.license_plate as 'Машина', rentals.rental_date as 'Дата взятия', employee.employeeLogin as 'Менеджер',rentals.return_date as 'Дата возвращения', rentals.total_amount as 'Сумма' FROM rentals JOIN customers ON rentals.customer_id = customers.customer_id JOIN cars ON rentals.car_id = cars.car_id JOIN employee ON rentals.employee_id = employee.employee_id;"; }
            db.MySqlReturnData(query, dataGridView1);
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
            if (table == "customers")
            {
                var carInfo = new fullInfoCar.fullInfoCar(selectedCarId);
                carInfo.ShowDialog();
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
                    selectedCarId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["car_id"].Value);
                }
                if (table == "employee")
                {
                    selectedEmployeeId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["employee_id"].Value);
                }
                    contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            ToggleFullScreen();
        }
        private void ToggleFullScreen()
        {
            if (isFullScreen)
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = previousWindowState;
                this.Bounds = previousBounds;

                referenceSize = this.ClientSize;
            }
            else
            {
                previousWindowState = this.WindowState;
                previousBounds = this.Bounds;

                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Normal;
                this.Bounds = Screen.PrimaryScreen.Bounds;

                referenceSize = this.ClientSize;
            }
            isFullScreen = !isFullScreen;
            ScaleControls();
        }
        private void InitControlRatios()
        {
            foreach (Control control in GetAllControls(this))
            {
                controlRatios[control] = new Rectangle(
                    control.Left, control.Top,
                    control.Width, control.Height
                );
            }
        }

        // Получение всех контролов
        private IEnumerable<Control> GetAllControls(Control container)
        {
            foreach (Control control in container.Controls)
            {
                yield return control;
                foreach (var child in GetAllControls(control))
                {
                    yield return child;
                }
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ScaleControls();
            UpdateMargins();
            if (originalWidth == 0 || originalHeight == 0) return;

            float widthRatio = this.Width / originalWidth;
            float heightRatio = this.Height / originalHeight;

            foreach (var entry in controlRatios)
            {
                Control control = entry.Key;
                Rectangle ratio = entry.Value;

                if (control is DataGridView) continue;

                int newX = (int)(ratio.X * widthRatio);
                int newY = (int)(ratio.Y * heightRatio);
                int newWidth = (int)(ratio.Width * widthRatio);
                int newHeight = (int)(ratio.Height * heightRatio);

                control.Location = new Point(newX, newY);
                control.Size = new Size(newWidth, newHeight);
            }

            // Особые настройки для DataGridView
            dataGridView1.Width = (int)(this.Width * 0.7f);
            dataGridView1.Height = (int)(this.Height * 0.6f);
        }
        private void UpdateMargins()
        {
            int baseMargin = (int)(20 * (this.Width / (float)originalFormSize.Width));
            label2.Margin = new Padding(baseMargin * 2, baseMargin, 0, 0);
            dataGridView1.Margin = new Padding(baseMargin, baseMargin * 4, baseMargin, baseMargin * 3);
        }


        private void ScaleControls()
        {
            float scaleX = (float)ClientSize.Width / referenceSize.Width;
            float scaleY = (float)ClientSize.Height / referenceSize.Height;

            ScaleControl(comboBox1, scaleX, scaleY);
            ScaleControl(ascendingBtn, scaleX, scaleY);
            ScaleControl(descendingBtn, scaleX, scaleY);
            ScaleControl(label2, scaleX, scaleY);
            ScaleControl(dataGridView1, scaleX, scaleY);
        }

        private void ScaleControl(Control control, float scaleX, float scaleY)
        {
            // Позиция
            int newX = (int)(control.Location.X * scaleX);
            int newY = (int)(control.Location.Y * scaleY);

            // Размер
            int newWidth = (int)(control.Width * scaleX);
            int newHeight = (int)(control.Height * scaleY);

            // Шрифт
            float fontSize = control.Font.Size * Math.Min(scaleX, scaleY);

            control.Location = new Point(newX, newY);
            control.Size = new Size(newWidth, newHeight);
            control.Font = new Font(control.Font.FontFamily, fontSize);
        }
    }
}
