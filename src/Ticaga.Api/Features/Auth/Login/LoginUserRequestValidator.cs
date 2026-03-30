using System.ComponentModel.DataAnnotations;

namespace Ticaga.Api.Features.Auth.Login;

public static class LoginUserRequestValidator
{
    public static Dictionary<string, string[]> Validate(LoginUserRequest request)
    {
        var errors = new Dictionary<string, List<string>>();

        ValidateEmail(request.Email, errors);
        ValidatePassword(request.Password, errors);

        return errors.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.ToArray());
    }

    private static void ValidateEmail(string? emailInput, Dictionary<string, List<string>> errors)
    {
        const string key = nameof(LoginUserRequest.Email);

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
        }
    }

    private static void ValidatePassword(string? passwordInput, Dictionary<string, List<string>> errors)
    {
        const string key = nameof(LoginUserRequest.Password);

        if (string.IsNullOrWhiteSpace(passwordInput))
        {
            AddError(errors, key, "Password is required.");
            return;
        }

        if (passwordInput.Length > 100)
        {
            AddError(errors, key, "Password must be 100 characters or fewer.");
        }
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
}