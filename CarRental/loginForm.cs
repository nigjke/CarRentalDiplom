using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental
{
    public partial class loginForm : Form
    {
        private CaptchaGenerator _captchaGenerator = new CaptchaGenerator();
        private string _currentCaptcha;
        private DateTime _lastFailedAttempt = DateTime.MinValue;
        private int countfailloginandpwd = 0;
        private int _blockDurationSeconds = 10;

        private Size initialSize = new Size(452, 643);
        private Size expandedSize = new Size(857, 643);
        private string captchaText;
        public loginForm()
        {
            InitializeComponent();
            HideCaptchaAndControls();
            this.Size = initialSize;

        }
        private void UpdateCaptcha()
        {
            _currentCaptcha = _captchaGenerator.GenerateCaptcha();
            captchaImage.Image = _captchaGenerator.RenderCaptcha(_currentCaptcha);
        }

        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public async void sendBtn_Click(object sender, EventArgs e)
        {
            String loginUser = loginField.Text;
            String pwdUser = pwdField.Text;
            if (loginUser == "" || pwdUser == "")
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                loginField.Text = "Login";
                pwdField.Text = "Password";
                pwdField.PasswordChar = default;
                pictureBox3.BackgroundImage = Properties.Resources.padlock;
                panel1.BackColor = Color.White;
                pwdField.ForeColor = Color.White;
            }
            else
            {
                string adminUsername = ConfigurationManager.AppSettings["AdminUsername"];

                string adminPassword = ConfigurationManager.AppSettings["AdminPassword"];

                if (loginUser == adminUsername && pwdUser == adminPassword)
                {
                    sysAdminForm sysAdminForm = new sysAdminForm();
                    sysAdminForm.Show();
                    this.Hide();
                }
                else
                {
                    int role = db.CheckUserRole(loginUser, pwdUser);
                    if (role == 2)
                    {
                        adminForm a = new adminForm(loginUser);
                        a.Show();
                        this.Hide();
                    }
                    else if (role == 1)
                    {
                        managerForm a = new managerForm(loginUser);
                        a.Show();
                        this.Hide();
                    }
                    else
                    {
                        loginField.Text = "Login";
                        pwdField.Text = "Password";
                        pwdField.PasswordChar = default;
                        pictureBox3.BackgroundImage = Properties.Resources.padlock;
                        panel1.BackColor = Color.White;
                        pwdField.ForeColor = Color.White;
                        this.Size = expandedSize;
                        ShowCaptchaAndControls();
                        countfailloginandpwd++;
                        if (DateTime.Now - _lastFailedAttempt < TimeSpan.FromSeconds(_blockDurationSeconds))
                        {
                            this.Enabled = false;
                            MessageBox.Show("Вы были заблокированы на 10 секунд из-за слишком большого количества неудачных попыток входа в систему.");
                            await Task.Delay(TimeSpan.FromSeconds(10));
                            this.Enabled = true;
                            return;
                        }
                        if (inputcaptcha.Text != _currentCaptcha && countfailloginandpwd == 1)
                        {
                            _lastFailedAttempt = DateTime.Now;
                            UpdateCaptcha();
                            updatecaptch.Click += (s, args) => UpdateCaptcha();
                            MessageBox.Show("Неверный логин, пароль или КАПЧА. Пожалуйста, повторите попытку");
                            this.Size = expandedSize;
                            ShowCaptchaAndControls();
                            inputcaptcha.Enabled = true;
                            updatecaptch.Enabled = true;
                            return;
                        }
                    }
                }
            }
        }
        private void loginField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!CharCorrectLogin(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private bool CharCorrectLogin(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z');
        }

        private void pwdField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!CharCorrectLogin(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void loginField_Click(object sender, EventArgs e)
        {
            loginField.Text = "";
            pictureBox2.BackgroundImage = Properties.Resources.avatar1;
            panel.BackColor = Color.FromArgb(92, 96, 255);
            loginField.ForeColor = Color.FromArgb(92, 96, 255);

            pictureBox3.BackgroundImage = Properties.Resources.padlock;
            panel1.BackColor = Color.White;
            pwdField.ForeColor = Color.White;
        }

        private void pwdField_Click(object sender, EventArgs e)
        {
            pwdField.Text = "";
            pwdField.PasswordChar = '*';
            pictureBox2.BackgroundImage = Properties.Resources.avatar;
            panel.BackColor = Color.White;
            loginField.ForeColor = Color.White;

            pictureBox3.BackgroundImage = Properties.Resources.padlock1;
            panel1.BackColor = Color.FromArgb(92, 96, 255);
            pwdField.ForeColor = Color.FromArgb(92, 96, 255);
        }

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox4.BackgroundImage = Properties.Resources.show;
            pwdField.PasswordChar = default;
        }

        private void pictureBox4_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox4.BackgroundImage = Properties.Resources.hide;
            pwdField.PasswordChar = '*';
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            pwdField.PasswordChar = default;
        }
        public class CaptchaGenerator
        {
            private static readonly Random _random = new Random();
            private const string _characters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            private const int _captchaLength = 4;

            public string GenerateCaptcha()
            {
                var captcha = new string(Enumerable.Range(0, _captchaLength)
                    .Select(_ => _characters[_random.Next(_characters.Length)]).ToArray());
                return captcha;
            }

            public Bitmap RenderCaptcha(string captcha)
            {
                var bitmap = new Bitmap(500, 200);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.White);
                    graphics.DrawString(captcha, new Font("Monserat", 32), new SolidBrush(Color.Black), _random.Next(0,250), _random.Next(50, 200));
                    for (int i = 0; i < 50; i++)
                    {
                        graphics.DrawLine(new Pen(Color.Gray, 1), _random.Next(bitmap.Width), _random.Next(bitmap.Height),
                            _random.Next(bitmap.Width), _random.Next(bitmap.Height));
                    }
                }
                return bitmap;
            }
        }
        private void ShowCaptchaAndControls()
        {
            captchaImage.Visible = true;
            inputcaptcha.Visible = true;
            updatecaptch.Visible = true;
        }
        private void HideCaptchaAndControls()
        {
            captchaImage.Visible = false;
            inputcaptcha.Visible = false;
            updatecaptch.Visible = false;
        }

    }
}
