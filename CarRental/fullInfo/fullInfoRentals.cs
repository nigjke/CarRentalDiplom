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
    public partial class fullInfoRentals: Form
    {
        private int _rentaId;
        private bool _isClosing;
        public fullInfoRentals(int rentalId)
        {
            InitializeComponent();
            _rentaId = rentalId;
            this.FormClosing += (s, e) => _isClosing = true;
            LoadData(_rentaId);
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            _isClosing = true;
            this.Close();
        }
        private void LoadData(int rentalId)
        {
            using (MySqlConnection connection = new MySqlConnection(db.connect))
            {
                connection.Open();

                string query = @"
                SELECT 
                    cars.make AS 'Марка', 
                    cars.model AS 'Модель', 
                    rentals.rental_date AS 'Дата взятия', 
                    rentals.return_date AS 'Дата возврата', 
                    rentals.total_amount AS 'Сумма',
                    CONCAT(customers.first_name, ' ', customers.last_name) AS 'Клиент',
                    customers.passport AS 'Паспорт'
                FROM carrentaldb.rentals 
                INNER JOIN carrentaldb.cars ON cars.car_id = rentals.car_id 
                INNER JOIN carrentaldb.customers ON customers.customer_id = rentals.customer_id
                WHERE rentals.rental_id = @rentalId
                ORDER BY rentals.rental_date DESC;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@rentalId", rentalId);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;

                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10);
                    dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

                    if (dataGridView1.Columns.Contains("ID штрафа"))
                        dataGridView1.Columns["ID штрафа"].Visible = false;
                }
            }
        }

    }
}
