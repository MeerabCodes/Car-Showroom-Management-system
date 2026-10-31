namespace CarShowroom.Admins
{
    // MODEL: the showroom owner. A model only holds data.
    public class Admin
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        // Empty constructor
        public Admin()
        {
        }

        // Full constructor
        public Admin(int id, string fullName, string username, string password)
        {
            Id = id;
            FullName = fullName;
            Username = username;
            Password = password;
        }

        // One line of admins.txt: values separated by '|'
        public string ToFileString()
        {
            return Id + "|" + FullName + "|" + Username + "|" + Password;
        }
    }
}
