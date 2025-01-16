using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class MarksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MarksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> AddMarks()
        {
            var model = new AddMarksViewModel
            {
                Grades = await _context.Grades.Select(g => new SelectListItem
                {
                    Value = g.GradeId.ToString(),
                    Text = g.GradeName
                }).ToListAsync(),
                Subjects = new List<SelectListItem>(),
                Students = new List<StudentMarksViewModel>()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddMarks(AddMarksViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.GradeId == null || model.SubjectId == null)
                {
                    ModelState.AddModelError("", "Please select a Grade and Subject.");
                    return View(await PrepareViewModel(model));
                }

                model.Students ??= new List<StudentMarksViewModel>();
                var validStudents = model.Students.Where(s => s.Marks.HasValue).ToList();

                if (!validStudents.Any())
                {
                    ModelState.AddModelError("", "Please enter marks for at least one student.");
                    return View(await PrepareViewModel(model));
                }

                foreach (var student in validStudents)
                {
                    var marks = new Marks
                    {
                        StudentId = student.StudentId,
                        SubjectId = model.SubjectId,
                        GradeId = model.GradeId.Value,
                        Score = student.Marks.Value
                    };
                    _context.Marks.Add(marks);
                }


                await _context.SaveChangesAsync();

                // Set a success message
                TempData["SuccessMessage"] = "Marks have been successfully saved.";

                // Redirect to a relevant page (for example, showing the student marks for the selected grade and subject)
                return RedirectToAction("StudentMarks", new { gradeId = model.GradeId, subjectId = model.SubjectId });
                //return RedirectToAction("StudentMarks");
            }

            return View(await PrepareViewModel(model));
        }


        private async Task<AddMarksViewModel> PrepareViewModel(AddMarksViewModel model)
        {
            // Populate Grades dropdown
            var grades = await _context.Grades.ToListAsync();
            model.Grades = grades.Select(g => new SelectListItem
            {
                Value = g.GradeId.ToString(),
                Text = g.GradeName
            }).ToList();

            // Populate Subjects dropdown
            if (model.GradeId.HasValue)
            {
                var subjects = await _context.Subjects
                    .Where(s => s.GradeId == model.GradeId)
                    .ToListAsync();
                model.Subjects = subjects.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.SubjectName
                }).ToList();
            }

            // Populate Students table
            if (model.GradeId.HasValue)
            {
                model.Students = await _context.Users
                    .Where(u => u.GradeId == model.GradeId)
                    .Select(u => new StudentMarksViewModel
                    {
                        StudentId = u.Id,
                        FullName = $"{u.FirstName} {u.LastName}",
                        Marks = null  // Initialize marks as null for new entries
                    })
                    .ToListAsync();
            }

            return model;
        }

        public async Task<IActionResult> StudentMarks(int gradeId, int subjectId)
        {
            var subjectName = await _context.Subjects
                .Where(s => s.Id == subjectId)
                .Select(s => s.SubjectName)
                .FirstOrDefaultAsync();

            var gradeName = await _context.Grades
                .Where(g => g.GradeId == gradeId)
                .Select(g => g.GradeName)
                .FirstOrDefaultAsync();

            var studentsMarks = await _context.Marks
                .Where(m => m.GradeId == gradeId && m.SubjectId == subjectId)
                .Select(m => new StudentMarksViewModel
                {
                    StudentId = m.StudentId,
                    FullName = _context.Users
                                .Where(u => u.Id == m.StudentId)
                                .Select(u => u.FirstName + " " + u.LastName)
                                .FirstOrDefault(),
                    Marks = (int)m.Score
                })
                .ToListAsync();

            var viewModel = new StudentMarksListViewModel
            {
                GradeId = gradeId,
                GradeName = gradeName,
                SubjectId = subjectId,
                SubjectName = subjectName,
                StudentsMarks = studentsMarks
            };

            return View(viewModel);
        }

        public async Task<IActionResult> MyDetails()
        {
            var userId = _userManager.GetUserId(User); // Get logged-in user's ID
            var student = await _context.Users
                .Include(u => u.Grade)
                .Where(u => u.Id == userId)
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

        [HttpPost]
        /*public async Task<IActionResult> EditMarks(StudentMarksListViewModel model)
        {


        }*/
        [HttpPost]
        public IActionResult DeleteMarks(int id)
        {
            var mark = _context.Marks.FirstOrDefault(m => m.Id == id);

            if (mark != null)
            {
                _context.Marks.Remove(mark);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Marks deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Marks not found.";
            }

            return RedirectToAction("index");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
