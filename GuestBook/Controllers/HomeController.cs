using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuestBook.Data;
using GuestBook.Models;

namespace GuestBook.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db) => _db = db;

    // ── GET / ─────────────────────────────────────────────────────────────
    public async Task<IActionResult> Index()
    {
        var messages = await _db.Messages
            .Include(m => m.User)
            .OrderByDescending(m => m.DateTime)
            .ToListAsync();

        var vm = new PostMessageViewModel();
        ViewBag.Messages = messages;
        return View(vm);
    }

    // ── POST / — add message (only for logged-in users) ──────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(PostMessageViewModel vm)
    {
        // If not logged in — redirect
        if (HttpContext.Session.GetInt32("UserId") is not int userId)
            return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid)
        {
            ViewBag.Messages = await _db.Messages
                .Include(m => m.User)
                .OrderByDescending(m => m.DateTime)
                .ToListAsync();
            return View(vm);
        }

        _db.Messages.Add(new Message
        {
            UserId   = userId,
            Text     = vm.Text.Trim(),
            DateTime = DateTime.Now
        });
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
