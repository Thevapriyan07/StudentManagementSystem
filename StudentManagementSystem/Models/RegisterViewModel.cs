using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailAvailable", controller: "Account")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password", ErrorMessage ="Password and Confirmation Password Do not match")]
        public string ConfirmPassword { get; set;}

        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public int? GradeId { get; set; }
        public Grade? Grade { get; set; }
        public ICollection<IdentityRole>? Roles { get; set; }


        public List<SelectListItem>? AvailableGrades { get; set; }
        [Required]
        public string Role {  get; set; }

    }
}
