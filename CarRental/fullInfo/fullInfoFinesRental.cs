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
    public partial class fullInfoFinesRental : Form
    {
        private int _rentalId;
        public fullInfoFinesRental(int rentalId)
        {
            InitializeComponent();
            _rentalId = rentalId;
            LoadFines();
        }

        private void LoadFines()
        {
            using (var conn = new MySqlConnection(db.connect))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        f.fine_id AS 'ID',
                        ft.name AS 'Описание',
                        ft.amount AS 'Сумма',
                        IF(f.is_paid = 1, 'Да', 'Нет') AS 'Оплачен',
                        CONCAT(c.first_name, ' ', c.last_name) AS 'Клиент'
                    FROM fines f
                    JOIN finesType ft ON f.fine_type_id = ft.fine_type_id
                    JOIN customers c ON f.customer_id = c.customer_id
                    WHERE f.rental_id = @rentalId
                    ORDER BY f.fine_id DESC;
                    ";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@rentalId", _rentalId);

                var adapter = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10);
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

                if (dataGridView1.Columns.Contains("ID"))
                    dataGridView1.Columns["ID"].Visible = false;
            }
        }
        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
