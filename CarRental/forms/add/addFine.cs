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
    public partial class addFine : Form
    {
        private int _rentalId;
        private int _customerId;
        public addFine(int rentalId)
        {
            InitializeComponent();
            _rentalId = rentalId;
            LoadRentalCustomer();
            LoadFineTypes();
        }
        private void LoadRentalCustomer()
        {
            using (var conn = new MySqlConnection(db.connect))
            {
                conn.Open();
                string query = @"
                    SELECT customer_id
                    FROM rentals
                    WHERE rental_id = @rentalId
                    LIMIT 1;
                ";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@rentalId", _rentalId);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        _customerId = Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show("Клиент для этой аренды не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                    }
                }
            }
        }

        private void LoadFineTypes()
        {
            using (var conn = new MySqlConnection(db.connect))
            {
                conn.Open();
                string query = "SELECT fine_type_id, name, amount FROM finesType ORDER BY name;";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    var adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    comboBoxFineTypes.DataSource = dt;
                    comboBoxFineTypes.DisplayMember = "name";
                    comboBoxFineTypes.ValueMember = "fine_type_id";
                    comboBoxFineTypes.SelectedIndex = -1;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxFineTypes.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите тип штрафа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int fineTypeId = (int)comboBoxFineTypes.SelectedValue;

            using (var conn = new MySqlConnection(db.connect))
            {
                conn.Open();

                string insertQuery = @"
                    INSERT INTO fines (fine_type_id, customer_id, rental_id, fine_date, is_paid)
                    VALUES (@fineTypeId, @customerId, @rentalId, NOW(), 0);
                ";

                using (var cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@fineTypeId", fineTypeId);
                    cmd.Parameters.AddWithValue("@customerId", _customerId);
                    cmd.Parameters.AddWithValue("@rentalId", _rentalId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Штраф успешно добавлен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при добавлении штрафа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
