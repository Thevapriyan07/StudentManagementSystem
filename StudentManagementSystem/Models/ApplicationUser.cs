using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public int PhoneNumber {  get; set; }
        public bool IsActive { get; set; }
        public int? GradeId { get; set; }
        public Grade? Grade { get; set; }
    }
}
