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
    public partial class fullInfoCustomerReviews : Form
    {
        private int customerId;
        public fullInfoCustomerReviews(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            LoadCustomerReviews();
        }

        private void LoadCustomerReviews()
        {
            using (MySqlConnection conn = new MySqlConnection(db.connect))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            r.review_id AS 'ID Отзыва',
                            c.make AS 'Марка',
                            c.model AS 'Модель',
                            r.rating AS 'Оценка',
                            r.comment AS 'Комментарий',
                            r.review_date AS 'Дата Отзыва'
                        FROM reviews r
                        INNER JOIN cars c ON r.car_id = c.car_id
                        WHERE r.customer_id = @customerId;
                    ";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@customerId", customerId);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridViewReviews.DataSource = dt;
                    dataGridViewReviews.AutoResizeColumns();
                    dataGridViewReviews.ReadOnly = true;
                    dataGridViewReviews.Columns[0].Visible = false; 
                    dataGridViewReviews.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridViewReviews.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewReviews.DefaultCellStyle.Font = new Font("Segoe UI", 10);
                    dataGridViewReviews.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке отзывов: " + ex.Message);
                }
                dataGridViewReviews.ClearSelection();
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
