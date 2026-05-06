using GuestBook.Models;

namespace GuestBook.Services;

public interface IUserService
{
    /// <summary>
    /// Повертає користувача за логіном або null, якщо не знайдено.
    /// </summary>
    Task<User?> GetByLoginAsync(string login);

    /// <summary>
    /// Перевіряє, чи існує користувач із таким логіном.
    /// </summary>
    Task<bool> LoginExistsAsync(string login);

    /// <summary>
    /// Реєструє нового користувача. Генерує сіль і хешує пароль перед збереженням.
    /// </summary>
    Task CreateAsync(string login, string plainPassword);

    /// <summary>
    /// Перевіряє пароль користувача: повертає true, якщо пароль вірний.
    /// </summary>
    bool VerifyPassword(User user, string plainPassword);
}
