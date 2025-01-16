using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Grade
    {
        public int GradeId { get; set; }

        public string GradeName { get; set; }
        public ICollection<Subject> Subjects { get; set; }
        public ICollection<ApplicationUser> Students { get; set; }
    }
}
