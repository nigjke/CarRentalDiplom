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
using Excel = Microsoft.Office.Interop.Excel;

namespace CarRental.forms.add
{
    public partial class addReport : Form
    {
        public addReport()
        {
            InitializeComponent();
            FillCarComboBox();
        }

        private void FillCarComboBox()
        {
            using (var con = new MySqlConnection(db.connect))
            {
                con.Open();
                string query = "SELECT car_id, CONCAT(make, ' ', model) AS car_name FROM cars";
                using (var adapter = new MySqlDataAdapter(query, con))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    cmbCars.DataSource = table;
                    cmbCars.DisplayMember = "car_name";
                    cmbCars.ValueMember = "car_id";
                }
            }
        }   

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (cmbCars.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, выберите машину.");
                return;
            }

            int carId = Convert.ToInt32(cmbCars.SelectedValue);

            using (MySqlConnection connection = new MySqlConnection(db.connect))
            {
                connection.Open();

                Excel.Application app = new Excel.Application();
                var workbook = app.Workbooks.Add(Type.Missing);
                Excel._Worksheet sheet = workbook.ActiveSheet;
                sheet.Name = "Отчёт по аренде";

                var titleCell = sheet.Cells[1, 1];
                titleCell.Value = "Отчёт по аренде автомобиля";
                titleCell.Font.Size = 20;
                titleCell.Font.Bold = true;
                sheet.Range[sheet.Cells[1, 1], sheet.Cells[1, 4]].Merge();
                titleCell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                sheet.Cells[2, 1] = $"Дата формирования: {DateTime.Now:dd.MM.yyyy}";

                sheet.Cells[4, 1] = "Дата начала";
                sheet.Cells[4, 2] = "Дата окончания";
                sheet.Cells[4, 3] = "Кол-во дней";
                sheet.Cells[4, 4] = "Сумма";

                Excel.Range headerRange = sheet.Range[sheet.Cells[4, 1], sheet.Cells[4, 4]];
                headerRange.Font.Bold = true;
                headerRange.Interior.Color = Excel.XlRgbColor.rgbLightGray;
                headerRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                string query = @"
                    SELECT rental_date, return_date, total_amount 
                    FROM rentals 
                    WHERE car_id = @carId AND return_date IS NOT NULL";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@carId", carId);
                    var reader = cmd.ExecuteReader();

                    int row = 5;
                    double totalSum = 0;
                    int totalDays = 0;

                    while (reader.Read())
                    {
                        DateTime start = Convert.ToDateTime(reader["rental_date"]);
                        DateTime end = Convert.ToDateTime(reader["return_date"]);
                        double days = (end - start).TotalDays;
                        decimal sum = Convert.ToDecimal(reader["total_amount"]);

                        sheet.Cells[row, 1] = start.ToShortDateString();
                        sheet.Cells[row, 2] = end.ToShortDateString();
                        sheet.Cells[row, 3] = Math.Ceiling(days).ToString();
                        sheet.Cells[row, 4] = sum.ToString("F2");

                        totalDays += (int)Math.Ceiling(days);
                        totalSum += (double)sum;

                        row++;
                    }

                    sheet.Cells[row, 2] = "ИТОГО:";
                    sheet.Cells[row, 2].Font.Bold = true;
                    sheet.Cells[row, 3] = totalDays;
                    sheet.Cells[row, 3].Font.Bold = true;
                    sheet.Cells[row, 4] = totalSum.ToString("F2");
                    sheet.Cells[row, 4].Font.Bold = true;

                    Excel.Range allData = sheet.Range[sheet.Cells[4, 1], sheet.Cells[row, 4]];
                    allData.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    allData.Columns.AutoFit();
                    allData.Rows.AutoFit();
                }

                app.Visible = true;
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

