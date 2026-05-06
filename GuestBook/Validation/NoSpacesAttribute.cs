using System.ComponentModel.DataAnnotations;

namespace GuestBook.Validation;

/// <summary>
/// Кастомний атрибут: поле не може містити пробіли.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class NoSpacesAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (value is string str && str.Contains(' '))
            return new ValidationResult(ErrorMessage ?? "Поле не може містити пробіли.");

        return ValidationResult.Success;
    }
}
