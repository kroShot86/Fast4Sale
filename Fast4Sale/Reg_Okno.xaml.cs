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
    /// Логика взаимодействия для Reg_Okno.xaml
    /// </summary>
    public partial class Reg_Okno : Window
    {
        public Reg_Okno()
        {
            InitializeComponent();
        }

        private void Kreate_Click(object sender, RoutedEventArgs e)
        {
            if ( (Login.Text != "") && (Email.Text != "") && (Parol.Text != "") )
            {
                BD bd = new BD();

                bd.SaveUser(Login.Text, Email.Text, Parol.Text);

                MessageBox.Show("Зареган");
                this.Close();
            }
            else
            {
                MessageBox.Show("Обломинго");
            }
        }
    }
}
