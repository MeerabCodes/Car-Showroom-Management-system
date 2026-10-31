using System;
using System.Collections.Generic;
using CarShowroom.Customers;

namespace CarShowroom.Cars
{
    // SERVICE: the business rules for cars, including searching and selling.
    public class CarService
    {
        private CarRepository repo;

        public string LastMessage { get; private set; }

        public CarService(CarRepository repo)
        {
            this.repo = repo;
            LastMessage = "";
            AddSampleCarsIfEmpty();
        }

        // So the project is not empty the first time you run it.
        private void AddSampleCarsIfEmpty()
        {
            if (repo.GetAll().Count > 0)
            {
                return;
            }

            repo.Add(new Car(repo.GetNextId(), "Toyota", "Corolla", 2024, "White", 8200000));
            repo.Add(new Car(repo.GetNextId(), "Honda", "Civic", 2022, "Black", 7400000));
            repo.Add(new Car(repo.GetNextId(), "Suzuki", "Alto", 2023, "Silver", 2900000));
            repo.Add(new Car(repo.GetNextId(), "Kia", "Sportage", 2024, "Grey", 9800000));
            repo.Add(new Car(repo.GetNextId(), "Hyundai", "Tucson", 2021, "Blue", 8100000));
        }

        // The same checks are used when adding and when updating.
        private bool IsYearValid(int year)
        {
            return year >= 1990 && year <= DateTime.Now.Year + 1;
        }

        // ---------- Create ----------

        public bool AddCar(string brand, string modelName, int year, string color, double price)
        {
            if (brand == "" || modelName == "")
            {
                LastMessage = "Brand and model cannot be empty.";
                return false;
            }

            if (!IsYearValid(year))
            {
                LastMessage = "Year must be between 1990 and " + (DateTime.Now.Year + 1) + ".";
                return false;
            }

            if (price <= 0)
            {
                LastMessage = "Price must be greater than zero.";
                return false;
            }

            Car car = new Car(repo.GetNextId(), brand, modelName, year, color, price);
            repo.Add(car);

            LastMessage = "Car added successfully with ID " + car.Id + ".";
            return true;
        }

        // ---------- Read ----------

        public List<Car> GetAll()
        {
            return repo.GetAll();
        }

        public Car GetById(int id)
        {
            return repo.GetById(id);
        }

        public List<Car> GetAvailable()
        {
            List<Car> result = new List<Car>();

            foreach (Car car in repo.GetAll())
            {
                if (!car.IsSold)
                {
                    result.Add(car);
                }
            }
            return result;
        }

        public List<Car> GetSold()
        {
            List<Car> result = new List<Car>();

            foreach (Car car in repo.GetAll())
            {
                if (car.IsSold)
                {
                    result.Add(car);
                }
            }
            return result;
        }

        // Finds cars whose brand contains the text (not case sensitive).
        public List<Car> SearchByBrand(string text, bool onlyAvailable)
        {
            List<Car> result = new List<Car>();

            foreach (Car car in repo.GetAll())
            {
                if (onlyAvailable && car.IsSold)
                {
                    continue;
                }

                if (car.Brand.ToLower().Contains(text.ToLower()))
                {
                    result.Add(car);
                }
            }
            return result;
        }

        // Available cars that cost no more than maxPrice.
        public List<Car> GetAvailableUnderPrice(double maxPrice)
        {
            List<Car> result = new List<Car>();

            foreach (Car car in GetAvailable())
            {
                if (car.Price <= maxPrice)
                {
                    result.Add(car);
                }
            }
            return result;
        }

        // The cars one customer has bought.
        public List<Car> GetByBuyer(int customerId)
        {
            List<Car> result = new List<Car>();

            foreach (Car car in GetSold())
            {
                if (car.BuyerId == customerId)
                {
                    result.Add(car);
                }
            }
            return result;
        }

        // Adds up the prices of any list of cars (used for revenue and for "total spent").
        public double GetTotalPrice(List<Car> cars)
        {
            double total = 0;

            foreach (Car car in cars)
            {
                total += car.Price;
            }
            return total;
        }

        // ---------- Update ----------

        // A blank brand, model or color means "keep the current one".
        public bool UpdateCar(int id, string brand, string modelName, int year, string color, double price)
        {
            Car car = repo.GetById(id);

            if (car == null)
            {
                LastMessage = "Car not found.";
                return false;
            }

            if (car.IsSold)
            {
                LastMessage = "A sold car cannot be updated.";
                return false;
            }

            if (!IsYearValid(year))
            {
                LastMessage = "Year must be between 1990 and " + (DateTime.Now.Year + 1) + ".";
                return false;
            }

            if (price <= 0)
            {
                LastMessage = "Price must be greater than zero.";
                return false;
            }

            if (brand != "")
            {
                car.Brand = brand;
            }
            if (modelName != "")
            {
                car.ModelName = modelName;
            }
            if (color != "")
            {
                car.Color = color;
            }
            car.Year = year;
            car.Price = price;

            repo.Update(car);
            LastMessage = "Car updated successfully.";
            return true;
        }

        // ---------- Sell ----------

        // The UI looks the customer up and passes it in (it can be null if the ID was wrong).
        public bool SellCar(int carId, Customer customer)
        {
            Car car = repo.GetById(carId);

            if (car == null)
            {
                LastMessage = "Car not found.";
                return false;
            }

            if (car.IsSold)
            {
                LastMessage = "This car is already sold.";
                return false;
            }

            if (customer == null)
            {
                LastMessage = "Customer not found.";
                return false;
            }

            car.IsSold = true;
            car.BuyerId = customer.Id;
            repo.Update(car);

            LastMessage = car.GetFullName() + " sold successfully for " + car.Price.ToString("N0") + ".";
            return true;
        }

        // ---------- Delete ----------

        public bool DeleteCar(int id)
        {
            Car car = repo.GetById(id);

            if (car == null)
            {
                LastMessage = "Car not found.";
                return false;
            }

            if (car.IsSold)
            {
                LastMessage = "A sold car cannot be deleted because it is a sales record.";
                return false;
            }

            repo.Delete(id);
            LastMessage = "Car deleted.";
            return true;
        }
    }
}
