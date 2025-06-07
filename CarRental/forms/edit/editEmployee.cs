using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental
{
    public partial class editEmployee : Form
    {
        private DataGridViewRow selectedRow;
        public int des1;
        string connect = db.connect;
        private db db;
        private int employeeId;

        public editEmployee(DataGridViewRow row)
        {
            db = new db();
            selectedRow = row;
            InitializeComponent();
            LoadData();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox4.Text != "" && maskedTextBox1.Text != "" && comboBox1.Text != "" && textBoxEmail.Text != "")
            {
                if (!IsValidEmail(textBoxEmail.Text))
                {
                    MessageBox.Show("Некорректный email");
                    return;
                }

                string password = GetHashPass(textBox4.Text);
                UpdateDatabase(
                    comboBox1.Text,
                    textBox1.Text,
                    textBox2.Text,
                    maskedTextBox1.Text,
                    textBoxEmail.Text,
                    textBox3.Text,
                    password
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            textBox4.Text = CreatePassword(15);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Utils Func()
        private void LoadData()
        {
            employeeId = Convert.ToInt32(selectedRow.Cells["employee_id"].Value);

            using (MySqlConnection conn = new MySqlConnection(connect))
            {
                conn.Open();

                string query = @"SELECT e.first_name, e.last_name, e.phone, e.email, e.employeeLogin, e.employeePass, r.name as roleName
                         FROM employee e
                         JOIN role r ON e.Role_id = r.Role_id
                         WHERE e.employee_id = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", employeeId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox1.Text = reader["first_name"].ToString();
                            textBox2.Text = reader["last_name"].ToString();
                            maskedTextBox1.Text = reader["phone"].ToString();
                            textBoxEmail.Text = reader["email"].ToString();
                            textBox3.Text = reader["employeeLogin"].ToString();
                            textBox4.Text = reader["employeePass"].ToString();
                            comboBox1.Text = reader["roleName"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Сотрудник не найден.");
                            this.Close();
                        }
                    }
                }
            }
        }

        public static string GetHashPass(string password)
        {

            byte[] bytesPass = Encoding.UTF8.GetBytes(password);

            SHA256Managed hashstring = new SHA256Managed();

            byte[] hash = hashstring.ComputeHash(bytesPass);

            string hashPasswd = string.Empty;

            foreach (byte x in hash)
            {

                hashPasswd += String.Format("{0:x2}", x);
            }

            hashstring.Dispose();

            return hashPasswd;
        }
        private void UpdateDatabase(string roleName, string first_name, string last_name, string phone, string email, string employeeLogin, string employeePass)
        {
            using (MySqlConnection conn = new MySqlConnection(connect))
            {
                conn.Open();

                MySqlCommand getRoleCmd = new MySqlCommand("SELECT Role_id FROM role WHERE name = @roleName", conn);
                getRoleCmd.Parameters.AddWithValue("@roleName", roleName);
                object roleResult = getRoleCmd.ExecuteScalar();

                if (roleResult == null)
                {
                    MessageBox.Show("Роль не найдена.");
                    return;
                }

                des1 = Convert.ToInt32(roleResult);

                MySqlCommand cmd = new MySqlCommand(@"
                    UPDATE employee
                    SET Role_id = @roleId,
                        first_name = @firstName,
                        last_name = @lastName,
                        phone = @phone,
                        email = @email,
                        employeeLogin = @login,
                        employeePass = @pass
                    WHERE employee_id = @id", conn);

                cmd.Parameters.AddWithValue("@roleId", des1);
                cmd.Parameters.AddWithValue("@firstName", first_name);
                cmd.Parameters.AddWithValue("@lastName", last_name);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@login", employeeLogin);
                cmd.Parameters.AddWithValue("@pass", employeePass);
                cmd.Parameters.AddWithValue("@id", employeeId);

                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                    MessageBox.Show("Данные успешно обновлены.");
                else
                    MessageBox.Show("Ошибка при обновлении.");
            }
        }


        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyz_-ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Validation keyPress

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectRus(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectRus(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectEng(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                textBox1.Text = char.ToUpper(textBox1.Text[0]) + textBox1.Text.Substring(1);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
                textBox2.Text = char.ToUpper(textBox2.Text[0]) + textBox2.Text.Substring(1);
        }

        private void textBoxEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectEng(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
