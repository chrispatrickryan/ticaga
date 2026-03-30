using System.ComponentModel.DataAnnotations;

namespace Ticaga.Api.Features.Auth.Register;

public static class RegisterUserRequestValidator
{
    public static Dictionary<string, string[]> Validate(RegisterUserRequest request)
    {
        var errors = new Dictionary<string, List<string>>();

        ValidateEmail(request.Email, errors);
        ValidateDisplayName(request.DisplayName, errors);
        ValidatePassword(request.Password, errors);

        return ToErrorDictionary(errors);
    }

    private static void ValidateEmail(string? emailInput, Dictionary<string, List<string>> errors)
    {
        const string key = nameof(RegisterUserRequest.Email);

        if (string.IsNullOrWhiteSpace(emailInput))
        {
            AddError(errors, key, "Email is required.");
            return;
        }

        var email = emailInput.Trim().ToLowerInvariant();

        if (email.Length > 256)
        {
            AddError(errors, key, "Email must be 256 characters or fewer.");
        }

        var emailValidator = new EmailAddressAttribute();
        if (!emailValidator.IsValid(email))
        {
            AddError(errors, key, "Email format is invalid.");
            return;
        }

        if (!IsEmailDomainValid(email))
        {
            AddError(errors, key, "Email domain is invalid.");
        }
    }

    private static void ValidateDisplayName(string? displayNameInput, Dictionary<string, List<string>> errors)
    {
        const string key = nameof(RegisterUserRequest.DisplayName);

        if (string.IsNullOrWhiteSpace(displayNameInput))
        {
            AddError(errors, key, "Display name is required.");
            return;
        }

        var displayName = displayNameInput.Trim();

        if (displayName.Length < 3 || displayName.Length > 50)
        {
            AddError(errors, key, "Display name must be between 3 and 50 characters.");
        }
    }

    private static void ValidatePassword(string? passwordInput, Dictionary<string, List<string>> errors)
    {
        const string key = nameof(RegisterUserRequest.Password);

        if (string.IsNullOrWhiteSpace(passwordInput))
        {
            AddError(errors, key, "Password is required.");
            return;
        }

        if (passwordInput.Length < 8 || passwordInput.Length > 100)
        {
            AddError(errors, key, "Password must be between 8 and 100 characters.");
        }
    }

    private static bool IsEmailDomainValid(string email)
    {
        var parts = email.Split('@');

        if (parts.Length != 2)
        {
            return false;
        }

        var domain = parts[1];

        if (!domain.Contains('.'))
        {
            return false;
        }

        if (domain.StartsWith('.') || domain.EndsWith('.'))
        {
            return false;
        }

        var lastDotIndex = domain.LastIndexOf('.');
        var tld = domain[(lastDotIndex + 1)..];

        return !string.IsNullOrWhiteSpace(tld);
    }

    private static void AddError(Dictionary<string, List<string>> errors, string key, string message)
    {
        if (!errors.TryGetValue(key, out var list))
        {
            list = [];
            errors[key] = list;
        }

        list.Add(message);
    }

    private static Dictionary<string, string[]> ToErrorDictionary(Dictionary<string, List<string>> errors)
    {
        return errors.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.ToArray());
    }
}