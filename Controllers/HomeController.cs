using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Identity;

namespace AccountingProgram.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<User> _userManager;

    public HomeController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        ViewBag.FirstName = user?.FirstName;
        return View();
    }

    public IActionResult About()
    {
        return View();

    }

    public IActionResult AccessDenied()
    {
        var referer = Request.Headers["Referer"].ToString();
        ViewData["RefererUrl"] = referer;
        return View();
    }
}