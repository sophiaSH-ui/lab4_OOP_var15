using System.Windows;
using System.ComponentModel;

namespace lab4_oop
{
    public partial class MainWindow : Window
    {
        public static bool IsForceClose = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenCars_Click(object sender, RoutedEventArgs e)
        {
            CarsDirectoryWindow carsWindow = new CarsDirectoryWindow();
            this.Hide();
            carsWindow.ShowDialog();
            this.Show();
        }

        private void OpenKitties_Click(object sender, RoutedEventArgs e)
        {
            CatWindow catWindow = new CatWindow();
            this.Hide();
            catWindow.ShowDialog();
            this.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (IsForceClose)
            {
                base.OnClosing(e);
                return;
            }

            if (MessageBox.Show("Ти точно хочеш закрити програму?", "Вихід", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                IsForceClose = true;
                Application.Current.Shutdown();
            }

            base.OnClosing(e);
        }
    }
}       