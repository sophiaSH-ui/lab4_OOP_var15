using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace lab4_oop
{
    public partial class AddEditCarWindow : Window
    {
        private bool _isDataSaved = false;
        public Vehicle CurrentVehicle { get; private set; }

        public AddEditCarWindow(Vehicle vehicle = null)
        {
            InitializeComponent();
            LoadManufacturers();
            LoadRentalCompanies();
            cmbCategory.ItemsSource = Enum.GetValues(typeof(CarCategory));
            cmbCategory.SelectedIndex = 0;
            dpRentalStart.SelectedDate = DateTime.Now;

            InputValidator.AttachIntOnly(txtYear);
            InputValidator.AttachIntOnly(txtCarPrice);
            InputValidator.AttachIntOnly(txtDuration);
            InputValidator.AttachIntOnly(txtRentalPrice);

            if (vehicle != null)
            {
                CurrentVehicle = vehicle;
                cmbManufacturer.Text = vehicle.Car.Manufacturer;
                txtModel.Text = vehicle.Car.Model;
                txtYear.Text = vehicle.Car.Year.ToString();
                txtCarPrice.Text = vehicle.Car.Price.ToString();
                cmbCategory.SelectedItem = vehicle.Category;
                txtLicensePlate.Text = vehicle.LicensePlate;
                dpRentalStart.SelectedDate = vehicle.RentalStartDate;
                txtDuration.Text = vehicle.RentalDurationDays.ToString();
                txtRentalPrice.Text = vehicle.RentalPrice.ToString();
                chbIsCompleted.IsChecked = vehicle.IsCompleted;
                cmbRentalCompany.SelectedValue = vehicle.RentalCompanyId;
            }
            else
            {
                CurrentVehicle = new Vehicle();
                CurrentVehicle.Car = new Car();
            }
        }

        private void LoadManufacturers()
        {
            using (var db = new AppDbContext())
            {
                var list = db.Cars.Select(c => c.Manufacturer).Distinct().ToList();
                cmbManufacturer.ItemsSource = list;
            }
        }

        private void LoadRentalCompanies()
        {
            using (var db = new AppDbContext())
            {
                var companies = db.RentalCompanies.ToList();
                cmbRentalCompany.ItemsSource = companies;
                cmbRentalCompany.SelectedValuePath = "Id";
                if (companies.Any()) cmbRentalCompany.SelectedIndex = 0;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRentalCompany.SelectedItem == null || string.IsNullOrWhiteSpace(cmbManufacturer.Text) ||
                string.IsNullOrWhiteSpace(txtModel.Text) || string.IsNullOrWhiteSpace(txtYear.Text) ||
                string.IsNullOrWhiteSpace(txtCarPrice.Text) || string.IsNullOrWhiteSpace(txtLicensePlate.Text) ||
                string.IsNullOrWhiteSpace(txtDuration.Text) || string.IsNullOrWhiteSpace(txtRentalPrice.Text))
            {
                MessageBox.Show("Заповніть всі поля!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!InputValidator.IsValidYear(txtYear.Text))
            {
                MessageBox.Show("Некоректний рік випуску!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!InputValidator.IsValidLicensePlate(txtLicensePlate.Text))
            {
                MessageBox.Show("Формат номера: AA1234BB", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CurrentVehicle.Car.Manufacturer = cmbManufacturer.Text;
            CurrentVehicle.Car.Model = txtModel.Text;
            CurrentVehicle.Car.Year = int.Parse(txtYear.Text);
            CurrentVehicle.Car.Price = int.Parse(txtCarPrice.Text);
            CurrentVehicle.Category = (CarCategory)cmbCategory.SelectedItem;
            CurrentVehicle.LicensePlate = txtLicensePlate.Text.ToUpper();
            CurrentVehicle.RentalStartDate = dpRentalStart.SelectedDate ?? DateTime.Now;
            CurrentVehicle.RentalDurationDays = int.Parse(txtDuration.Text);
            CurrentVehicle.RentalPrice = int.Parse(txtRentalPrice.Text);
            CurrentVehicle.IsCompleted = chbIsCompleted.IsChecked ?? false;
            CurrentVehicle.RentalCompanyId = (int)cmbRentalCompany.SelectedValue;

            _isDataSaved = true;
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!_isDataSaved)
            {
                var result = MessageBox.Show("Зберегти зміни перед виходом?", "Підтвердження", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Save_Click(null, null);
                    if (this.DialogResult != true) e.Cancel = true;
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            base.OnClosing(e);
        }
    }
}