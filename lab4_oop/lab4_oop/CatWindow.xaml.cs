using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace lab4_oop
{
    public partial class CatWindow : Window
    {
        private string[] _catImages;
        private int _currentIndex = 0;
        private bool _isReturningToWork = false;

        public CatWindow()
        {
            InitializeComponent();
            LoadCats();
            ShowCat();
        }

        private void LoadCats()
        {
            string catsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cats");

            if (Directory.Exists(catsPath))
            {
                _catImages = Directory.GetFiles(catsPath)
                    .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    .ToArray();
            }
            else
            {
                _catImages = Array.Empty<string>();
            }
        }

        private void ShowCat()
        {
            if (_catImages != null && _catImages.Length > 0)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_catImages[_currentIndex], UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                imgCat.Source = bitmap;
            }
            else
            {
                MessageBox.Show("Не знайдено жодного котика у папці 'cats'!", "Сум :(", MessageBoxButton.OK, MessageBoxImage.Warning);
                _isReturningToWork = true;
                this.Close();
            }
        }

        private void NextCat_Click(object sender, RoutedEventArgs e)
        {
            if (_catImages != null && _catImages.Length > 0)
            {
                _currentIndex++;
                if (_currentIndex >= _catImages.Length) _currentIndex = 0;
                ShowCat();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _isReturningToWork = true;
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!_isReturningToWork)
            {
                var result = MessageBox.Show("Взагалі ми тут в прокаті... Може все ж повернемося до роботи?", "Час працювати!", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    MainWindow.IsForceClose = true;
                    Application.Current.Shutdown();
                }
            }
            base.OnClosing(e);
        }
    }
}