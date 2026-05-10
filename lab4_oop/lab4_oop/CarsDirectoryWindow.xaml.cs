using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace lab4_oop
{
    public partial class CarsDirectoryWindow : Window
    {
        private AppDbContext db;
        private ObservableCollection<Vehicle> vehiclesCollection;

        public CarsDirectoryWindow()
        {
            InitializeComponent();
            db = new AppDbContext();
            LoadData();
        }

        private void LoadData()
        {
            var vehiclesFromDb = db.Vehicles.Include(v => v.Car).ToList();
            vehiclesCollection = new ObservableCollection<Vehicle>(vehiclesFromDb);
            GridCars.ItemsSource = vehiclesCollection;
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            AddEditCarWindow window = new AddEditCarWindow();
            if (window.ShowDialog() == true)
            {
                db.Vehicles.Add(window.CurrentVehicle);
                db.SaveChanges();
                vehiclesCollection.Add(window.CurrentVehicle);
            }
        }

        private void EditCar_Click(object sender, RoutedEventArgs e)
        {
            if (GridCars.SelectedItem is Vehicle selectedVehicle)
            {
                AddEditCarWindow window = new AddEditCarWindow(selectedVehicle);
                if (window.ShowDialog() == true)
                {
                    db.Entry(selectedVehicle).State = EntityState.Modified;
                    db.SaveChanges();
                    GridCars.Items.Refresh();
                }
            }
        }

        private void DeleteCar_Click(object sender, RoutedEventArgs e)
        {
            if (GridCars.SelectedItem is Vehicle selectedVehicle)
            {
                if (MessageBox.Show("Видалити цей запис?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    db.Vehicles.Remove(selectedVehicle);
                    db.SaveChanges();
                    vehiclesCollection.Remove(selectedVehicle);
                }
            }
        }

        private void GridCars_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            bool hasSelection = GridCars.SelectedItem != null;
            btnEdit.IsEnabled = hasSelection;
            btnDelete.IsEnabled = hasSelection;
        }
    }
}