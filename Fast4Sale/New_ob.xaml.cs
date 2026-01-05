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
    /// Логика взаимодействия для New_ob.xaml
    /// </summary>
    public partial class New_ob : Window
    {
        public New_ob()
        {
            InitializeComponent();
        }

        private void OnlyNum(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PublishButton_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "" || AddressBox.Text == "" || Description.Text == "" || TypeBox.SelectedItem == null || AreaBox.Text == "" || RoomsBox.Text == "" || PriceBox.Text == "" || ContactBox.Text == "" || PhoneBox.Text == "")
            {
                MessageBox.Show("Заполните обязательные данные");
            }
            else
            {
                Global.Name = Name.Text;
                Global.Address = AddressBox.Text;
                Global.Description = Description.Text;
                Global.Area = AreaBox.Text;
                Global.Rooms = RoomsBox.Text;
                Global.Floor = FloorBox.Text;
                Global.TotalFloors = TotalFloorsBox.Text;
                Global.Price = PriceBox.Text;
                Global.Contact = ContactBox.Text;
                Global.PhoneNumber = PhoneBox.Text;
                Global.Email = EmailBox.Text;

                Global.Check = true;
                this.Close();
            }
        }

    }
}
