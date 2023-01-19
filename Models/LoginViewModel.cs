using System.ComponentModel.DataAnnotations;

namespace Placement.Portal.Skillup.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User Name is required.")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public string PasswordMatchError { get; set; }
        public string UserInactiveError { get; set; }
    }
}
