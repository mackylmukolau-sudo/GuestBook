using Microsoft.AspNetCore.Mvc;
using GuestBook.Models;
using GuestBook.Services;

namespace GuestBook.Controllers;

public class HomeController : Controller
{
    private readonly IMessageService _messages;

    public HomeController(IMessageService messages) => _messages = messages;

    // ── GET / ─────────────────────────────────────────────────────────────
    public async Task<IActionResult> Index()
    {
        ViewBag.Messages = await _messages.GetAllAsync();
        return View(new PostMessageViewModel());
    }

    // ── POST / — додати повідомлення (тільки для авторизованих) ──────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(PostMessageViewModel vm)
    {
        if (HttpContext.Session.GetInt32("UserId") is not int userId)
            return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid)
        {
            ViewBag.Messages = await _messages.GetAllAsync();
            return View(vm);
        }

        await _messages.AddAsync(userId, vm.Text);
        return RedirectToAction(nameof(Index));
    }
}
