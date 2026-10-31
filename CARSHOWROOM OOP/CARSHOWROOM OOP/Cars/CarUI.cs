using System;
using System.Collections.Generic;
using CarShowroom.Customers;

namespace CarShowroom.Cars
{
    // UI: the car management screen. The admin opens it from the admin menu.
    // The UI only asks questions and prints answers. All rules are in CarService.
    public class CarUI
    {
        private CarService carService;
        private CustomerService customerService;

        public CarUI(CarService carService, CustomerService customerService)
        {
            this.carService = carService;
            this.customerService = customerService;
        }

        public void Run()
        {
            bool running = true;

            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddCar(); break;
                    case "2": ViewAllCars(); break;
                    case "3": SearchCars(); break;
                    case "4": UpdateCar(); break;
                    case "5": DeleteCar(); break;
                    case "6": SellCar(); break;
                    case "7": ShowSalesReport(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("===== MANAGE CARS =====");
            Console.WriteLine("1. Add a car");
            Console.WriteLine("2. View all cars");
            Console.WriteLine("3. Search cars by brand");
            Console.WriteLine("4. Update a car");
            Console.WriteLine("5. Delete a car");
            Console.WriteLine("6. Sell a car to a customer");
            Console.WriteLine("7. Sales report");
            Console.WriteLine("0. Back");
            Console.Write("Choose an option: ");
        }

        // ---------- Create ----------

        private void AddCar()
        {
            Console.WriteLine();
            Console.WriteLine("===== ADD A CAR =====");

            Console.Write("Brand: ");
            string brand = Console.ReadLine();
            Console.Write("Model: ");
            string modelName = Console.ReadLine();

            Console.Write("Year: ");
            int year;
            if (!int.TryParse(Console.ReadLine(), out year))
            {
                Console.WriteLine("Year must be a number.");
                return;
            }

            Console.Write("Color: ");
            string color = Console.ReadLine();

            Console.Write("Price: ");
            double price;
            if (!double.TryParse(Console.ReadLine(), out price))
            {
                Console.WriteLine("Price must be a number.");
                return;
            }

            carService.AddCar(brand, modelName, year, color, price);
            Console.WriteLine(carService.LastMessage);
        }

        // ---------- Read ----------

        private void ViewAllCars()
        {
            Console.WriteLine();
            Console.WriteLine("===== ALL CARS =====");
            PrintCars(carService.GetAll());
        }

        private void SearchCars()
        {
            Console.WriteLine();
            Console.WriteLine("===== SEARCH CARS =====");

            Console.Write("Brand to search for: ");
            string text = Console.ReadLine();

            PrintCars(carService.SearchByBrand(text, false));
        }

        // ---------- Update ----------

        private void UpdateCar()
        {
            Console.WriteLine();
            Console.WriteLine("===== UPDATE A CAR =====");
            PrintCars(carService.GetAvailable());

            Console.Write("Enter the car ID to update: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Car car = carService.GetById(id);
            if (car == null)
            {
                Console.WriteLine("Car not found.");
                return;
            }

            Console.WriteLine("Press Enter to keep the current value.");

            Console.Write("Brand (" + car.Brand + "): ");
            string brand = Console.ReadLine();
            Console.Write("Model (" + car.ModelName + "): ");
            string modelName = Console.ReadLine();
            Console.Write("Year (" + car.Year + "): ");
            string yearText = Console.ReadLine();
            Console.Write("Color (" + car.Color + "): ");
            string color = Console.ReadLine();
            Console.Write("Price (" + car.Price + "): ");
            string priceText = Console.ReadLine();

            // Blank number = keep the old number.
            int year = car.Year;
            if (yearText != "" && !int.TryParse(yearText, out year))
            {
                Console.WriteLine("Year must be a number. Nothing was changed.");
                return;
            }

            double price = car.Price;
            if (priceText != "" && !double.TryParse(priceText, out price))
            {
                Console.WriteLine("Price must be a number. Nothing was changed.");
                return;
            }

            carService.UpdateCar(id, brand, modelName, year, color, price);
            Console.WriteLine(carService.LastMessage);
        }

        // ---------- Delete ----------

        private void DeleteCar()
        {
            Console.WriteLine();
            Console.WriteLine("===== DELETE A CAR =====");
            PrintCars(carService.GetAvailable());

            Console.Write("Enter the car ID to delete: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Console.Write("Are you sure? (yes/no): ");
            if (Console.ReadLine().ToLower() != "yes")
            {
                Console.WriteLine("Cancelled.");
                return;
            }

            carService.DeleteCar(id);
            Console.WriteLine(carService.LastMessage);
        }

        // ---------- Sell ----------

        private void SellCar()
        {
            Console.WriteLine();
            Console.WriteLine("===== SELL A CAR =====");

            List<Car> available = carService.GetAvailable();
            List<Customer> customers = customerService.GetAll();

            if (available.Count == 0)
            {
                Console.WriteLine("There are no cars available to sell.");
                return;
            }

            if (customers.Count == 0)
            {
                Console.WriteLine("There are no registered customers yet.");
                return;
            }

            PrintCars(available);
            Console.Write("Enter the car ID to sell: ");
            int carId;
            if (!int.TryParse(Console.ReadLine(), out carId))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Console.WriteLine();
            PrintCustomers(customers);
            Console.Write("Enter the customer ID: ");
            int customerId;
            if (!int.TryParse(Console.ReadLine(), out customerId))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Customer customer = customerService.GetById(customerId);
            carService.SellCar(carId, customer);
            Console.WriteLine(carService.LastMessage);
        }

        private void ShowSalesReport()
        {
            Console.WriteLine();
            Console.WriteLine("===== SALES REPORT =====");

            List<Car> sold = carService.GetSold();

            Console.WriteLine("Total cars     : " + carService.GetAll().Count);
            Console.WriteLine("Available cars : " + carService.GetAvailable().Count);
            Console.WriteLine("Sold cars      : " + sold.Count);
            Console.WriteLine("Total revenue  : " + carService.GetTotalPrice(sold).ToString("N0"));

            Console.WriteLine();
            Console.WriteLine("Sold cars and their buyers:");

            if (sold.Count == 0)
            {
                Console.WriteLine("  (no sales yet)");
            }

            foreach (Car car in sold)
            {
                string buyerName = "(deleted customer)";
                Customer buyer = customerService.GetById(car.BuyerId);

                if (buyer != null)
                {
                    buyerName = buyer.FullName;
                }

                Console.WriteLine("  " + car.GetFullName() + " -> " + buyerName +
                                  " (" + car.Price.ToString("N0") + ")");
            }
        }

        // ---------- Printing ----------

        private void PrintCars(List<Car> cars)
        {
            if (cars.Count == 0)
            {
                Console.WriteLine("No cars found.");
                return;
            }

            Console.WriteLine(Car.TableHeader());
            Console.WriteLine(new string('-', 70));

            foreach (Car car in cars)
            {
                Console.WriteLine(car.ToTableRow());
            }
        }

        private void PrintCustomers(List<Customer> customers)
        {
            Console.WriteLine(Customer.TableHeader());
            Console.WriteLine(new string('-', 65));

            foreach (Customer customer in customers)
            {
                Console.WriteLine(customer.ToTableRow());
            }
        }
    }
}
