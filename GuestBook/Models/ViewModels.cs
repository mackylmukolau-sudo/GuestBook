using System.ComponentModel.DataAnnotations;
using GuestBook.Validation;

namespace GuestBook.Models;

// ── Login ─────────────────────────────────────────────────────────────────
public class LoginViewModel
{
    [Required(ErrorMessage = "Введіть логін.")]
    [MaxLength(100, ErrorMessage = "Логін не може перевищувати 100 символів.")]
    [Display(Name = "Логін")]
    public string Login { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введіть пароль.")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = string.Empty;
}

// ── Register ──────────────────────────────────────────────────────────────
public class RegisterViewModel
{
    [Required(ErrorMessage = "Логін є обов'язковим.")]
    [MinLength(3, ErrorMessage = "Логін має містити щонайменше 3 символи.")]
    [MaxLength(100, ErrorMessage = "Логін не може перевищувати 100 символів.")]
    [NoSpaces(ErrorMessage = "Логін не може містити пробіли.")]
    [Display(Name = "Логін")]
    public string Login { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль є обов'язковим.")]
    [MinLength(6, ErrorMessage = "Пароль має містити щонайменше 6 символів.")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Підтвердження паролю є обов'язковим.")]
    [Compare("Password", ErrorMessage = "Паролі не збігаються.")]
    [DataType(DataType.Password)]
    [Display(Name = "Підтвердіть пароль")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

// ── Post message ──────────────────────────────────────────────────────────
public class PostMessageViewModel
{
    [Required(ErrorMessage = "Повідомлення не може бути порожнім.")]
    [MinLength(2, ErrorMessage = "Повідомлення має містити щонайменше 2 символи.")]
    [MaxLength(2000, ErrorMessage = "Повідомлення не може перевищувати 2000 символів.")]
    [Display(Name = "Повідомлення")]
    public string Text { get; set; } = string.Empty;
}
