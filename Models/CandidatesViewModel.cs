using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Placement.Portal.Skillup.Models
{
    public class CandidatesViewModel
    {
        public CandidatesViewModel()
        {
            CandidatesGrid = new List<CandidatesGrid>();
        }
        public List<CandidatesGrid> CandidatesGrid { get; set; }

        public IEnumerable<SelectListItem> College { get; set; }

        [Required(ErrorMessage = "College is required")]
        public string CollegeId { get; set; }
    }
}
