using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace CarRental
{
    public class helper
    {
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

        public void SortDataGridViewAscending(string tableName, int selectedIndex, DataGridView dataGridview1)
        {
            Dictionary<string, string[]> sortingColumns = new Dictionary<string, string[]>
    {
        { "cars", new string[] { "Марка", "Модель", "Год выпуска", "Гос.Номер", "Статус", "Цена" } },
        { "customers", new string[] { "Имя", "Фамилия", "Телефон", "Вод.Удостоверение", "Паспорт" } },
        { "rentals", new string[] { "Марка", "Модель", "Имя", "Фамилия", "Телефон", "Дата взятия", "Дата возврата", "Сумма" } }
    };

            if (sortingColumns.ContainsKey(tableName) && selectedIndex >= 0 && selectedIndex < sortingColumns[tableName].Length)
            {
                dataGridview1.Sort(dataGridview1.Columns[sortingColumns[tableName][selectedIndex]], System.ComponentModel.ListSortDirection.Ascending);
            }
        }

        public void SortDataGridViewDescending(string tableName, int selectedIndex, DataGridView dataGridview1)
        {
            Dictionary<string, string[]> sortingColumns = new Dictionary<string, string[]>
    {
        { "cars", new string[] { "Марка", "Модель", "Год выпуска", "Гос.Номер", "Статус", "Цена" } },
        { "customers", new string[] { "Имя", "Фамилия", "Телефон", "Вод.Удостоверение", "Паспорт" } },
        { "rentals", new string[] { "Марка", "Модель", "Имя", "Фамилия", "Телефон", "Дата взятия", "Дата возврата", "Сумма" } }
    };

            if (sortingColumns.ContainsKey(tableName) && selectedIndex >= 0 && selectedIndex < sortingColumns[tableName].Length)
            {
                dataGridview1.Sort(dataGridview1.Columns[sortingColumns[tableName][selectedIndex]], System.ComponentModel.ListSortDirection.Descending);
            }
        }
        public void CreateWordReport(DataGridViewRow row)
        {
            string templatePath = System.IO.Directory.GetCurrentDirectory() + @"\template\template.docx";
            var wordApp = new Word.Application();
            var doc = wordApp.Documents.Add(templatePath);

            try
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    string bookmarkName = cell.OwningColumn.HeaderText.Replace(" ", "_");
                    if (doc.Bookmarks.Exists(bookmarkName))
                    {
                        doc.Bookmarks[bookmarkName].Range.Text = cell.Value.ToString();
                    }
                }

                wordApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                doc = null;
                wordApp = null;
            }
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
