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
using System.Diagnostics;

namespace Fast4Sale
{
    /// <summary>
    /// Логика взаимодействия для Write.xaml
    /// </summary>
    public partial class Write : Window
    {

        private int advertisementId;
        public Write(int adId)
        {
            InitializeComponent();
            advertisementId = adId;
            ContactInf();
        }

        private void ContactInf()
        {
            BD bd = new BD();
            var ad = bd.GetAdvertisementById(advertisementId);
            var TS = bd.GetUserContacts(ad.UserId);

            Name.Text = TS.Username;
            PhoneNumber.Text = TS.PhoneNumber;
            Email.Text = TS.Email;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void PhoneNumber_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(PhoneNumber.Text))
            {
                Clipboard.SetText(PhoneNumber.Text);

                CopyHint.Visibility = Visibility.Visible;

                await Task.Delay(1500);

                CopyHint.Visibility = Visibility.Collapsed;
            }
        }

        private async void Email_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(PhoneNumber.Text))
            {
                Clipboard.SetText(PhoneNumber.Text);

                CopyHint.Visibility = Visibility.Visible;

                await Task.Delay(1500);

                CopyHint.Visibility = Visibility.Collapsed;
            }
        }
    }
}
