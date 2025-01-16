using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class GradeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GradeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ViewStudentsByGrade(int gradeId)
        {
            var students = await _context.Users
                .Where(u => u.GradeId == gradeId)
                .Select(u => new StudentViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Gender = u.Gender,
                    Address = u.Address,
                    DateOfBirth = u.DateOfBirth,
                    PhoneNumber = u.PhoneNumber,
                    GradeName = u.Grade.GradeName
                })
                .ToListAsync();

            var gradeName = await _context.Grades
                .Where(g => g.GradeId == gradeId)
                .Select(g => g.GradeName)
                .FirstOrDefaultAsync();

            ViewBag.GradeName = gradeName ?? "Unknown Grade";

            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> StudentDetails(string id)
        {
            var student = await _context.Users
                .Include(u => u.Grade)
                .Where(u => u.Id == id)
                .Select(u => new StudentDetailsViewModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Gender = u.Gender,
                    Address = u.Address,
                    PhoneNumber = u.PhoneNumber,
                    DateOfBirth = u.DateOfBirth,
                    GradeName = u.Grade.GradeName,
                    Marks = _context.Marks
                        .Where(m => m.StudentId == u.Id)
                        .Select(m => new SubjectMarksViewModel
                        {
                            SubjectName = m.Subject.SubjectName,
                            Score = (int)m.Score
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}
