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
                string query = "SELECT rental_id, make as 'Марка', model as 'Модель', first_name as 'Имя', last_name as 'Фамилия', phone as 'Телефон', rental_date as 'Дата взятия', return_date as 'Дата возврата', total_amount as 'Сумма' FROM carrentaldb.rentals " +
        "INNER JOIN customers ON rentals.customer_id = customers.customer_id " +
        "INNER JOIN cars ON cars.car_id = rentals.car_id WHERE rental_id=@rentalId;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@rentalId", rentalId);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
                dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);
                dataGridView1.Columns[0].Visible = false;
            }
        }
    }
}
