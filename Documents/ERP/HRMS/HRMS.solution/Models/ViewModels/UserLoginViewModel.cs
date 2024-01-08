using System.ComponentModel.DataAnnotations;

namespace HRMS.solution.Models.ViewModels
{
    public class UserLoginViewModel
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
