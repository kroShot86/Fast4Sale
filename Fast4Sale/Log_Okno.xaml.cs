using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для Log_Okno.xaml
    /// </summary>
    public partial class Log_Okno : Window
    {
        public Log_Okno()
        {
            InitializeComponent();
        }

        private void Whod_Click(object sender, RoutedEventArgs e)
        {
            if (Log.Text == "" || Par.Text == "")
            {
                MessageBox.Show("Обломинго");
            }
            else
            {
                BD bd = new BD();
                if (bd.CheckLogin(Log.Text, Par.Text))
                {
                    MessageBox.Show("Ок");
                    Global.ID = bd.GetID(Log.Text);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }

            }
        }
    }
}
