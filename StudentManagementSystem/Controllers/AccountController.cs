using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var viewModel = new RegisterViewModel
            {
                // Load available grades from database
                AvailableGrades = _context.Grades
                    .Select(g => new SelectListItem
                    {
                        Value = g.GradeId.ToString(),
                        Text = g.GradeName
                    }).ToList(),
                Roles = _roleManager.Roles.ToList()  // Get all roles from the database
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    GradeId = model.Role == "Student" ? model.GradeId : null
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign the selected role to the user
                    await _userManager.AddToRoleAsync(user, model.Role);

                    // If the role is "Student", assign a grade to the user
                    if (model.Role == "Student" && model.GradeId.HasValue)
                    {
                        user.GradeId = model.GradeId.Value;
                        await _userManager.UpdateAsync(user);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // Reload grades and roles if the model is invalid
            model.AvailableGrades = _context.Grades
                .Select(g => new SelectListItem
                {
                    Value = g.GradeId.ToString(),
                    Text = g.GradeName
                }).ToList();
            model.Roles = _roleManager.Roles.ToList();

            return View(model);
        }
    


       [HttpGet]
        public IActionResult Login(string? ReturnUrl = null)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,lockoutOnFailure:false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                      
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login attempt");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [AllowAnonymous]
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> IsEmailAvailable(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {Email} is already in use.");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
