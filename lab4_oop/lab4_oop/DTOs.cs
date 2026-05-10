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
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
    }

    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        public CarCategory Category { get; set; }
        public int CarId { get; set; }
        public virtual Car Car { get; set; }
        public DateTime RentalStartDate { get; set; }
        public int RentalPrice { get; set; }
        public int RentalDurationDays { get; set; }
        public string LicensePlate { get; set; }
        public bool IsCompleted { get; set; }
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