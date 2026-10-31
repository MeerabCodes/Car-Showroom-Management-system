using System;

namespace CarShowroom.Cars
{
    // MODEL: describes ONE car. It only holds data and a few small methods.
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public double Price { get; set; }
        public bool IsSold { get; set; }
        public int BuyerId { get; set; }    // id of the customer who bought it (0 = not sold yet)

        // Empty constructor
        public Car()
        {
        }

        // Full constructor for a NEW car (a new car is never sold yet)
        public Car(int id, string brand, string modelName, int year, string color, double price)
        {
            Id = id;
            Brand = brand;
            ModelName = modelName;
            Year = year;
            Color = color;
            Price = price;
            IsSold = false;
            BuyerId = 0;
        }

        public string GetFullName()
        {
            return Brand + " " + ModelName;
        }

        public int GetAge()
        {
            return DateTime.Now.Year - Year;
        }

        public string GetStatus()
        {
            if (IsSold)
            {
                return "Sold";
            }
            return "Available";
        }

        // One line of cars.txt
        public string ToFileString()
        {
            return Id + "|" + Brand + "|" + ModelName + "|" + Year + "|" + Color + "|" +
                   Price + "|" + IsSold + "|" + BuyerId;
        }

        public static string TableHeader()
        {
            return string.Format("{0,-4} {1,-12} {2,-14} {3,-5} {4,-9} {5,-14} {6}",
                                 "ID", "Brand", "Model", "Year", "Color", "Price", "Status");
        }

        public string ToTableRow()
        {
            return string.Format("{0,-4} {1,-12} {2,-14} {3,-5} {4,-9} {5,-14} {6}",
                                 Id, Brand, ModelName, Year, Color, Price.ToString("N0"), GetStatus());
        }
    }
}
