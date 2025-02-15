using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace CarRental
{
    public partial class editCar : Form
    {
        private db db;
        private DataGridViewRow selectedRow;

        public editCar(DataGridViewRow row)
        {
            db = new db();
            selectedRow = row;
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            textBox1.Text = selectedRow.Cells["Марка"].Value.ToString();
            textBox2.Text = selectedRow.Cells["Модель"].Value.ToString();
            textBox3.Text = selectedRow.Cells["Год выпуска"].Value.ToString();
            maskedTextBox1.Text = selectedRow.Cells["Гос.Номер"].Value.ToString();
            textBox4.Text = selectedRow.Cells["Цена за сутки"].Value.ToString();
        }

        private void UpdateDatabase(string make, string model, string year, string new_license_plate, string price, string old_license_plate)
        {
            using (MySqlConnection connection = new MySqlConnection(db.connect))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("UPDATE cars SET make = @make, model = @model, year = @year, license_plate = @new_license_plate, price = @price WHERE license_plate = @old_license_plate", connection);
                command.Parameters.AddWithValue("@make", make);
                command.Parameters.AddWithValue("@model", model);
                command.Parameters.AddWithValue("@year", year);
                command.Parameters.AddWithValue("@new_license_plate", new_license_plate);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@old_license_plate", old_license_plate); 
                command.ExecuteNonQuery();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && maskedTextBox1.Text != "" && textBox4.Text != "")
            {
                string old_license_plate = selectedRow.Cells["Гос.Номер"].Value.ToString();
                UpdateDatabase(textBox1.Text, textBox2.Text, textBox3.Text, maskedTextBox1.Text, textBox4.Text, old_license_plate);

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectEng(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectEng(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectNum(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectNum(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
