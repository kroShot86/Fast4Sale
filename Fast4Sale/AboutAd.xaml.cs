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
    /// Логика взаимодействия для AboutAd.xaml
    /// </summary>
    public partial class AboutAd : Window
    {
        private int advertisementId;

        public AboutAd(int adId)
        {
            InitializeComponent();
            advertisementId = adId;
            LoadAdvertisementData();
        }

        private void LoadAdvertisementData()
        {
            try
            {
                BD bd = new BD();
                var ad = bd.GetAdvertisementById(advertisementId);

                if (ad != null)
                {
                    if (ad.UserId == Global.ID)
                        Write.Visibility = Visibility.Collapsed;

                    Name.Text = ad.Title;
                    Type.Text = ad.Type;
                    AddressBox.Text = ad.Address;
                    Description.Text = ad.Description;
                    AreaBox.Text = ad.Area + " м²";
                    RoomsBox.Text = ad.Rooms + " комн.";
                    FloorBox.Text = ad.Floor;
                    TotalFloorsBox.Text = ad.TotalFloors;
                    PriceBox.Text = ad.Price + "₽";

                    if (ad.PhotoData != null)
                    {
                        AdImage.Source = ByteArrayToImage(ad.PhotoData);
                    }
                    else
                    {
                        AdImage.Source = null;
                    }
                }
                else
                {
                    MessageBox.Show("Объявление не найдено");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private BitmapImage ByteArrayToImage(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            using (var ms = new System.IO.MemoryStream(bytes))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Write_Click(object sender, RoutedEventArgs e)
        {
            Write write = new Write(advertisementId);
            write.ShowDialog();
        }
    }
}
