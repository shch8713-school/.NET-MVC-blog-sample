namespace HRMS.solution.Models.ViewModels
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string EmployeeId { get; set; } // Unique identifier for the employee
        public string Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        // Other employee-specific fields

        // Link to the User model for authentication
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
