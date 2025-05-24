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

namespace CarRental.add
{
    public partial class addReview : Form
    {
        private int rentalId;
        private int carId;
        public addReview(int rentalId, int carId)
        {
            InitializeComponent();
            this.rentalId = rentalId;
            this.carId = carId;

            comboBoxRating.Items.AddRange(new object[] { 1, 2, 3, 4, 5 });
            comboBoxRating.SelectedIndex = 4;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxRating.SelectedItem == null || string.IsNullOrWhiteSpace(textBoxComment.Text))
            {
                MessageBox.Show("Пожалуйста, введите оценку и комментарий.");
                return;
            }

            int rating = Convert.ToInt32(comboBoxRating.SelectedItem);
            string comment = textBoxComment.Text.Trim();

            using (var conn = new MySqlConnection(db.connect))
            {
                conn.Open();

                int customerId = -1;
                using (var getCustomerCmd = new MySqlCommand("SELECT customer_id FROM rentals WHERE rental_id = @rentalId", conn))
                {
                    getCustomerCmd.Parameters.AddWithValue("@rentalId", rentalId);
                    var result = getCustomerCmd.ExecuteScalar();
                    if (result == null)
                    {
                        MessageBox.Show("Не удалось найти клиента по аренде.");
                        return;
                    }
                    customerId = Convert.ToInt32(result);
                }
                var cmd = new MySqlCommand(@"
                    INSERT INTO reviews (rental_id, car_id, customer_id, rating, comment, review_date)
                    VALUES (@rentalId, @carId, @customerId, @rating, @comment, @date)", conn);

                cmd.Parameters.AddWithValue("@rentalId", rentalId);
                cmd.Parameters.AddWithValue("@carId", carId);
                cmd.Parameters.AddWithValue("@customerId", customerId);
                cmd.Parameters.AddWithValue("@rating", rating);
                cmd.Parameters.AddWithValue("@comment", comment);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Отзыв успешно добавлен!");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
