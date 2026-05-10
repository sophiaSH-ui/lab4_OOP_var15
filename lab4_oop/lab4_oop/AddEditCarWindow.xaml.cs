using System;
using System.Windows;

namespace lab4_oop
{
    public partial class AddEditCarWindow : Window
    {
        public Vehicle CurrentVehicle { get; private set; }

        public AddEditCarWindow(Vehicle vehicle = null)
        {
            InitializeComponent();

            cmbCategory.ItemsSource = Enum.GetValues(typeof(CarCategory));
            cmbCategory.SelectedIndex = 0;
            dpRentalStart.SelectedDate = DateTime.Now;

            InputValidator.AttachTextOnly(txtManufacturer);
            InputValidator.AttachIntOnly(txtYear);
            InputValidator.AttachIntOnly(txtCarPrice);
            InputValidator.AttachIntOnly(txtDuration);
            InputValidator.AttachIntOnly(txtRentalPrice);

            if (vehicle != null)
            {
                CurrentVehicle = vehicle;

                txtManufacturer.Text = vehicle.Car.Manufacturer;
                txtModel.Text = vehicle.Car.Model;
                txtYear.Text = vehicle.Car.Year.ToString();
                txtCarPrice.Text = vehicle.Car.Price.ToString();
                cmbCategory.SelectedItem = vehicle.Category;

                txtLicensePlate.Text = vehicle.LicensePlate;
                dpRentalStart.SelectedDate = vehicle.RentalStartDate;
                txtDuration.Text = vehicle.RentalDurationDays.ToString();
                txtRentalPrice.Text = vehicle.RentalPrice.ToString();
            }
            else
            {
                CurrentVehicle = new Vehicle();
                CurrentVehicle.Car = new Car();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtManufacturer.Text) ||
                string.IsNullOrWhiteSpace(txtModel.Text) ||
                string.IsNullOrWhiteSpace(txtYear.Text) ||
                string.IsNullOrWhiteSpace(txtCarPrice.Text) ||
                string.IsNullOrWhiteSpace(txtLicensePlate.Text) ||
                string.IsNullOrWhiteSpace(txtDuration.Text) ||
                string.IsNullOrWhiteSpace(txtRentalPrice.Text))
            {
                MessageBox.Show("Будь ласка, заповніть всі текстові поля!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CurrentVehicle.Car.Manufacturer = txtManufacturer.Text;
            CurrentVehicle.Car.Model = txtModel.Text;
            CurrentVehicle.Car.Year = int.Parse(txtYear.Text);
            CurrentVehicle.Car.Price = int.Parse(txtCarPrice.Text);

            CurrentVehicle.Category = (CarCategory)cmbCategory.SelectedItem;
            CurrentVehicle.LicensePlate = txtLicensePlate.Text;
            CurrentVehicle.RentalStartDate = dpRentalStart.SelectedDate ?? DateTime.Now;
            CurrentVehicle.RentalDurationDays = int.Parse(txtDuration.Text);
            CurrentVehicle.RentalPrice = int.Parse(txtRentalPrice.Text);

            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}