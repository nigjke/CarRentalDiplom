using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.fullInfo
{
    public partial class fullInfoEmployee: Form
    {
        private int employeeId;
        private bool _isClosing;
        public fullInfoEmployee(int employeeId)
        {
            InitializeComponent();
            this.employeeId = employeeId;
            this.Load += (s, e) => LoadEmployeeDetails(); 
            this.FormClosing += (s, e) => _isClosing = true;
        }
        private void LoadEmployeeDetails()
        {
            if (this.IsDisposed || !this.IsHandleCreated)
                return;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ScrollBars = ScrollBars.None;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowTemplate.Height = 100;
            dataGridView1.Height = dataGridView1.RowTemplate.Height + dataGridView1.ColumnHeadersHeight;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(db.connect))
                {
                    string query = @"
                        SELECT 
                            first_name as 'Имя', 
                            last_name as 'Фамилия', 
                            phone as 'Телефон', 
                            email as 'Почта', 
                            employeeLogin as 'Логин', 
                            employeePass as 'Пароль' 
                        FROM employee
                        WHERE employee_id = @employeeId";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@employeeId", employeeId);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (!this.IsDisposed && this.IsHandleCreated)
                    {
                        dataGridView1.Invoke((MethodInvoker)delegate
                        {
                            dataGridView1.DataSource = dt;
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                if (!_isClosing && !this.IsDisposed && this.IsHandleCreated)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                    this.Close();
                }
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            _isClosing = true;
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
