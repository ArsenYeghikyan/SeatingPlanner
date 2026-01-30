using System.ComponentModel.DataAnnotations;

namespace EventSeatingPlanner.App.Models;

public sealed class RegisterFormModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string ConfirmPassword { get; set; } = string.Empty;
}
