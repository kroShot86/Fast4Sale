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
                Foreground = new SolidColorBrush(Color.FromRgb(203, 213, 225)), // #CBD5E1
                Margin = new Thickness(0, 5, 0, 0)
            };

            TextBlock priceText = new TextBlock
            {
                Text = price,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(16, 185, 129)), // #10B981
                Margin = new Thickness(0, 10, 0, 0)
            };

            Button details = new Button
            {
                Content = "Подробнее",
                Width = 100,
                Height = 25,
                Background = new SolidColorBrush(Color.FromRgb(59, 130, 246)), // #3B82F6
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

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            New_ob new_Ob = new New_ob();
            new_Ob.ShowDialog();

            //AddCard("название", "адрес", "10000");
        }
    }
}
