using Microsoft.EntityFrameworkCore;
using GuestBook.Data;
using GuestBook.Models;

namespace GuestBook.Services;

public class MessageService : IMessageService
{
    private readonly AppDbContext _db;

    public MessageService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<Message>> GetAllAsync() =>
        await _db.Messages
            .Include(m => m.User)
            .OrderByDescending(m => m.DateTime)
            .ToListAsync();

    public async Task AddAsync(int userId, string text)
    {
        _db.Messages.Add(new Message
        {
            UserId   = userId,
            Text     = text.Trim(),
            DateTime = DateTime.Now
        });

        await _db.SaveChangesAsync();
    }
}
