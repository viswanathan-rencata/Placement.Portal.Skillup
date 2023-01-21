using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Placement.Portal.Skillup.Models
{
    public class UserProfileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User Name is required.")]
        [StringLength(20, ErrorMessage = "Must be between 5 and 20 characters", MinimumLength = 5)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessage = "Must be 10 digits", MinimumLength = 10)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Input should contain only numbers")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Education detail is required")]
        [StringLength(20, ErrorMessage = "Education detail length should be between 0 and 20.")]
        public string Education { get; set; }

        [Required(ErrorMessage = "AddressLine1 is required")]
        [StringLength(20, ErrorMessage = "AddressLine1 length should be between 0 and 20.")]
        public string AddressLine1 { get; set; }

        [Required(ErrorMessage = "AddressLine2 is required")]
        [StringLength(20, ErrorMessage = "AddressLine length should be between 0 and 20.")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Postcode is required")]
        [DataType(DataType.PostalCode)]
        [StringLength(6, ErrorMessage = "Must be 6 digits", MinimumLength = 6)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Input should contain only numbers")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(10, ErrorMessage = "City length should be between 0 and 10.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Area is required")]
        [StringLength(20, ErrorMessage = "Area length should be between 0 and 20.")]
        public string Area { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(10, ErrorMessage = "Country length should be between 0 and 10.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(10, ErrorMessage = "State length should be between 0 and 10.")]
        public string State { get; set; }
        public string ProfileEmailAddress { get { return this.Email; } }

        public string UpdateSuccess { get; set; }
        public bool IsUpdateSuccess { get; set; }
    }
}
