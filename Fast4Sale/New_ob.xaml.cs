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
using System.IO;

namespace Fast4Sale
{
    public partial class New_ob : Window
    {
        public int EditAdId
        {
            get => editAdId;
            set
            {
                editAdId = value;
                if (editAdId > 0)
                    LoadAdForEdit();
            }
        }
        private int editAdId = -1;
        private byte[] selectedPhoto;

        public New_ob()
        {
            InitializeComponent();
        }

        private void LoadAdForEdit()
        {
            try
            {
                BD bd = new BD();
                var ad = bd.GetAdvertisementById(editAdId);

                if (ad != null)
                {
                    Name.Text = ad.Title;
                    AddressBox.Text = ad.Address;
                    Description.Text = ad.Description;
                    AreaBox.Text = ad.Area;
                    RoomsBox.Text = ad.Rooms;
                    FloorBox.Text = ad.Floor;
                    TotalFloorsBox.Text = ad.TotalFloors;
                    PriceBox.Text = ad.Price;

                    foreach (ComboBoxItem item in TypeBox.Items)
                    {
                        if (item.Content.ToString() == ad.Type)
                        {
                            TypeBox.SelectedItem = item;
                            break;
                        }
                    }

                    Title = "Редактировать объявление";
                    PublishButton.Content = "Сохранить изменения";

                    if (ad.PhotoData != null && ad.PhotoData.Length > 0)
                    {
                        selectedPhoto = ad.PhotoData;

                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = new MemoryStream(ad.PhotoData);
                        bitmap.EndInit();
                        bitmap.Freeze();

                        PreviewImage.Source = bitmap;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }

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

        private void AddPhoto_Click(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Изображения|*.jpg;*.jpeg;*.png";

            if (dialog.ShowDialog() == true)
            {
                selectedPhoto = File.ReadAllBytes(dialog.FileName);

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = new MemoryStream(selectedPhoto);
                image.EndInit();

                PreviewImage.Source = image;
            }
        }

        private void PublishButton_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "" || AddressBox.Text == "" || Description.Text == "" ||
                TypeBox.SelectedItem == null || AreaBox.Text == "" || RoomsBox.Text == "" ||
                PriceBox.Text == "")
            {
                MessageBox.Show("Заполните обязательные данные");
                return;
            }

            try
            {
                string selectedType = "Квартира";
                if (TypeBox.SelectedItem is ComboBoxItem comboBoxItem)
                {
                    selectedType = comboBoxItem.Content.ToString();
                }

                BD bd = new BD();

                if (EditAdId > 0)
                {
                    bool success = bd.UpdateAdvertisement(
                        adId: EditAdId,
                        title: Name.Text.Trim(),
                        address: AddressBox.Text.Trim(),
                        description: Description.Text.Trim(),
                        area: AreaBox.Text.Trim(),
                        rooms: RoomsBox.Text.Trim(),
                        floor: FloorBox.Text.Trim(),
                        totalFloors: TotalFloorsBox.Text.Trim(),
                        price: PriceBox.Text.Trim(),
                        type: selectedType,
                        photo: selectedPhoto
                    );

                    if (success)
                    {
                        MessageBox.Show("Объявление успешно обновлено!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при обновлении объявления");
                    }
                }
                else
                {
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
                        type: selectedType,
                        photo: selectedPhoto
                    );

                    MessageBox.Show("Объявление успешно опубликовано!");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}