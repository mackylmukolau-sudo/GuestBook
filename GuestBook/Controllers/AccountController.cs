using Microsoft.AspNetCore.Mvc;
using GuestBook.Models;
using GuestBook.Services;

namespace GuestBook.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _users;

    public AccountController(IUserService users) => _users = users;

    // ── GET /Account/Login ────────────────────────────────────────────────
    public IActionResult Login() => View(new LoginViewModel());

    // ── POST /Account/Login ───────────────────────────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var user = await _users.GetByLoginAsync(vm.Login);

        if (user is null || !_users.VerifyPassword(user, vm.Password))
        {
            ModelState.AddModelError(string.Empty, "Невірний логін або пароль.");
            return View(vm);
        }

        HttpContext.Session.SetInt32("UserId",    user.Id);
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

        if (await _users.LoginExistsAsync(vm.Login))
        {
            ModelState.AddModelError("Login", "Цей логін вже зайнятий.");
            return View(vm);
        }

        await _users.CreateAsync(vm.Login, vm.Password);

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
