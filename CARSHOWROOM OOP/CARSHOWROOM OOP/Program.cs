using System;
using CarShowroom.Admins;
using CarShowroom.Cars;
using CarShowroom.Customers;

namespace CarShowroom
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // STEP 1: one repository per entity (each one loads its own text file).
            AdminRepository adminRepo = new AdminRepository();
            CustomerRepository customerRepo = new CustomerRepository();
            CarRepository carRepo = new CarRepository();

            // STEP 2: one service per entity. A service receives the repository it needs.
            // CustomerService also needs CarService, because a customer who bought a car cannot be deleted.
            AdminService adminService = new AdminService(adminRepo);
            CarService carService = new CarService(carRepo);
            CustomerService customerService = new CustomerService(customerRepo, carService);

            // STEP 3: one UI per entity. A UI receives the services it needs.
            CarUI carUI = new CarUI(carService, customerService);
            AdminUI adminUI = new AdminUI(adminService, customerService, carUI);
            CustomerUI customerUI = new CustomerUI(customerService, carService);

            // STEP 4: the first screen.
            // Default admin login:    admin / admin123
            // Sample customer login:  ahmed / ahmed123
            bool running = true;

            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("===== CAR SHOWROOM MANAGEMENT SYSTEM =====");
                Console.WriteLine("1. Admin login");
                Console.WriteLine("2. Customer login");
                Console.WriteLine("3. Customer registration");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": adminUI.Run(); break;
                    case "2": customerUI.Run(); break;
                    case "3": customerUI.Register(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }

            Console.WriteLine("Goodbye!");
        }
    }
}
