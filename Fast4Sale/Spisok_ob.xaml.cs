using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class Spisok_ob : Window
    {

        public Spisok_ob()
        {
            InitializeComponent();
            LoadAdvertisements();
        }

        private void AddCard(Advertisement ad)
        {
            Border card = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(51, 65, 85)),
                CornerRadius = new CornerRadius(8),
                BorderBrush = new SolidColorBrush(Color.FromRgb(71, 85, 105)),
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 0, 0, 10),
                Padding = new Thickness(15)
            };

            Grid grid = new Grid();

            ColumnDefinition col1 = new ColumnDefinition { Width = GridLength.Auto };
            ColumnDefinition col2 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            grid.ColumnDefinitions.Add(col1);
            grid.ColumnDefinitions.Add(col2);

            Image img = new Image
            {
                Width = 120,
                Height = 90,
                Stretch = Stretch.UniformToFill
            };

            if (ad.PhotoData != null && ad.PhotoData.Length > 0)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = new MemoryStream(ad.PhotoData);
                bitmap.EndInit();
                bitmap.Freeze();

                img.Source = bitmap;
            }

            Border image = new Border
            {
                Width = 120,
                Height = 90,
                CornerRadius = new CornerRadius(6),
                Margin = new Thickness(0, 0, 15, 0),
                Child = img,
                ClipToBounds = true
            };
            Grid.SetColumn(image, 0);

            StackPanel info = new StackPanel();
            Grid.SetColumn(info, 1);

            TextBlock titleText = new TextBlock
            {
                Text = ad.Title,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White
            };

            TextBlock addressText = new TextBlock
            {
                Text = ad.Address,
                Foreground = new SolidColorBrush(Color.FromRgb(203, 213, 225)),
                Margin = new Thickness(0, 5, 0, 0)
            };

            TextBlock priceText = new TextBlock
            {
                Text = ad.Price + "₽",
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(16, 185, 129)),
                Margin = new Thickness(0, 10, 0, 0)
            };

            StackPanel buttonsPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 15, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            Button details = new Button
            {
                Content = "Подробнее",
                Width = 100,
                Height = 25,
                Background = new SolidColorBrush(Color.FromRgb(59, 130, 246)),
                Foreground = Brushes.White,
                Margin = new Thickness(0, 0, 10, 0)
            };

            Button change = new Button
            {
                Content = "Редактировать",
                Width = 100,
                Height = 25,
                Background = new SolidColorBrush(Color.FromRgb(245, 158, 11)),
                Foreground = Brushes.White,
                Margin = new Thickness(0, 0, 10, 0)
            };

            Button delete = new Button
            {
                Content = "Удалить",
                Width = 100,
                Height = 25,
                Background = new SolidColorBrush(Color.FromRgb(239, 68, 68)),
                Foreground = Brushes.White
            };

            buttonsPanel.Children.Add(details);

            BD bD = new BD();
            if(ad.UserId == Global.ID)
            {
                buttonsPanel.Children.Add(change);
                buttonsPanel.Children.Add(delete);
            }
            
            details.Click += (s, e) =>
            {
                AboutAd aboutWindow = new AboutAd(ad.Id);
                aboutWindow.ShowDialog();
            };

            change.Click += (s, e) =>
            {
                New_ob editWindow = new New_ob();
                editWindow.PublishButton.Content = "Принять изменения";

                editWindow.EditAdId = ad.Id;
                editWindow.ShowDialog();

                LoadAdvertisements();
                

            };

            delete.Click += (s, e) =>
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить объявление \"{ad.Title}\"?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    BD bd = new BD();
                    bool success = bd.DeleteAdvertisement(ad.Id);

                    if (success)
                    {
                        LoadAdvertisements();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении");
                    }
                }
            };

            info.Children.Add(titleText);
            info.Children.Add(addressText);
            info.Children.Add(priceText);
            info.Children.Add(buttonsPanel);

            grid.Children.Add(image);
            grid.Children.Add(info);
            card.Child = grid;

            ListPanel.Children.Add(card);
        }

        private void OnlyNum(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Global.ID != -1)
            {
                New_ob new_Ob = new New_ob();
                new_Ob.ShowDialog();

                LoadAdvertisements();
            }
            else
            {
                MessageBox.Show("Только авторизированные пользователи могут выставлять недвижимость");
            }
        }

        private void LoadAdvertisements()
        {
            BD bd = new BD();

            string type = (TypeCombo.SelectedItem as ComboBoxItem)?.Content.ToString();
            string minPrice = MinPrice.Text;
            string maxPrice = MaxPrice.Text;

            List<Advertisement> ads = bd.SearchAdvertisements(
                searchTerm: "",
                minPrice: minPrice,
                maxPrice: maxPrice,
                type: type
            );

            switch (SortCombo.SelectedIndex)
            {
                case 1:
                    ads = ads.OrderBy(a => int.Parse(a.Price)).ToList();
                    break;

                case 2:
                    ads = ads.OrderByDescending(a => int.Parse(a.Price)).ToList();
                    break;
            }

            ListPanel.Children.Clear();

            foreach (var ad in ads)
                AddCard(ad);

            if (ads.Count == 0)
            {
                ListPanel.Children.Add(new TextBlock
                {
                    Text = "Ничего не найдено",
                    Foreground = Brushes.White,
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(20)
                });
            }
        }

        private void FiltersChanged(object sender, EventArgs e)
        {
            if (IsLoaded)
                LoadAdvertisements();
        }



        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}