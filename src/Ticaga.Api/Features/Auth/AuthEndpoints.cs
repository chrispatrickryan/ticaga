using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticaga.Api.Features.Auth.Jwt;
using Ticaga.Api.Features.Auth.Login;
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

        group.MapPost("/login", LoginAsync)
            .WithName("LoginUser")
            .WithSummary("Logs in a user and returns a JWT access token.")
            .WithDescription("Authenticates a Ticaga user using email and password.")
            .Produces<LoginUserResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/me", GetCurrentUserAsync)
            .WithName("GetCurrentUser")
            .WithSummary("Gets the currently authenticated user.")
            .WithDescription("Returns the currently authenticated Ticaga user based on the JWT bearer token.")
            .Produces<CurrentUserResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();

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

    private static async Task<IResult> LoginAsync(
        LoginUserRequest request,
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher,
        IJwtTokenService jwtTokenService,
        CancellationToken cancellationToken)
    {
        var errors = LoginUserRequestValidator.Validate(request);

        if (errors.Count > 0)
        {
            return Results.ValidationProblem(errors);
        }

        var canonicalEmail = User.CanonicalizeEmail(request.Email);

        var user = await userRepository.GetByEmailAsync(canonicalEmail, cancellationToken);

        if (user is null)
        {
            return Results.Json(
                new { Message = "Invalid email or password." },
                statusCode: StatusCodes.Status401Unauthorized);
        }

        var verificationResult = passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return Results.Json(
                new { Message = "Invalid email or password." },
                statusCode: StatusCodes.Status401Unauthorized);
        }

        var tokenResult = jwtTokenService.GenerateToken(user);

        var response = new LoginUserResponse(
            tokenResult.AccessToken,
            tokenResult.ExpiresUtc,
            user.Id,
            user.Email,
            user.DisplayName);

        return Results.Ok(response);
    }

    private static async Task<IResult> GetCurrentUserAsync(
        ClaimsPrincipal claimsPrincipal,
        IUserRepository userRepository,
        CancellationToken cancellationToken)
    {
        var userIdValue = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? claimsPrincipal.FindFirstValue("sub");

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Results.Unauthorized();
        }

        var user = await userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return Results.Unauthorized();
        }

        var response = new CurrentUserResponse(user.Id, user.Email, user.DisplayName, user.CreatedUtc);

        return Results.Ok(response);
    }
}