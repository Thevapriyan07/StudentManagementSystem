using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name ="Role")]
        public string RoleName { get; set; }
    }
}
