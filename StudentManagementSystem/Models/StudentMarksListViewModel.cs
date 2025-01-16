namespace StudentManagementSystem.Models
{
    public class StudentMarksListViewModel
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }  
        public List<StudentMarksViewModel> StudentsMarks { get; set; }
    }
}
