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

        private void UserMenuButton_Click(object sender, RoutedEventArgs e)
        {
            UserMenuButton.ContextMenu.IsOpen = true;
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы уверены, что хотите выйти из аккаунта?",
                "Подтверждение выхода",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Global.ID = -1;
                p_reg.Visibility = Visibility.Collapsed;
                p_nereg.Visibility = Visibility.Visible;

                MessageBox.Show("Вы успешно вышли из аккаунта.", "Выход",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "⚠ ВНИМАНИЕ!\n\nУдаление аккаунта приведет к:\n• Безвозвратному удалению всех данных\n• Удалению всех ваших объявлений\n• Невозможности восстановления аккаунта\n\nВы уверены, что хотите удалить аккаунт?",
                "Подтверждение удаления аккаунта",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                MessageBoxResult finalResult = MessageBox.Show(
                    "Это последнее предупреждение! Аккаунт будет удален навсегда.\n\nВведите 'УДАЛИТЬ' для подтверждения:",
                    "Финальное подтверждение",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Exclamation);

                if (finalResult == MessageBoxResult.OK)
                {
                    try
                    {
                        BD bd = new BD();
                        bd.DeleteUserById(Global.ID);

                        Global.ID = -1;
                        p_reg.Visibility = Visibility.Collapsed;
                        p_nereg.Visibility = Visibility.Visible;

                        MessageBox.Show("Аккаунт успешно удален.", "Удаление",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении аккаунта: {ex.Message}", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}