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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fast4Sale
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (Global.ID != -1)
            {
                p_nereg.Visibility = Visibility.Collapsed;
                p_reg.Visibility = Visibility.Visible;
                BD bd = new BD();
                NikName.Text = "Добро пожаловать " + bd.GetUsername(Global.ID);
            }
        }

        private void k_spisky_ob_Click(object sender, RoutedEventArgs e)
        {
            Spisok_ob spisok_ob = new Spisok_ob();
            spisok_ob.Show();
            this.Close();
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            Reg_Okno reg_okno = new Reg_Okno();
            reg_okno.ShowDialog();
            
        }

        private void Log_Click(object sender, RoutedEventArgs e)
        {
            Log_Okno log_Okno = new Log_Okno();
            log_Okno.ShowDialog();

            if (Global.ID != -1)
            {
                p_nereg.Visibility = Visibility.Collapsed;
                p_reg.Visibility = Visibility.Visible;
                BD bd = new BD();
                NikName.Text = "Добро пожаловать " + bd.GetUsername(Global.ID);
            }
            
        }

        private void user_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BD bd = new BD();
            List<string> allUsers = bd.GetAllUsers();

            string allUsersText = string.Join("\n", allUsers);
            MessageBox.Show(allUsersText, "Все пользователи");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Global.ID = -1;

            p_reg.Visibility = Visibility.Collapsed;
            p_nereg.Visibility = Visibility.Visible;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            BD bd = new BD();
            bd.DeleteUserById(Global.ID);
            p_reg.Visibility = Visibility.Collapsed;
            p_nereg.Visibility = Visibility.Visible;
        }
    }
}
