using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental
{
    internal class db
    {
        public static string connect = $@"host={Properties.Settings.Default["host"]};
                                      uid={Properties.Settings.Default["uid"]};
                                      pwd={Properties.Settings.Default["pwd"]};
                                      database={Properties.Settings.Default["db"]};";
        MySqlConnection connection = new MySqlConnection(connect);

        public DataTable MySqlReturnData(string query,DataGridView grid)
        {
            try
            {
                using (connection)
                {
                    connection.Open();
                    if (connection.State != ConnectionState.Open)
                    {
                        MessageBox.Show("Не удалось установить подключение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(query, connection))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        grid.DataSource = dt;
                        return dt;

                    }
                }
            }
            catch (MySqlException e)
            {
                MessageBox.Show($"Возникла ошибка при выполнении запроса : {e.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Произошла ошибка : {e.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        static public int CheckUserRole(string login, string password)
        {
            int role = -1;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connect))
                {
                    con.Open();

                    string query = "SELECT role_id, employeePass FROM employee WHERE employeeLogin = @login";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@login", login);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHash = reader["employeePass"].ToString();
                                string enteredHash = GetHashPass(password);

                                if (storedHash == enteredHash)
                                {
                                    int roleId = Convert.ToInt32(reader["role_id"]);
                                    role = (roleId == 2) ? 1 : (roleId == 1) ? 2 : -1;
                                }
                                else
                                {
                                    MessageBox.Show("Неправильный логин или пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return role;
        }

        public static string GetHashPass(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }



        public bool CharCorrectEng(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z');
        }
        public bool CharCorrectRus(char c)
        {
            return (c >= 'а' && c <= 'я') ||
                   (c >= 'А' && c <= 'Я');
        }
        public bool CharCorrectNum(char c)
        {
            return (c >= '0' && c <= '9');
        }

    }
}
