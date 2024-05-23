using AccountingProgram.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    TempData["SuccessMessage"] = "Nastąpiło poprawne zalogowanie. Możesz teraz korzystać z aplikacji.";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login lub hasło jest błędne. Spróbuj ponownie");
                return View(model);
            }
        }

        // Jeśli doszło do tego miejsca, coś poszło nie tak - wyświetl ponownie formularz
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        TempData["SuccessMessage"] = "Nastąpiło poprawne wylogowanie z systemu. Zapraszamy ponownie";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Authorize(Roles = "Główna księgowa")]
    public IActionResult Create()
    {
        ViewBag.UserRoles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
        return View();
    }


    [HttpPost]
    [Authorize(Roles = "Główna księgowa")]
    public async Task<IActionResult> Create(UserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.UserRole);
                TempData["SuccessMessage"] = "Dodano nowego pracownika.";
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        ViewBag.UserRoles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
        return View(model);
    }


    [HttpGet]
    [Authorize(Roles = "Główna księgowa")]
    public async Task<IActionResult> Edit(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var model = new UserViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty
        };

        ViewBag.UserRoles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
        return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Główna księgowa")]
    public async Task<IActionResult> Edit(User model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            TempData["SuccessMessage"] = "Dane użytkownika zostały zaktualizowane.";
            return RedirectToAction("Index");
        }
        
        ViewBag.UserRoles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
        return View(model);
    }


    [HttpGet]
    [Authorize(Roles = "Główna księgowa")]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Pobierz rolę użytkownika, aby sprawdzić, czy jest "Główna księgowa"
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains("Główna księgowa"))
        {
            TempData["ErrorMessage"] = "Nie można usunąć użytkownika z rolą 'Główna księgowa'.";
            return RedirectToAction("Index");
        }

        return View(user);
    }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Główna księgowa")]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Sprawdź, czy użytkownik ma rolę "Główna księgowa"
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains("Główna księgowa"))
        {
            TempData["ErrorMessage"] = "Nie można usunąć użytkownika z rolą 'Główna księgowa'.";
            return RedirectToAction("Index");
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View("Index"); // Lub widok błędu
        }

        TempData["SuccessMessage"] = "Użytkownik został usunięty.";
        return RedirectToAction("Index");
    }



    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        var userRolesViewModel = new List<UserRolesViewModel>();

        foreach (var user in users)
        {
            var thisViewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Roles = await _userManager.GetRolesAsync(user)
            };
            userRolesViewModel.Add(thisViewModel);
        }

        return View(userRolesViewModel);
    }
}
