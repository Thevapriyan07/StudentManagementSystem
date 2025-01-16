namespace StudentManagementSystem.Models
{
    public class Marks
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public double Score { get; set; }
        public int GradeId { get; set; }
        public Grade Grade { get; set; }
    }
}
