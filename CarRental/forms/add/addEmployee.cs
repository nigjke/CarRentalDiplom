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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CarRental
{
    public partial class addEmployee : Form
    {
        private db db;
        public int des1;
        string connect = db.connect;
        public addEmployee()
        {
            db = new db();
            InitializeComponent();
        }
        private void addEmployee_Load(object sender, EventArgs e)
        {
            DataTable Rooms = new DataTable();
            using (MySqlConnection coon = new MySqlConnection(connect))
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = coon;
                cmd.CommandText = "select name from `role`";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(Rooms);
            }

            for (int i = 0; i < Rooms.Rows.Count; i++)
            {
                comboBox1.Items.Add(Rooms.Rows[i]["name"]);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string password = db.GetHashPass(textBox4.Text.Trim());
            if (textBox1.Text != "" && textBox2.Text != "" && textBox4.Text != "" && textBox4.Text != "" && maskedTextBox1.Text != "" && comboBox1.Text != "")
            {
                if (!IsValidEmail(textBoxEmail.Text))
                {
                    MessageBox.Show("Некорректный email");
                    return;
                }
                string role = comboBox1.Text;
                DataTable Rooms = new DataTable();
                using (MySqlConnection coon = new MySqlConnection(connect))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = coon;
                    cmd.CommandText = $@"select Role_id from `role` where name = '{role}'";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(Rooms);
                }

                for (int i = 0; i < Rooms.Rows.Count; i++)
                {
                    des1 = Convert.ToInt32(Rooms.Rows[i]["Role_id"]);
                }

                string sqlQuery = @"INSERT INTO employee (Role_id, first_name, last_name, phone, email, employeeLogin, employeePass) 
                    VALUES (@role, @firstName, @lastName, @phone, @email, @login, @pass)";

                using (MySqlConnection con = new MySqlConnection(connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(sqlQuery, con);
                    cmd.Parameters.AddWithValue("@role", des1);
                    cmd.Parameters.AddWithValue("@firstName", textBox1.Text);
                    cmd.Parameters.AddWithValue("@lastName", textBox2.Text);
                    cmd.Parameters.AddWithValue("@phone", maskedTextBox1.Text);
                    cmd.Parameters.AddWithValue("@email", textBox3.Text);
                    cmd.Parameters.AddWithValue("@login", textBox4.Text);
                    cmd.Parameters.AddWithValue("@pass", password);

                    int res = cmd.ExecuteNonQuery();

                    if (res == 1)
                    {
                        MessageBox.Show("Пользователь добавлен");
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не добавлен");
                    }

                    textBox2.Text = null;
                    comboBox1.Text = null;
                    textBox3.Text = null;
                    textBox1.Text = null;
                    textBox4.Text = null;
                    maskedTextBox1.Text = null;
                    textBoxEmail.Text = null; 
                    DialogResult = DialogResult.OK;

            }
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

        // Utils Func()

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

        private void textBox1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectRus(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectRus(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectEng(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!db.CharCorrectEng(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_Leave_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
                textBox2.Text = char.ToUpper(textBox2.Text[0]) + textBox2.Text.Substring(1);
        }

        private void textBox1_Leave_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                textBox1.Text = char.ToUpper(textBox1.Text[0]) + textBox1.Text.Substring(1);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            textBox4.Text = CreatePassword(15);
        }

        private void textBoxEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (db.CharCorrectRus(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
