using System.Collections.Generic;
using CarShowroom.Cars;

namespace CarShowroom.Customers
{
    // SERVICE: the business rules for customers.
    public class CustomerService
    {
        private CustomerRepository repo;
        private CarService carService;   // needed for the delete rule

        public string LastMessage { get; private set; }

        public CustomerService(CustomerRepository repo, CarService carService)
        {
            this.repo = repo;
            this.carService = carService;
            LastMessage = "";
            AddSampleCustomerIfEmpty();
        }

        // So you can test the customer login the first time you run the project.
        private void AddSampleCustomerIfEmpty()
        {
            if (repo.GetAll().Count == 0)
            {
                repo.Add(new Customer(repo.GetNextId(), "Ahmed Khan", "ahmed", "ahmed123", "0321-7654321", "Lahore"));
            }
        }

        // Returns the Customer if the login is correct, otherwise null.
        public Customer Login(string username, string password)
        {
            Customer customer = repo.GetByUsername(username);

            if (customer != null && customer.Password == password)
            {
                return customer;
            }
            return null;
        }

        // ---------- Create (registration) ----------

        public bool Register(string fullName, string username, string password, string phone, string city)
        {
            if (fullName == "" || username == "")
            {
                LastMessage = "Name and username cannot be empty.";
                return false;
            }

            if (repo.GetByUsername(username) != null)
            {
                LastMessage = "That username is already taken.";
                return false;
            }

            if (password.Length < 4)
            {
                LastMessage = "Password must be at least 4 characters.";
                return false;
            }

            repo.Add(new Customer(repo.GetNextId(), fullName, username, password, phone, city));
            LastMessage = "Registration successful. You can log in now.";
            return true;
        }

        // ---------- Read ----------

        public List<Customer> GetAll()
        {
            return repo.GetAll();
        }

        public Customer GetById(int id)
        {
            return repo.GetById(id);
        }

        // ---------- Update ----------

        // A blank value means "keep the current one".
        public bool UpdateDetails(Customer customer, string fullName, string phone, string city)
        {
            if (fullName != "")
            {
                customer.FullName = fullName;
            }
            if (phone != "")
            {
                customer.Phone = phone;
            }
            if (city != "")
            {
                customer.City = city;
            }

            repo.Update(customer);
            LastMessage = "Details updated successfully.";
            return true;
        }

        public bool ChangePassword(Customer customer, string oldPassword, string newPassword)
        {
            if (customer.Password != oldPassword)
            {
                LastMessage = "Old password is incorrect.";
                return false;
            }

            if (newPassword.Length < 4)
            {
                LastMessage = "New password must be at least 4 characters.";
                return false;
            }

            customer.Password = newPassword;
            repo.Update(customer);
            LastMessage = "Password changed successfully.";
            return true;
        }

        // ---------- Delete ----------

        public bool DeleteCustomer(int id)
        {
            // Sales records point to the customer, so a buyer must stay in the system.
            if (carService.GetByBuyer(id).Count > 0)
            {
                LastMessage = "This customer has bought a car, so the account cannot be deleted.";
                return false;
            }

            if (repo.Delete(id))
            {
                LastMessage = "Customer deleted.";
                return true;
            }

            LastMessage = "Customer not found.";
            return false;
        }
    }
}
