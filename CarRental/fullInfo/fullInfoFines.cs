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
        private int currentCustomerId;
        private int? selectedFineId = null;
        public fullInfoFines(int customerId)
        {
            InitializeComponent();
            currentCustomerId = customerId;
            LoadFines(customerId);
            payBtn.Click -= payBtn_Click;
            payBtn.Click += payBtn_Click;
        }

        private void LoadFines(int customerId)
        {
            using (var conn = new MySqlConnection(db.connect))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        f.fine_id AS 'ID',
                        ft.name AS 'Описание',
                        ft.amount AS 'Сумма',
                        f.fine_date AS 'Дата',
                        IF(f.is_paid = 1, 'Да', 'Нет') AS 'Оплачен'
                    FROM fines f
                    JOIN finesType ft ON f.fine_type_id = ft.fine_type_id
                    WHERE f.customer_id = @customerId
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
                    if (dataGridView1.Columns.Contains("ID"))
                        dataGridView1.Columns["ID"].Visible = false;
                }

                string totalUnpaidQuery = @"
                    SELECT IFNULL(SUM(ft.amount), 0)
                    FROM fines f
                    JOIN finesType ft ON f.fine_type_id = ft.fine_type_id
                    WHERE f.customer_id = @customerId AND f.is_paid = 0;
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
        private void payBtn_Click(object sender, EventArgs e)
        {

            PaySelectedFine();
        }
        private void PaySelectedFine()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите штраф", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int fineId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

            using (var conn = new MySqlConnection(db.connect))
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE fines SET is_paid = 1 WHERE fine_id = @fineId", conn);
                cmd.Parameters.AddWithValue("@fineId", fineId);
                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Штраф успешно оплачен!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadFines(currentCustomerId);
                }
            }
        }
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hit = dataGridView1.HitTest(e.X, e.Y);
                if (hit.RowIndex >= 0)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[hit.RowIndex].Selected = true;
                    contextMenuStrip1.Show(dataGridView1, e.Location);
                }
            }
        }
    }
}
