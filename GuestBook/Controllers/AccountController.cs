using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuestBook.Data;
using GuestBook.Models;

namespace GuestBook.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _db;

    public AccountController(AppDbContext db) => _db = db;

    // ── GET /Account/Login ────────────────────────────────────────────────
    public IActionResult Login() => View(new LoginViewModel());

    // ── POST /Account/Login ───────────────────────────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Login == vm.Login);

        if (user is null || !BCrypt.Net.BCrypt.Verify(vm.Password, user.PasswordHash))
        {
            ModelState.AddModelError(string.Empty, "Невірний логін або пароль.");
            return View(vm);
        }

        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("UserLogin", user.Login);

        return RedirectToAction("Index", "Home");
    }

    // ── GET /Account/Register ─────────────────────────────────────────────
    public IActionResult Register() => View(new RegisterViewModel());

    // ── POST /Account/Register ────────────────────────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        // Check login uniqueness
        bool exists = await _db.Users.AnyAsync(u => u.Login == vm.Login);
        if (exists)
        {
            ModelState.AddModelError("Login", "Цей логін вже зайнятий.");
            return View(vm);
        }

        _db.Users.Add(new User
        {
            Login        = vm.Login,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(vm.Password)
        });
        await _db.SaveChangesAsync();

        TempData["Success"] = "Реєстрація успішна! Тепер увійдіть у систему.";
        return RedirectToAction(nameof(Login));
    }

    // ── POST /Account/Logout ──────────────────────────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
