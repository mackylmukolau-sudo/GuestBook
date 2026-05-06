using Microsoft.EntityFrameworkCore;
using GuestBook.Data;
using GuestBook.Models;

namespace GuestBook.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db) => _db = db;

    public Task<User?> GetByLoginAsync(string login) =>
        _db.Users.FirstOrDefaultAsync(u => u.Login == login);

    public Task<bool> LoginExistsAsync(string login) =>
        _db.Users.AnyAsync(u => u.Login == login);

    public async Task CreateAsync(string login, string plainPassword)
    {
        // Генеруємо унікальну сіль за допомогою BCrypt
        string salt         = BCrypt.Net.BCrypt.GenerateSalt(workFactor: 12);
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword, salt);

        _db.Users.Add(new User
        {
            Login        = login,
            Salt         = salt,
            PasswordHash = passwordHash
        });

        await _db.SaveChangesAsync();
    }

    public bool VerifyPassword(User user, string plainPassword)
    {
        // Хешуємо введений пароль із збереженою сіллю і порівнюємо з хешем у БД
        string computed = BCrypt.Net.BCrypt.HashPassword(plainPassword, user.Salt);
        return computed == user.PasswordHash;
    }
}
