namespace StudentManagementSystem.Models
{
    public class StudentViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string GradeName { get; set; }
    }
}
