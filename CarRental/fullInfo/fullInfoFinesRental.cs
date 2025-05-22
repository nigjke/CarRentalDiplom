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
                    f.description AS 'Описание',
                    f.fine_amount AS 'Сумма',
                    f.fine_date AS 'Дата',
                    IF(f.is_paid = 1, 'Да', 'Нет') AS 'Оплачен'
                FROM fines f
                WHERE f.rental_id = @rentalId
                ORDER BY f.fine_date DESC;
            ";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@rentalId", _rentalId);

                var adapter = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;

                // Настройка таблицы
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10);
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
