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
                f.fine_id AS 'ID штрафа',
                f.description AS 'Описание',
                f.fine_amount AS 'Сумма штрафа',
                f.fine_date AS 'Дата штрафа',
                CASE 
                    WHEN f.is_paid = 1 THEN 'Оплачен'
                    ELSE 'Не оплачен'
                END AS 'Статус',
                c.first_name AS 'Имя клиента',
                c.last_name AS 'Фамилия клиента'
            FROM fines f
            INNER JOIN customers c ON f.customer_id = c.customer_id
            WHERE f.rental_id = @rentalId
            ORDER BY f.fine_date DESC;";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@rentalId", rentalId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
                dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);

                if (dataGridView1.Columns.Contains("ID штрафа"))
                    dataGridView1.Columns["ID штрафа"].Visible = false;
            }
        }
    }
}
