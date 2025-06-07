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
    public partial class carReview: Form
    {
        private int reviewId;
        private bool _isClosing;
        private int _carId;
        public carReview(int carId)
        {
            InitializeComponent();
            _carId = carId;
        }
        private void carReview_Load(object sender, EventArgs e)
        {
            LoadReviews(_carId);
        }
        public void LoadReviews(int carId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(db.connect))
                {
                    conn.Open();
                    string getCarNameQuery = "SELECT make, model FROM cars WHERE car_id = @carId;";
                    using (MySqlCommand carCmd = new MySqlCommand(getCarNameQuery, conn))
                    {
                        carCmd.Parameters.AddWithValue("@carId", carId);

                        using (MySqlDataReader reader = carCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string make = reader["make"].ToString();
                                string model = reader["model"].ToString();
                                label1.Text = $"Отзывы о машине: {make} {model}";
                            }
                            else
                            {
                                label1.Text = "Машина не найдена";
                            }
                        }
                    }
                    string query = @"
                SELECT
                    r.review_id,
                    cust.first_name as 'Пользователь',
                    r.rating as 'Рейтинг',
                    r.comment as 'Комментарий',
                    r.review_date as 'Дата'
                FROM
                    reviews r
                JOIN
                    customers cust ON r.customer_id = cust.customer_id
                WHERE
                    r.car_id = @carId
                ORDER BY
                    r.review_date DESC;
            ";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@carId", carId);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                        dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12);
                        if (dataGridView1.Columns.Count > 0)
                            dataGridView1.Columns[0].Visible = false;

                        dataGridView1.CellDoubleClick -= dataGridView1_CellDoubleClick; 
                        dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке отзывов: " + ex.Message);
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            _isClosing = true;
            this.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "Комментарий")
            {
                string fullComment = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                MessageBox.Show(fullComment, "Полный комментарий", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
