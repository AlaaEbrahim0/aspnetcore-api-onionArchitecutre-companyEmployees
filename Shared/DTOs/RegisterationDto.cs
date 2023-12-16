using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs;
public record RegisterationDto
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    [Required(ErrorMessage = "Username is required")]
    public string? Username { get; init; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; init; }

    public string? Email { get; init; }

    public string? PhoneNumber { get; init; }

    public ICollection<string>? Roles { get; init; }
}
