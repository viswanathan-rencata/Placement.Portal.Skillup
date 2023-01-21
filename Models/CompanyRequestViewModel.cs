using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System;

namespace Placement.Portal.Skillup.Models
{
    public class CompanyRequestViewModel
    {
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "College is required.")]
        public string CollegeName { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime RequestDate { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Core Area is required")]
        public string CoreAreas { get; set; }

        [Required(ErrorMessage = "Percentage is required")]
        public int CGPAPercent { get; set; }

        public string Comments { get; set; }

        public IEnumerable<SelectListItem> College { get; set; }
    }
}
