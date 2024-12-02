using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental
{
    public partial class sysAdminForm : Form
    {
        public sysAdminForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string tableName = comboBox1.Text;
            OpenFileDialog OPF = new OpenFileDialog();
            OPF.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            OPF.Filter = "Файлы csv|*.csv";
            string FileName = string.Empty;
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                FileName = OPF.FileName;
                if (Path.GetFileNameWithoutExtension(FileName) != tableName)
                {
                    MessageBox.Show("Название файла не совпадает с названием таблицы. Пожалуйста, выберите правильный файл.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Файл не выбран");
                return;
            }
            string[] readText = File.ReadAllLines(FileName);
            string[] titleField = readText[0].Split(';');
            string[] valField;
            string strCommand = string.Empty;
            int count;
            using (MySqlConnection con = new MySqlConnection(db.connect))
            {
                con.Open();
                MySqlCommand cmdClearTable = new MySqlCommand($"DELETE FROM `{tableName}`;", con);
                cmdClearTable.ExecuteNonQuery();

                foreach (string str in readText.Skip(1).ToArray())
                {
                    valField = str.Split(';');

                    #region tableImport

                    switch (tableName)
                    {
                        case "Role":
                            strCommand = $@"Insert Into `Role`({String.Join(",", titleField)}) VALUES(
                                '{valField[0]}','{valField[1]}')";
                            break;
                        case "Cars":
                            strCommand = $@"Insert Into `Cars`({String.Join(",", titleField)}) VALUES(
                                '{valField[0]}','{valField[1]}','{valField[2]}','{valField[3]}','{valField[4]}','{valField[5]}','{valField[6]}')";
                            break;
                        case "Customers":
                            strCommand = $@"Insert Into `Customers`({String.Join(",", titleField.Select(word => "`" + word + "`"))}) VALUES(
                                '{valField[0]}','{valField[1]}','{valField[2]}','{valField[3]}','{valField[4]}','{valField[5]}')";
                            break;
                        case "Employee":
                            MySqlCommand cmdCheckTableRole = new MySqlCommand("SELECT COUNT(*) FROM role;", con);
                            count = cmdCheckTableRole.ExecuteNonQuery();
                            if (count > 0)
                            {
                                strCommand = $@"Insert Into `Employee`({String.Join(",", titleField)}) VALUES(
                                '{valField[0]}','{valField[1]}','{valField[2]}','{valField[3]}','{valField[4]}','{valField[5]}','{valField[6]}')";
                            }
                            else
                            {
                                MessageBox.Show("Сначала заполните таблицу Role");
                                return;
                            }
                            break;
                        }

                    #endregion
                    MySqlCommand cmd = new MySqlCommand(strCommand, con);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }



                }
                MessageBox.Show($"Данные импортированны в таблицу {tableName}");
                con.Close();
            }
        }
    }
}
