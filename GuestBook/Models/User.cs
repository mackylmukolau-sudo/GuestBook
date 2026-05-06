using System.ComponentModel.DataAnnotations;

namespace GuestBook.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Login { get; set; } = string.Empty;

    [Required]
    [MaxLength(256)]
    public string PasswordHash { get; set; } = string.Empty;

    // Navigation
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
