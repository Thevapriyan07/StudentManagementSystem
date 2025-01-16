namespace StudentManagementSystem.Models
{
    public class StudentDetailsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string GradeName { get; set; }
        public List<SubjectMarksViewModel> Marks { get; set; } = new List<SubjectMarksViewModel>();
    }

    public class SubjectMarksViewModel
    {
        public string SubjectName { get; set; }
        public int Score { get; set; }
    }
}

