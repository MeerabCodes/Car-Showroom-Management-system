using System;
using System.Collections.Generic;
using System.IO;

namespace CarShowroom.Customers
{
    // REPOSITORY: the only class that touches customers.txt.
    public class CustomerRepository
    {
        private List<Customer> customers = new List<Customer>();
        private string filePath = "customers.txt";

        public CustomerRepository()
        {
            LoadFromFile();
        }

        // ---------- File handling ----------

        private void LoadFromFile()
        {
            customers.Clear();

            if (!File.Exists(filePath))
            {
                return;
            }

            foreach (string line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] p = line.Split('|');
                if (p.Length < 6)
                {
                    continue;
                }

                customers.Add(new Customer(int.Parse(p[0]), p[1], p[2], p[3], p[4], p[5]));
            }
        }

        private void SaveToFile()
        {
            List<string> lines = new List<string>();

            foreach (Customer customer in customers)
            {
                lines.Add(customer.ToFileString());
            }

            File.WriteAllLines(filePath, lines);
        }

        // ---------- CRUD ----------

        public int GetNextId()
        {
            int maxId = 0;
            foreach (Customer customer in customers)
            {
                if (customer.Id > maxId)
                {
                    maxId = customer.Id;
                }
            }
            return maxId + 1;
        }

        // Create
        public void Add(Customer customer)
        {
            customers.Add(customer);
            SaveToFile();
        }

        // Read
        public List<Customer> GetAll()
        {
            return new List<Customer>(customers);
        }

        public Customer GetById(int id)
        {
            foreach (Customer customer in customers)
            {
                if (customer.Id == id)
                {
                    return customer;
                }
            }
            return null;
        }

        public Customer GetByUsername(string username)
        {
            foreach (Customer customer in customers)
            {
                if (customer.Username.ToLower() == username.ToLower())
                {
                    return customer;
                }
            }
            return null;
        }

        // Update
        public bool Update(Customer updatedCustomer)
        {
            for (int i = 0; i < customers.Count; i++)
            {
                if (customers[i].Id == updatedCustomer.Id)
                {
                    customers[i] = updatedCustomer;
                    SaveToFile();
                    return true;
                }
            }
            return false;
        }

        // Delete
        public bool Delete(int id)
        {
            Customer customer = GetById(id);

            if (customer == null)
            {
                return false;
            }

            customers.Remove(customer);
            SaveToFile();
            return true;
        }
    }
}
