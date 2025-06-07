using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.forms.settings
{
    public partial class settings : Form
    {
        public settings()
        {
            InitializeComponent();
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["uid"] = textBox1.Text;
            Properties.Settings.Default["pwd"] = textBox2.Text;
            Properties.Settings.Default["host"] = textBox3.Text;
            Properties.Settings.Default["db"] = textBox4.Text;
            Properties.Settings.Default.Save();

            db.connect = $@"host={Properties.Settings.Default["host"]};
                                    uid={Properties.Settings.Default["uid"]};
                                    pwd={Properties.Settings.Default["pwd"]};
                                    database={Properties.Settings.Default["db"]};";

            MessageBox.Show("Соединение установлено!", "Сообщение пользователю", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadData()
        {
            textBox1.Text = Properties.Settings.Default["uid"].ToString();
            textBox2.Text = Properties.Settings.Default["pwd"].ToString();
            textBox3.Text = Properties.Settings.Default["host"].ToString();
            textBox4.Text = Properties.Settings.Default["db"].ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            loginForm loginForm = new loginForm();
            loginForm.Show();
        }
    }
}
