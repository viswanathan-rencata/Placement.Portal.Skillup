using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Placement.Portal.Skillup.Models
{
    public class StudentInterviewRoundModel
    {
        public StudentInterviewRoundModel()
        {
            Status = new List<SelectListItem> {
                new SelectListItem { Text = "Select", Value = "0", Selected = true },
                new SelectListItem { Text = "Round1", Value = "1" },                
                new SelectListItem { Text = "Round2", Value = "2" },
                new SelectListItem { Text = "Round3", Value = "3" },
                new SelectListItem { Text = "Selected", Value = "4" },
                new SelectListItem { Text = "Rejected", Value = "5" },
            };
        }
        [Required(ErrorMessage = "Please Enter feedback.")]
        public string FeedBack { get; set; }

        public IEnumerable<SelectListItem> Status { get; set; }

        [Required(ErrorMessage = "Staus is required")]
        public string StatusId { get; set; }

        public long StudentId { get; set; }
        public int StudentsInterViewScheduleDetailsId { get; set; }
        public bool IsStatusUpdateSuccess { get; set; }
        public string UpdateSuccess { get; set; }
    }
}
