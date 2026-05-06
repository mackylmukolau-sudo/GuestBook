using System.ComponentModel.DataAnnotations;

namespace GuestBook.Models;

public class Message
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    [MaxLength(2000)]
    public string Text { get; set; } = string.Empty;

    public DateTime DateTime { get; set; } = DateTime.Now;
}
