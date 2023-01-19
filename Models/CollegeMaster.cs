
namespace Placement.Portal.Skillup.Models
{
    public class CollegeMaster
    {
        public CollegeMaster()
        {
            Status = true;
        }
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
