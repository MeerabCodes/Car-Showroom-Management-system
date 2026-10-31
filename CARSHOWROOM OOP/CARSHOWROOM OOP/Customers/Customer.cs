namespace CarShowroom.Customers
{
    // MODEL: a person who browses cars and buys them.
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }

        // Empty constructor
        public Customer()
        {
        }

        // Full constructor
        public Customer(int id, string fullName, string username, string password,
                        string phone, string city)
        {
            Id = id;
            FullName = fullName;
            Username = username;
            Password = password;
            Phone = phone;
            City = city;
        }

        // One line of customers.txt
        public string ToFileString()
        {
            return Id + "|" + FullName + "|" + Username + "|" + Password + "|" + Phone + "|" + City;
        }

        public static string TableHeader()
        {
            return string.Format("{0,-4} {1,-20} {2,-12} {3,-15} {4}",
                                 "ID", "Name", "Username", "Phone", "City");
        }

        public string ToTableRow()
        {
            return string.Format("{0,-4} {1,-20} {2,-12} {3,-15} {4}",
                                 Id, FullName, Username, Phone, City);
        }
    }
}
