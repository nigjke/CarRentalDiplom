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
    public partial class fullInfoFines : Form
    {
        public fullInfoFines(int customerId)
        {
            InitializeComponent();
            LoadFines(customerId);
        }

        private void LoadFines(int customerId)
        {
            using (var conn = new MySqlConnection(db.connect))
            {
                conn.Open();
                string query = @"
                    SELECT f.description AS 'Описание', f.fine_amount AS 'Сумма', 
                           f.fine_date AS 'Дата', IF(f.is_paid = 1, 'Да', 'Нет') AS 'Оплачен'
                    FROM fines f
                    JOIN rentals r ON f.rental_id = r.rental_id
                    WHERE r.customer_id = @customerId
                    ORDER BY f.fine_date DESC;
                    ";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    var adapter = new MySqlDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;

                    dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10);
                    dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                string totalUnpaidQuery = @"
                    SELECT IFNULL(SUM(f.fine_amount), 0)
                    FROM fines f
                    JOIN rentals r ON f.rental_id = r.rental_id
                    WHERE r.customer_id = @customerId AND f.is_paid = 0;
                    ";

                using (var sumCmd = new MySqlCommand(totalUnpaidQuery, conn))
                {
                    sumCmd.Parameters.AddWithValue("@customerId", customerId);
                    object result = sumCmd.ExecuteScalar();
                    decimal totalUnpaid = Convert.ToDecimal(result);
                    label1.Text = $"Непогашенные штрафы: {totalUnpaid:C}";
                }
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
