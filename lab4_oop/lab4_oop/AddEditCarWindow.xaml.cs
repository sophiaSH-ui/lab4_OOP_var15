using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
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

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbManufacturer.Text) || string.IsNullOrWhiteSpace(txtModel.Text) ||
                string.IsNullOrWhiteSpace(txtYear.Text) || string.IsNullOrWhiteSpace(txtCarPrice.Text) ||
                string.IsNullOrWhiteSpace(txtLicensePlate.Text) || string.IsNullOrWhiteSpace(txtDuration.Text) ||
                string.IsNullOrWhiteSpace(txtRentalPrice.Text))
            {
                MessageBox.Show("Заповніть всі поля!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtYear.Text, out int year) || year < 1900 || year > DateTime.Now.Year + 1)
            {
                MessageBox.Show("Некоректний рік випуску!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string platePattern = @"^[A-ZА-Я]{2}\d{4}[A-ZА-Я]{2}$";
            if (!Regex.IsMatch(txtLicensePlate.Text.ToUpper(), platePattern))
            {
                MessageBox.Show("Формат номера: AA1234BB", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CurrentVehicle.Car.Manufacturer = cmbManufacturer.Text;
            CurrentVehicle.Car.Model = txtModel.Text;
            CurrentVehicle.Car.Year = year;
            CurrentVehicle.Car.Price = int.Parse(txtCarPrice.Text);
            CurrentVehicle.Category = (CarCategory)cmbCategory.SelectedItem;
            CurrentVehicle.LicensePlate = txtLicensePlate.Text.ToUpper();
            CurrentVehicle.RentalStartDate = dpRentalStart.SelectedDate ?? DateTime.Now;
            CurrentVehicle.RentalDurationDays = int.Parse(txtDuration.Text);
            CurrentVehicle.RentalPrice = int.Parse(txtRentalPrice.Text);
            CurrentVehicle.IsCompleted = chbIsCompleted.IsChecked ?? false;

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