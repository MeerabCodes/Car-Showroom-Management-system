using System;
using System.Collections.Generic;
using System.IO;

namespace CarShowroom.Admins
{
    // REPOSITORY: the only class that touches admins.txt.
    // It keeps all admins in a List and saves the list to the file after every change.
    // It has NO rules and NO printing. It only stores and finds data.
    public class AdminRepository
    {
        private List<Admin> admins = new List<Admin>();
        private string filePath = "admins.txt";

        public AdminRepository()
        {
            LoadFromFile();
        }

        // ---------- File handling ----------

        private void LoadFromFile()
        {
            admins.Clear();

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
                if (p.Length < 4)
                {
                    continue;   // skip broken lines
                }

                admins.Add(new Admin(int.Parse(p[0]), p[1], p[2], p[3]));
            }
        }

        private void SaveToFile()
        {
            List<string> lines = new List<string>();

            foreach (Admin admin in admins)
            {
                lines.Add(admin.ToFileString());
            }

            File.WriteAllLines(filePath, lines);
        }

        // ---------- CRUD ----------

        public int GetNextId()
        {
            int maxId = 0;
            foreach (Admin admin in admins)
            {
                if (admin.Id > maxId)
                {
                    maxId = admin.Id;
                }
            }
            return maxId + 1;
        }

        // Create
        public void Add(Admin admin)
        {
            admins.Add(admin);
            SaveToFile();
        }

        // Read
        public List<Admin> GetAll()
        {
            return new List<Admin>(admins);
        }

        public Admin GetById(int id)
        {
            foreach (Admin admin in admins)
            {
                if (admin.Id == id)
                {
                    return admin;
                }
            }
            return null;
        }

        public Admin GetByUsername(string username)
        {
            foreach (Admin admin in admins)
            {
                if (admin.Username.ToLower() == username.ToLower())
                {
                    return admin;
                }
            }
            return null;
        }

        // Update
        public bool Update(Admin updatedAdmin)
        {
            for (int i = 0; i < admins.Count; i++)
            {
                if (admins[i].Id == updatedAdmin.Id)
                {
                    admins[i] = updatedAdmin;
                    SaveToFile();
                    return true;
                }
            }
            return false;
        }

        // Delete
        public bool Delete(int id)
        {
            Admin admin = GetById(id);

            if (admin == null)
            {
                return false;
            }

            admins.Remove(admin);
            SaveToFile();
            return true;
        }
    }
}
