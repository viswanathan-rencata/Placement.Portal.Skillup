using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Placement.Portal.Skillup.Models
{
    public class CompanyRegisterViewModel
    {
        [Required(ErrorMessage = "User Name is required.")]
        [StringLength(20, ErrorMessage = "Must be between 5 and 20 characters", MinimumLength = 5)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public IEnumerable<SelectListItem> Company { get; set; }

        [Required(ErrorMessage = "Compay is required")]
        public string CompanyId { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessage = "Must be 10 digits", MinimumLength = 10)]
        [RegularExpression(@"^\d+$", ErrorMessage ="Input should contain only numbers")]
        public string PhoneNumber { get; set; }

        public string UserNameMatchError { get; set; }
        public string CompanySelectionError { get; set; }
        public bool IsRegistrationFailed { get; set; }        
    }
}
