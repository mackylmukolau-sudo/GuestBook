using GuestBook.Models;

namespace GuestBook.Services;

public interface IMessageService
{
    /// <summary>
    /// Повертає всі повідомлення разом із даними користувача, відсортовані за датою (новіші першими).
    /// </summary>
    Task<IReadOnlyList<Message>> GetAllAsync();

    /// <summary>
    /// Додає нове повідомлення від вказаного користувача.
    /// </summary>
    Task AddAsync(int userId, string text);
}
