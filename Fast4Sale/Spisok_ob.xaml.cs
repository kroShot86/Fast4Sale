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
    /// Логика взаимодействия для Spisok_ob.xaml
    /// </summary>
    public partial class Spisok_ob : Window
    {
        public Spisok_ob()
        {
            InitializeComponent();
            LoadAdvertisements();
        }

        private void AddCard(string title, string address, string price)
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

            Border image = new Border
            {
                Width = 120,
                Height = 90,
                Background = new SolidColorBrush(Color.FromRgb(71, 85, 105)),
                CornerRadius = new CornerRadius(6),
                Margin = new Thickness(0, 0, 15, 0)
            };
            Grid.SetColumn(image, 0);

            StackPanel info = new StackPanel();
            Grid.SetColumn(info, 1);

            TextBlock titleText = new TextBlock
            {
                Text = title,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White
            };

            TextBlock addressText = new TextBlock
            {
                Text = address,
                Foreground = new SolidColorBrush(Color.FromRgb(203, 213, 225)),
                Margin = new Thickness(0, 5, 0, 0)
            };

            TextBlock priceText = new TextBlock
            {
                Text = price,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(16, 185, 129)),
                Margin = new Thickness(0, 10, 0, 0)
            };

            Button details = new Button
            {
                Content = "Подробнее",
                Width = 100,
                Height = 25,
                Background = new SolidColorBrush(Color.FromRgb(59, 130, 246)),
                Foreground = Brushes.White,
                Margin = new Thickness(0, 15, 0, 0)
            };

            details.Click += (s, e) =>
            {
                MessageBox.Show($"{title}\n{address}\n{price}", "Подробности");
            };

            info.Children.Add(titleText);
            info.Children.Add(addressText);
            info.Children.Add(priceText);
            info.Children.Add(details);

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
            if(Global.ID != -1)
            {
                New_ob new_Ob = new New_ob();
                new_Ob.ShowDialog();

                LoadAdvertisements();
            }
            else
            {
                MessageBox.Show("Только авторизированные пользователи могут выстявлять недвижимость");
            }
        }

        private void LoadAdvertisements()
        {
            BD bd = new BD();

            List<Advertisement> ads = bd.GetAllAdvertisements(); // Используй новый метод из BD

            // Очищаем список
            ListPanel.Children.Clear();

            // Добавляем карточки из БД
            foreach (var ad in ads)
            {
                AddCard(ad.Title, ad.Address, ad.Price.ToString() + "₽");
            }

            // Если нет объявлений
            if (ads.Count == 0)
            {
                TextBlock noAds = new TextBlock
                {
                    Text = "Объявлений пока нет",
                    Foreground = Brushes.White,
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                ListPanel.Children.Add(noAds);
            }
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
