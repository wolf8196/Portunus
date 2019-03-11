using System.ComponentModel.DataAnnotations;

namespace DemoApps.Shared.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}