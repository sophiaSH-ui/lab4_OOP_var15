using System.Windows;

namespace lab4_oop
{
    public partial class MainWindow : Window
    {
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
            MessageBox.Show("Мяу! 🐱", "Gravity Auto");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ти точно хочеш закрити програму?", "Вихід", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}