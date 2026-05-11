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
            LoadCompanies();
        }

        private void LoadCompanies()
        {
            var companies = db.RentalCompanies.Include(c => c.RentedVehicles).ThenInclude(v => v.Car).ToList();

            if (!companies.Any())
            {
                var newCompany = new RentalCompany { CompanyName = "Gravity Auto" };
                db.RentalCompanies.Add(newCompany);
                db.SaveChanges();
                companies.Add(newCompany);
            }

            cmbCompanies.ItemsSource = companies;
            cmbCompanies.SelectedIndex = 0;
        }

        private void cmbCompanies_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbCompanies.SelectedItem is RentalCompany selectedCompany)
            {
                currentCompany = selectedCompany;
                vehiclesCollection = new ObservableCollection<Vehicle>(currentCompany.RentedVehicles);
                GridCars.ItemsSource = vehiclesCollection;
                UpdateLabels();
            }
        }

        private void AddCompany_Click(object sender, RoutedEventArgs e)
        {
            string newName = txtNewCompanyName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(newName))
            {
                var newCompany = new RentalCompany { CompanyName = newName };
                db.RentalCompanies.Add(newCompany);
                db.SaveChanges();

                var companies = db.RentalCompanies.Include(c => c.RentedVehicles).ThenInclude(v => v.Car).ToList();
                cmbCompanies.ItemsSource = companies;
                cmbCompanies.SelectedItem = companies.Last();
                txtNewCompanyName.Clear();
            }
        }

        private void UpdateLabels()
        {
            if (currentCompany != null)
            {
                txtFullInfo.Text = currentCompany.ToString();
                txtShortSummary.Text = currentCompany.ToShortString();
            }
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            if (currentCompany == null) return;

            AddEditCarWindow window = new AddEditCarWindow();
            if (window.ShowDialog() == true)
            {
                window.CurrentVehicle.RentalCompanyId = currentCompany.Id;
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
                    db.Vehicles.Remove(selectedVehicle);
                    db.SaveChanges();
                    vehiclesCollection.Remove(selectedVehicle);
                    UpdateLabels();
                }
            }
        }

        private void DeleteCompany_Click(object sender, RoutedEventArgs e)
        {
            if (currentCompany == null) return;

            var result = MessageBox.Show($"Ви дійсно хочете видалити фірму \"{currentCompany.CompanyName}\"?\nВсі пов'язані автомобілі та замовлення також будуть безповоротно видалені!", "Підтвердження видалення", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (currentCompany.RentedVehicles != null && currentCompany.RentedVehicles.Any())
                {
                    db.Vehicles.RemoveRange(currentCompany.RentedVehicles);
                }

                db.RentalCompanies.Remove(currentCompany);
                db.SaveChanges();

                LoadCompanies();
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

                if (result == MessageBoxResult.Yes)
                {
                    MainWindow.IsForceClose = true; 
                    Application.Current.Shutdown();
                }
                else if (result == MessageBoxResult.No)
                {
                    _isClosingFromX = false; 
                }
                else
                {
                    e.Cancel = true; 
                }
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