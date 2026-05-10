using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace lab4_oop
{
    public partial class CarsDirectoryWindow : Window
    {
        private AppDbContext db;
        private ObservableCollection<Vehicle> vehiclesCollection;
        private RentalCompany currentCompany;
        private bool _isClosingFromX = true;

        public CarsDirectoryWindow()
        {
            InitializeComponent();
            db = new AppDbContext();
            LoadData();
        }

        private void LoadData()
        {
            currentCompany = db.RentalCompanies.Include(c => c.RentedVehicles).ThenInclude(v => v.Car).FirstOrDefault();

            if (currentCompany == null)
            {
                currentCompany = new RentalCompany { CompanyName = "Gravity Auto" };
                db.RentalCompanies.Add(currentCompany);
                db.SaveChanges();
            }

            vehiclesCollection = new ObservableCollection<Vehicle>(currentCompany.RentedVehicles);
            GridCars.ItemsSource = vehiclesCollection;
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            // Використовуємо стандартний ToString() для заголовка
            txtFullInfo.Text = currentCompany.ToString();

            // Використовуємо ToShortString() для нижньої панелі звіту
            txtShortSummary.Text = currentCompany.ToShortString();
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            AddEditCarWindow window = new AddEditCarWindow();
            if (window.ShowDialog() == true)
            {
                currentCompany.AddVehicle(window.CurrentVehicle);
                db.SaveChanges();
                vehiclesCollection.Add(window.CurrentVehicle);
                UpdateLabels();
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
                    UpdateLabels();
                }
            }
        }

        private void DeleteCar_Click(object sender, RoutedEventArgs e)
        {
            if (GridCars.SelectedItem is Vehicle selectedVehicle)
            {
                var result = MessageBox.Show("Видалити цей запис?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    currentCompany.RemoveVehicle(selectedVehicle);
                    db.SaveChanges();
                    vehiclesCollection.Remove(selectedVehicle);
                    UpdateLabels();
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _isClosingFromX = false;
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_isClosingFromX)
            {
                var result = MessageBox.Show("Вийти з програми повністю?\r(Ні - поверне до головного меню)", "Вихід", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes) Application.Current.Shutdown();
                else if (result == MessageBoxResult.No) _isClosingFromX = false;
                else e.Cancel = true;
            }
            base.OnClosing(e);
        }

        private void GridCars_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = GridCars.SelectedItem != null;
            btnDelete.IsEnabled = GridCars.SelectedItem != null;
        }
    }
}