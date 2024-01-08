namespace HRMS.solution.Models.ViewModels
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        // You can also include navigation properties if needed
        // public virtual Employee Employee { get; set; }
    }
}
