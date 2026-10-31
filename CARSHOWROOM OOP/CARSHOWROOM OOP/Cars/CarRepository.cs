using System;
using System.Collections.Generic;
using System.IO;

namespace CarShowroom.Cars
{
    // REPOSITORY: the only class that touches cars.txt.
    public class CarRepository
    {
        private List<Car> cars = new List<Car>();
        private string filePath = "cars.txt";

        public CarRepository()
        {
            LoadFromFile();
        }

        // ---------- File handling ----------

        private void LoadFromFile()
        {
            cars.Clear();

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
                if (p.Length < 8)
                {
                    continue;
                }

                Car car = new Car(int.Parse(p[0]), p[1], p[2], int.Parse(p[3]), p[4], double.Parse(p[5]));
                car.IsSold = bool.Parse(p[6]);
                car.BuyerId = int.Parse(p[7]);

                cars.Add(car);
            }
        }

        private void SaveToFile()
        {
            List<string> lines = new List<string>();

            foreach (Car car in cars)
            {
                lines.Add(car.ToFileString());
            }

            File.WriteAllLines(filePath, lines);
        }

        // ---------- CRUD ----------

        public int GetNextId()
        {
            int maxId = 0;
            foreach (Car car in cars)
            {
                if (car.Id > maxId)
                {
                    maxId = car.Id;
                }
            }
            return maxId + 1;
        }

        // Create
        public void Add(Car car)
        {
            cars.Add(car);
            SaveToFile();
        }

        // Read
        public List<Car> GetAll()
        {
            return new List<Car>(cars);
        }

        public Car GetById(int id)
        {
            foreach (Car car in cars)
            {
                if (car.Id == id)
                {
                    return car;
                }
            }
            return null;
        }

        // Update
        public bool Update(Car updatedCar)
        {
            for (int i = 0; i < cars.Count; i++)
            {
                if (cars[i].Id == updatedCar.Id)
                {
                    cars[i] = updatedCar;
                    SaveToFile();
                    return true;
                }
            }
            return false;
        }

        // Delete
        public bool Delete(int id)
        {
            Car car = GetById(id);

            if (car == null)
            {
                return false;
            }

            cars.Remove(car);
            SaveToFile();
            return true;
        }
    }
}
