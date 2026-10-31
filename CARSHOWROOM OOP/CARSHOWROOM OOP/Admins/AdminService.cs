namespace CarShowroom.Admins
{
    // SERVICE: the rules for the admin account (login, change password).
    // The UI talks to the service. The service talks to the repository.
    public class AdminService
    {
        private AdminRepository repo;

        // If an action fails, the UI reads the reason from here.
        public string LastMessage { get; private set; }

        public AdminService(AdminRepository repo)
        {
            this.repo = repo;
            LastMessage = "";
            CreateDefaultAdminIfEmpty();
        }

        // The first time the program runs there must be at least one admin.
        private void CreateDefaultAdminIfEmpty()
        {
            if (repo.GetAll().Count == 0)
            {
                repo.Add(new Admin(repo.GetNextId(), "Showroom Owner", "admin", "admin123"));
            }
        }

        // Returns the Admin if the login is correct, otherwise null.
        public Admin Login(string username, string password)
        {
            Admin admin = repo.GetByUsername(username);

            if (admin != null && admin.Password == password)
            {
                return admin;
            }
            return null;
        }

        public bool ChangePassword(Admin admin, string oldPassword, string newPassword)
        {
            if (admin.Password != oldPassword)
            {
                LastMessage = "Old password is incorrect.";
                return false;
            }

            if (newPassword.Length < 4)
            {
                LastMessage = "New password must be at least 4 characters.";
                return false;
            }

            admin.Password = newPassword;
            repo.Update(admin);
            LastMessage = "Password changed successfully.";
            return true;
        }
    }
}
