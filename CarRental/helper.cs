using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace CarRental
{
    public class helper
    {
        public int selectedCarId = -1;
        public int selectedCustomerId = -1;
        public int selectedRentalId = -1;
        public void SetButtonColors(Button activeButton, Button other1, Button other2, Button other3 = null)
        {
            activeButton.BackColor = Color.FromArgb(92, 96, 255);
            activeButton.ForeColor = Color.FromArgb(34, 36, 49);
            other1.BackColor = Color.FromArgb(34, 36, 49);
            other1.ForeColor = Color.FromArgb(92, 96, 255);
            other2.BackColor = Color.FromArgb(34, 36, 49);
            other2.ForeColor = Color.FromArgb(92, 96, 255);
            if (other3 != null)
            {
                other3.BackColor = Color.FromArgb(34, 36, 49);
                other3.ForeColor = Color.FromArgb(92, 96, 255);
            }
        }
        public string MapDisplayColumnToDbField(string table, string displayColumn)
        {
            var map = new Dictionary<string, Dictionary<string, string>>
            {
                ["cars"] = new Dictionary<string, string>
                {
                    ["Марка"] = "make",
                    ["Модель"] = "model",
                    ["Статус"] = "status",
                    ["Цена за сутки"] = "price"
                },
                ["customers"] = new Dictionary<string, string>
                {
                    ["Имя"] = "first_name",
                    ["Фамилия"] = "last_name",
                    ["Телефон"] = "phone",
                    ["Вод.Удостоверение"] = "driver_license",
                    ["Паспорт"] = "passport"
                },
                ["employee"] = new Dictionary<string, string>
                {
                    ["Имя"] = "e.first_name",
                    ["Фамилия"] = "e.last_name",
                    ["Роль"] = "r.name"
                },
                ["rentals"] = new Dictionary<string, string>
                {
                    ["Марка"] = "c.make",
                    ["Модель"] = "c.model",
                    ["Дата взятия"] = "r.rental_date",
                    ["Дата возврата"] = "r.return_date",
                    ["Сумма"] = "r.total_amount"
                }
            };

            return map.ContainsKey(table) && map[table].ContainsKey(displayColumn)
                ? map[table][displayColumn]
                : null;
        }

        public void CreateWordReport(DataGridViewRow row)
        {
            string templatePath = Path.Combine(Application.StartupPath, "template", "template_with_bookmarks.docx");

            if (!File.Exists(templatePath))
            {
                MessageBox.Show("Файл шаблона не найден:\n" + templatePath);
                return;
            }

            Word.Application wordApp = null;
            Word.Document doc = null;

            try
            {
                wordApp = new Word.Application();
                wordApp.Visible = false;

                doc = wordApp.Documents.Open(templatePath, ReadOnly: false, Visible: false);
                doc.Activate();

                foreach (DataGridViewCell cell in row.Cells)
                {
                    string bookmarkName = cell.OwningColumn.HeaderText.Replace(" ", "_");

                    if (doc.Bookmarks.Exists(bookmarkName))
                    {
                        string value = cell.Value?.ToString() ?? "";
                        doc.Bookmarks[bookmarkName].Range.Text = value;
                    }
                }
                int rentalId = Convert.ToInt32(row.Cells["rental_id"].Value);
                string fullName = GetCustomerFullNameByRentalId(rentalId);
                if (doc.Bookmarks.Exists("ФИО"))
                {
                    doc.Bookmarks["ФИО"].Range.Text = fullName;
                }

                wordApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при генерации документа:\n" + ex.Message);
            }
            finally
            {
                doc = null;
                wordApp = null;
            }
        }


        private string GetCustomerFullNameByRentalId(int rentalId)
        {
            string fullName = "Неизвестно";

            using (MySqlConnection con = new MySqlConnection(db.connect))
            {
                con.Open();
                string query = @"
            SELECT CONCAT(c.first_name, ' ', c.last_name) AS full_name
            FROM rentals r
            JOIN customers c ON r.customer_id = c.customer_id
            WHERE r.rental_id = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", rentalId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            fullName = reader["full_name"].ToString();
                        }
                    }
                }
            }

            return fullName;
        }


        public static byte[] GetCarPhoto(int carId)
        {
            using (var con = new MySqlConnection(db.connect))
            {
                con.Open();
                using (var cmd = new MySqlCommand("SELECT photo FROM cars WHERE car_id = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", carId);
                    return cmd.ExecuteScalar() as byte[];
                }
            }
        }
    }
}
