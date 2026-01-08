using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
                string selectedType = "Квартира";
                if (TypeBox.SelectedItem is ComboBoxItem comboBoxItem)
                {
                    selectedType = comboBoxItem.Content.ToString();
                }

                BD bd = new BD();
                int advertisementId = bd.SaveAdvertisement(
                    userId: Global.ID,
                    title: Name.Text.Trim(),
                    address: AddressBox.Text.Trim(),
                    description: Description.Text.Trim(),
                    area: AreaBox.Text.Trim(),
                    rooms: RoomsBox.Text.Trim(),
                    floor: FloorBox.Text.Trim(),
                    totalFloors: TotalFloorsBox.Text.Trim(),
                    price: PriceBox.Text.Trim(),
                    contact: ContactBox.Text.Trim(),
                    phone: PhoneBox.Text.Trim(),
                    email: EmailBox.Text.Trim(),
                    type: selectedType
                );

                this.Close();
            }
        }

    }
}
