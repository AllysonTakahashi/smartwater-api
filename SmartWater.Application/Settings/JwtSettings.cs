using System.ComponentModel.DataAnnotations;

namespace SmartWater.Application.Settings;

public class JwtSettings
{
    public const string SectionName = "Jwt";

    [Required, MinLength(32)]
    public string Key { get; init; } = string.Empty;

    [Required]
    public string Issuer { get; init; } = string.Empty;

    [Required]
    public string Audience { get; init; } = string.Empty;

    public int ExpirationHours { get; init; } = 2;
}
