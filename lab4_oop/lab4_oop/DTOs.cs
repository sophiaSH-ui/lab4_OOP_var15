using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lab4_oop
{
    public enum CarCategory
    {
        Family,
        Sports,
        Convertible,
        Jeep
    }

    public class Car
    {
        [Key]
        public int Id { get; set; }

        private string _manufacturer;
        public string Manufacturer
        {
            get => _manufacturer;
            set => _manufacturer = value;
        }

        private string _model;
        public string Model
        {
            get => _model;
            set => _model = value;
        }

        private int _year;
        public int Year
        {
            get => _year;
            set => _year = value;
        }

        private int _price;
        public int Price
        {
            get => _price;
            set => _price = value;
        }
    }

    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        private CarCategory _category;
        public CarCategory Category
        {
            get => _category;
            set => _category = value;
        }

        public int CarId { get; set; }
        public virtual Car Car { get; set; }

        private DateTime _rentalStartDate;
        public DateTime RentalStartDate
        {
            get => _rentalStartDate;
            set => _rentalStartDate = value;
        }

        private int _rentalPrice;
        public int RentalPrice
        {
            get => _rentalPrice;
            set => _rentalPrice = value;
        }

        private int _rentalDurationDays;
        public int RentalDurationDays
        {
            get => _rentalDurationDays;
            set => _rentalDurationDays = value;
        }

        private string _licensePlate;
        public string LicensePlate
        {
            get => _licensePlate;
            set => _licensePlate = value;
        }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set => _isCompleted = value;
        }

        public int? RentalCompanyId { get; set; }
        public virtual RentalCompany RentalCompany { get; set; }
    }

    public class RentalCompany
    {
        [Key]
        public int Id { get; set; }

        private string _companyName;
        public string CompanyName
        {
            get => _companyName;
            set => _companyName = value;
        }

        public virtual List<Vehicle> RentedVehicles { get; set; } = new List<Vehicle>();

        public void AddVehicle(Vehicle vehicle)
        {
            if (vehicle != null)
            {
                RentedVehicles.Add(vehicle);
            }
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            if (RentedVehicles.Contains(vehicle))
            {
                RentedVehicles.Remove(vehicle);
            }
        }

        public override string ToString()
        {
            return $"Прокатна фірма: {CompanyName}, Загальна кількість замовлень у списку: {RentedVehicles.Count}";
        }

        public string ToShortString()
        {
            int totalValue = 0;
            if (RentedVehicles != null)
            {
                foreach (var vehicle in RentedVehicles)
                {
                    if (vehicle.Car != null)
                    {
                        totalValue += vehicle.Car.Price;
                    }
                }
            }
            return $"Фірма: {CompanyName}, Дата: {DateTime.Now:dd.MM.yyyy}, Сумарна вартість авто: {totalValue}$";
        }
    }
}