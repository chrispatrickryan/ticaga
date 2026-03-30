using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticaga.Api.Features.Auth.Register;
using Ticaga.Domain.Users;

namespace Ticaga.Api.Features.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/auth").WithTags("Auth");

        group.MapPost("/register", RegisterAsync)
            .WithName("RegisterUser")
            .WithSummary("Registers a new user account.")
            .WithDescription("Creates a new Ticaga user account using email, display name, and password.")
            .Produces<RegisterUserResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);

        return endpoints;
    }

    private static async Task<IResult> RegisterAsync(
        RegisterUserRequest request,
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher,
        CancellationToken cancellationToken)
    {
        var errors = RegisterUserRequestValidator.Validate(request);

        if (errors.Count > 0)
        {
            return Results.ValidationProblem(errors);
        }

        var canonicalEmail = User.CanonicalizeEmail(request.Email);
        var trimmedDisplayName = request.DisplayName.Trim();

        var existingUserByEmail = await userRepository.GetByEmailAsync(canonicalEmail, cancellationToken);
        if (existingUserByEmail is not null)
        {
            return Results.Conflict(new
            {
                Message = "Unable to register with the provided credentials."
            });
        }

        var displayNameExists = await userRepository.ExistsByDisplayNameAsync(trimmedDisplayName, cancellationToken);
        if (displayNameExists)
        {
            return Results.Conflict(new
            {
                Message = $"A user with the display name '{trimmedDisplayName}' already exists."
            });
        }

        var user = User.Register(canonicalEmail, trimmedDisplayName, DateTime.UtcNow);

        var passwordHash = passwordHasher.HashPassword(user, request.Password);
        user.SetPasswordHash(passwordHash);

        try
        {
            await userRepository.AddAsync(user, cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Results.Conflict(new
            {
                Message = "Registration failed because the email or display name already exists."
            });
        }

        var response = new RegisterUserResponse(
            user.Id,
            user.Email,
            user.DisplayName,
            user.CreatedUtc);

        return Results.Created($"/users/{user.Id}", response);
    }
}