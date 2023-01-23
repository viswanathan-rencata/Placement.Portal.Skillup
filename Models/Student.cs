namespace Placement.Portal.Skillup.Models
{
    public class Students
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public char Gender { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DOJ { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public long CollegeId { get; set; }
        public string Dept { get; set; }
        public string ClassName { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public long PhoneNumber { get; set; }
        public decimal Percentage { get; set; }

    }
    public class StudentsDropdown
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}