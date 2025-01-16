using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class AddMarksViewModel
    {
        public int? GradeId { get; set; }
        public int SubjectId { get; set; }
        public List<SelectListItem> Grades { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Subjects { get; set; } = new List<SelectListItem>();
        public List<StudentMarksViewModel> Students { get; set; } = new List<StudentMarksViewModel>();

    }

    public class StudentMarksViewModel
    {
        public string StudentId { get; set; }
        public string? FullName { get; set; }

        [Range(0, 100, ErrorMessage = "Marks must be between 0 and 100.")]
        public int? Marks { get; set; }
    }
}
