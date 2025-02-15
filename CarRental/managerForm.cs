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
using Word = Microsoft.Office.Interop.Word;

namespace CarRental
{
    public partial class managerForm : Form
    {
        private db db;
        private static string table = string.Empty;
        private DataGridViewRow selectedRow;
        public managerForm(string labelLog)
        {
            db = new db();
            InitializeComponent();
            this.label1.Text = $"{labelLog}";
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.CellClick += dataGridView1_CellClick;
        }
        private void SetButtonVisibility(bool btn7, bool btn8, bool btn9)
        {
            addBtn.Visible = btn7;
            editBtn.Visible = btn8;
            checkBtn.Visible = btn9;
        }

        private void SetButtonColors(Button activeButton, Button other1, Button other2)
        {
            activeButton.BackColor = Color.FromArgb(92, 96, 255);
            activeButton.ForeColor = Color.FromArgb(34, 36, 49);
            other1.BackColor = Color.FromArgb(34, 36, 49);
            other1.ForeColor = Color.FromArgb(92, 96, 255);
            other2.BackColor = Color.FromArgb(34, 36, 49);
            other2.ForeColor = Color.FromArgb(92, 96, 255);
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
        private void LoadData()
        {
            using (MySqlConnection connection = new MySqlConnection(db.connect))
            {
                connection.Open();
                string query = "";
                if (table == "customers")
                {
                    query = "SELECT first_name as 'Имя', last_name as 'Фамилия', phone as 'Телефон', driver_license as 'Вод.Удостоверение', passport as 'Паспорт' FROM customers";
                }
                else if (table == "cars")
                {
                    query = "SELECT make as 'Марка', model as 'Модель', year as 'Год выпуска', license_plate as 'Гос.Номер', status as 'Статус' , price 'Цена за сутки' FROM cars";
                }else if (table == "rentals")
                {
                    query = "Select make as 'Марка', model as 'Модель', first_name as 'Имя', last_name as 'Фамилия', phone as 'Телефон', rental_date as 'Дата взятия', return_date as 'Дата возврата', total_amount as 'Сумма' FROM carrentaldb.rentals inner join customers on rentals.customer_id = customers.customer_id inner join cars on cars.car_id = rentals.car_id; ";
                }
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
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
            SetButtonColors(carBtn, rentalBtn, customerBtn);
            LoadTable("cars", "Машины", new string[] { "По Марке", "По Модели", "По Году выпуска", "По Гос.Номеру", "По Статусу", "По Цене" });
        }

        private void SortDataGridViewAscending(string tableName, int selectedIndex)
        {
            Dictionary<string, string[]> sortingColumns = new Dictionary<string, string[]>
    {
        { "cars", new string[] { "Марка", "Модель", "Год выпуска", "Гос.Номер", "Статус", "Цена" } },
        { "customers", new string[] { "Имя", "Фамилия", "Телефон", "Вод.Удостоверение", "Паспорт" } },
        { "rentals", new string[] { "Марка", "Модель", "Имя", "Фамилия", "Телефон", "Дата взятия", "Дата возврата", "Сумма" } }
    };

            if (sortingColumns.ContainsKey(tableName) && selectedIndex >= 0 && selectedIndex < sortingColumns[tableName].Length)
            {
                dataGridView1.Sort(dataGridView1.Columns[sortingColumns[tableName][selectedIndex]], System.ComponentModel.ListSortDirection.Ascending);
            }
        }

        private void SortDataGridViewDescending(string tableName, int selectedIndex)
        {
            Dictionary<string, string[]> sortingColumns = new Dictionary<string, string[]>
    {
        { "cars", new string[] { "Марка", "Модель", "Год выпуска", "Гос.Номер", "Статус", "Цена" } },
        { "customers", new string[] { "Имя", "Фамилия", "Телефон", "Вод.Удостоверение", "Паспорт" } },
        { "rentals", new string[] { "Марка", "Модель", "Имя", "Фамилия", "Телефон", "Дата взятия", "Дата возврата", "Сумма" } }
    };

            if (sortingColumns.ContainsKey(tableName) && selectedIndex >= 0 && selectedIndex < sortingColumns[tableName].Length)
            {
                dataGridView1.Sort(dataGridView1.Columns[sortingColumns[tableName][selectedIndex]], System.ComponentModel.ListSortDirection.Descending);
            }
        }
        private void CreateWordReport(DataGridViewRow row)
        {
            string templatePath = Directory.GetCurrentDirectory() + @"\template\template.docx";
            Word.Application wordApp = new Word.Application();
            Word.Document doc = wordApp.Documents.Add(templatePath);

            try
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    string bookmarkName = cell.OwningColumn.HeaderText.Replace(" ", "_");
                    if (doc.Bookmarks.Exists(bookmarkName))
                    {
                        doc.Bookmarks[bookmarkName].Range.Text = cell.Value.ToString();
                    }
                }

                wordApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                doc = null;
                wordApp = null;
            }
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
            SetButtonColors(rentalBtn, customerBtn, carBtn);
            LoadTable("rentals", "Аренды", new string[] { "По Имени", "По Фамилии", "По Телефону", "По марке", "По моделе", "По дате взятия", "По дате возврата", "По сумме" });
        }

        private void customerBtn_Click(object sender, EventArgs e)
        {
            SetButtonVisibility(true, true, false);
            SetButtonColors(customerBtn, rentalBtn, carBtn);
            LoadTable("customers", "Клиенты", new string[] { "По Имени", "По Фамилии", "По Телефону", "По Вод.Удостоверению", "По Паспорту" });
        }

        private void carBtn_Click(object sender, EventArgs e)
        {
            SetButtonVisibility(false, false, false);
            SetButtonColors(carBtn, rentalBtn, customerBtn);
            LoadTable("cars", "Машины", new string[] { "По Марке", "По Модели", "По Году выпуска", "По Гос.Номеру", "По Статусу", "По Цене" });
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            string txt = searchBox.Text;
            string query = "";
            if (table == "cars") { query = $"SELECT make as 'Марка', model as 'Модель', year as 'Год выпуска', license_plate as 'Гос.Номер', status as 'Статус', price 'Цена за сутки' FROM cars WHERE make LIKE '%{txt}%' OR model LIKE '%{txt}%' OR license_plate LIKE '%{txt}%' OR status LIKE '%{txt}%' OR year LIKE '%{txt}%' OR price LIKE '%{txt}%'"; }
            else if (table == "customers") { query = $"SELECT first_name as 'Имя', last_name as 'Фамилия', phone as 'Телефон', driver_license as 'Вод.Удостоверение', passport as 'Паспорт' FROM customers WHERE first_name LIKE '%{txt}%' OR last_name LIKE '%{txt}%' OR phone LIKE '%{txt}%' OR driver_license LIKE '%{txt}%' OR passport LIKE '%{txt}%'"; }
            else if (table == "rentals") { query = $"SELECT customers.passport as 'Клиент', cars.license_plate as 'Машина', rentals.rental_date as 'Дата взятия', employee.employeeLogin as 'Менеджер',rentals.return_date as 'Дата возвращения', rentals.total_amount as 'Сумма' FROM rentals JOIN customers ON rentals.customer_id = customers.customer_id JOIN cars ON rentals.car_id = cars.car_id JOIN employee ON rentals.employee_id = employee.employee_id;"; }
            db.MySqlReturnData(query, dataGridView1);
        }

        private void ascendingBtn_Click(object sender, EventArgs e)
        {
            SortDataGridViewAscending(table, comboBox1.SelectedIndex);
        }

        private void descendingBtn_Click(object sender, EventArgs e)
        {
            SortDataGridViewDescending(table, comboBox1.SelectedIndex);
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (table == "customers")
            {
                addCustomer addCustomer = new addCustomer();
                addCustomer.ShowDialog();
            }
            else if (table == "rentals")
            {
                addRental addRental = new addRental();
                addRental.ShowDialog();
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
                else
                {
                    DataRow selectedRows = ((DataRowView)dataGridView1.SelectedRows[0].DataBoundItem).Row;
                    editRental editRental = new editRental(selectedRows);
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
                CreateWordReport(selectedRow);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку.");
            }
        }
    }
}
