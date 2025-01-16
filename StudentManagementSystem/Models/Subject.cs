namespace StudentManagementSystem.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public int? GradeId { get; set; }
        public Grade Grade { get; set; }
        public ICollection<Marks> Marks { get; set; }
        

    }
}
