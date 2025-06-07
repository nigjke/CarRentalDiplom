using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.fullInfo
{
    public partial class fullInfoReference : Form
    {
        public fullInfoReference()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            labelRed.BackColor = Color.Red;
            labelRed.ForeColor = Color.White;
            labelRed.Text = "Срок аренды истёк";

            labelYellow.BackColor = Color.Yellow;
            labelYellow.ForeColor = Color.Black;
            labelYellow.Text = "Аренда в процессе";

            labelGreen.BackColor = Color.Green;
            labelGreen.ForeColor = Color.White;
            labelGreen.Text = "Аренда запланирована";
        }
        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
