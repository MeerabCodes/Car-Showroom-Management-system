using System;
using System.Collections.Generic;
using CarShowroom.Cars;

namespace CarShowroom.Customers
{
    // UI: everything the customer sees on the screen.
    // A customer can browse and search cars and see what they bought.
    public class CustomerUI
    {
        private CustomerService customerService;
        private CarService carService;

        private Customer loggedInCustomer;

        public CustomerUI(CustomerService customerService, CarService carService)
        {
            this.customerService = customerService;
            this.carService = carService;
        }

        // ---------- Registration ----------

        // Called from the first menu in Program.cs.
        public void Register()
        {
            Console.WriteLine();
            Console.WriteLine("===== CUSTOMER REGISTRATION =====");

            Console.Write("Full name: ");
            string name = Console.ReadLine();
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password (min 4 characters): ");
            string password = Console.ReadLine();
            Console.Write("Phone: ");
            string phone = Console.ReadLine();
            Console.Write("City: ");
            string city = Console.ReadLine();

            customerService.Register(name, username, password, phone, city);
            Console.WriteLine(customerService.LastMessage);
        }

        // ---------- Login and main loop ----------

        public void Run()
        {
            if (!Login())
            {
                return;
            }

            bool running = true;

            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ViewAvailableCars(); break;
                    case "2": SearchByBrand(); break;
                    case "3": SearchByBudget(); break;
                    case "4": ShowMyPurchases(); break;
                    case "5": UpdateDetails(); break;
                    case "6": ChangePassword(); break;
                    case "7":
                        if (DeleteAccount())
                        {
                            running = false;
                        }
                        break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        private bool Login()
        {
            Console.WriteLine();
            Console.WriteLine("===== CUSTOMER LOGIN =====");

            for (int attempt = 1; attempt <= 3; attempt++)
            {
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();

                loggedInCustomer = customerService.Login(username, password);

                if (loggedInCustomer != null)
                {
                    Console.WriteLine("Welcome, " + loggedInCustomer.FullName + "!");
                    return true;
                }

                Console.WriteLine("Wrong username or password.");
            }

            Console.WriteLine("Too many wrong attempts.");
            return false;
        }

        private void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("===== CUSTOMER MENU =====");
            Console.WriteLine("1. View available cars");
            Console.WriteLine("2. Search cars by brand");
            Console.WriteLine("3. Search cars by budget");
            Console.WriteLine("4. My purchases");
            Console.WriteLine("5. Update my details");
            Console.WriteLine("6. Change my password");
            Console.WriteLine("7. Delete my account");
            Console.WriteLine("0. Logout");
            Console.Write("Choose an option: ");
        }

        // ---------- Browsing cars ----------

        private void ViewAvailableCars()
        {
            Console.WriteLine();
            Console.WriteLine("===== AVAILABLE CARS =====");
            PrintCars(carService.GetAvailable());
        }

        private void SearchByBrand()
        {
            Console.WriteLine();
            Console.WriteLine("===== SEARCH BY BRAND =====");

            Console.Write("Brand to search for: ");
            string text = Console.ReadLine();

            PrintCars(carService.SearchByBrand(text, true));
        }

        private void SearchByBudget()
        {
            Console.WriteLine();
            Console.WriteLine("===== SEARCH BY BUDGET =====");

            Console.Write("Your maximum budget: ");
            double budget;
            if (!double.TryParse(Console.ReadLine(), out budget))
            {
                Console.WriteLine("Budget must be a number.");
                return;
            }

            PrintCars(carService.GetAvailableUnderPrice(budget));
        }

        private void ShowMyPurchases()
        {
            Console.WriteLine();
            Console.WriteLine("===== MY PURCHASES =====");

            List<Car> myCars = carService.GetByBuyer(loggedInCustomer.Id);
            PrintCars(myCars);

            Console.WriteLine();
            Console.WriteLine("Cars bought: " + myCars.Count);
            Console.WriteLine("Total spent: " + carService.GetTotalPrice(myCars).ToString("N0"));
        }

        // ---------- Account ----------

        private void UpdateDetails()
        {
            Console.WriteLine();
            Console.WriteLine("===== UPDATE MY DETAILS =====");
            Console.WriteLine("Press Enter to keep the current value.");

            Console.Write("Full name (" + loggedInCustomer.FullName + "): ");
            string name = Console.ReadLine();
            Console.Write("Phone (" + loggedInCustomer.Phone + "): ");
            string phone = Console.ReadLine();
            Console.Write("City (" + loggedInCustomer.City + "): ");
            string city = Console.ReadLine();

            customerService.UpdateDetails(loggedInCustomer, name, phone, city);
            Console.WriteLine(customerService.LastMessage);
        }

        private void ChangePassword()
        {
            Console.WriteLine();
            Console.WriteLine("===== CHANGE PASSWORD =====");

            Console.Write("Current password: ");
            string oldPassword = Console.ReadLine();
            Console.Write("New password (min 4 characters): ");
            string newPassword = Console.ReadLine();

            customerService.ChangePassword(loggedInCustomer, oldPassword, newPassword);
            Console.WriteLine(customerService.LastMessage);
        }

        // Returns true if the account was deleted (so the menu loop can stop).
        private bool DeleteAccount()
        {
            Console.WriteLine();
            Console.WriteLine("===== DELETE MY ACCOUNT =====");
            Console.Write("Type yes to delete your account permanently: ");

            if (Console.ReadLine().ToLower() != "yes")
            {
                Console.WriteLine("Cancelled.");
                return false;
            }

            bool deleted = customerService.DeleteCustomer(loggedInCustomer.Id);
            Console.WriteLine(customerService.LastMessage);
            return deleted;
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
    }
}
