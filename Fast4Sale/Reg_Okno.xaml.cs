using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Fast4Sale
{
    /// <summary>
    /// Логика взаимодействия для Reg_Okno.xaml
    /// </summary>
    public partial class Reg_Okno : Window
    {
        public Reg_Okno()
        {
            InitializeComponent();
        }

        private bool IsValidLogin(string login)
        {
            return Regex.IsMatch(login, @"^[a-zA-Z0-9]{3,}$");
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^\d{10,12}$");
        }

        private void Kreate_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text.Trim();
            string email = Email.Text.Trim();
            string phone = PhoneNumber.Text.Trim();
            string password = Parol.Text;

            if (!IsValidLogin(login))
            {
                MessageBox.Show("Логин: минимум 3 символа, только буквы и цифры");
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Такой почты не существует");
                return;
            }

            if (!IsValidPhone(phone))
            {
                MessageBox.Show("Номер: только цифры, 10-12 символов");
                return;
            }

            if (password == "")
            {
                MessageBox.Show("Пароль не может быть пустым");
                return;
            }

            BD bd = new BD();
            bd.SaveUser(login, email, phone, password);

            MessageBox.Show("Зарегистрирован");
            this.Close();
        }
    }
}
