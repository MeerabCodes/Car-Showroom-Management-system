using System;
using System.Collections.Generic;
using CarShowroom.Cars;
using CarShowroom.Customers;

namespace CarShowroom.Admins
{
    // UI: everything the admin sees on the screen.
    // Car management is a separate screen (CarUI), so the admin menu stays short.
    public class AdminUI
    {
        private AdminService adminService;
        private CustomerService customerService;
        private CarUI carUI;

        private Admin loggedInAdmin;

        public AdminUI(AdminService adminService, CustomerService customerService, CarUI carUI)
        {
            this.adminService = adminService;
            this.customerService = customerService;
            this.carUI = carUI;
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
                    case "1": carUI.Run(); break;
                    case "2": ViewCustomers(); break;
                    case "3": RemoveCustomer(); break;
                    case "4": ChangePassword(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        private bool Login()
        {
            Console.WriteLine();
            Console.WriteLine("===== ADMIN LOGIN =====");

            for (int attempt = 1; attempt <= 3; attempt++)
            {
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();

                loggedInAdmin = adminService.Login(username, password);

                if (loggedInAdmin != null)
                {
                    Console.WriteLine("Welcome, " + loggedInAdmin.FullName + "!");
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
            Console.WriteLine("===== ADMIN MENU =====");
            Console.WriteLine("1. Manage cars");
            Console.WriteLine("2. View all customers");
            Console.WriteLine("3. Remove a customer");
            Console.WriteLine("4. Change my password");
            Console.WriteLine("0. Logout");
            Console.Write("Choose an option: ");
        }

        // ---------- Customers ----------

        private void ViewCustomers()
        {
            Console.WriteLine();
            Console.WriteLine("===== ALL CUSTOMERS =====");
            PrintCustomers(customerService.GetAll());
        }

        private void RemoveCustomer()
        {
            Console.WriteLine();
            Console.WriteLine("===== REMOVE A CUSTOMER =====");
            PrintCustomers(customerService.GetAll());

            Console.Write("Enter the customer ID to remove: ");
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

            customerService.DeleteCustomer(id);
            Console.WriteLine(customerService.LastMessage);
        }

        private void PrintCustomers(List<Customer> customers)
        {
            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
                return;
            }

            Console.WriteLine(Customer.TableHeader());
            Console.WriteLine(new string('-', 65));

            foreach (Customer customer in customers)
            {
                Console.WriteLine(customer.ToTableRow());
            }
        }

        // ---------- Account ----------

        private void ChangePassword()
        {
            Console.WriteLine();
            Console.WriteLine("===== CHANGE PASSWORD =====");

            Console.Write("Current password: ");
            string oldPassword = Console.ReadLine();
            Console.Write("New password (min 4 characters): ");
            string newPassword = Console.ReadLine();

            adminService.ChangePassword(loggedInAdmin, oldPassword, newPassword);
            Console.WriteLine(adminService.LastMessage);
        }
    }
}
