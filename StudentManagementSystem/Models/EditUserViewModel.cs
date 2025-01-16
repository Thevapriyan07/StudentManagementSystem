using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        [Required]
        [Phone]
        public int PhoneNumber { get; set; }

        public string Role { get; set; }
        public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>();

        public int? GradeId { get; set; }
        public List<SelectListItem> AvailableGrades { get; set; } = new List<SelectListItem>();
    }

}
