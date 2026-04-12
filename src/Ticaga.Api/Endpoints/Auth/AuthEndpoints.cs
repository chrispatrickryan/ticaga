using System.Security.Claims;
using Ticaga.Api.Endpoints.Auth.Dto;
using Ticaga.Application.Common;
using Ticaga.Application.Features.Auth.Login;
using Ticaga.Application.Features.Auth.Login.Dto;
using Ticaga.Application.Features.Auth.Register;
using Ticaga.Application.Features.Auth.Register.Dto;
using Ticaga.Application.Features.Users.GetCurrentUser;

namespace Ticaga.Api.Endpoints.Auth;

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
            .Produces<LoginResponse>(StatusCodes.Status200OK)
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
        RegisterUserHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(request.Email, request.DisplayName, request.Password);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorType switch
            {
                TicagaErrorType.Validation => Results.ValidationProblem(result.ValidationErrors!),
                TicagaErrorType.Conflict => Results.Conflict(result.ErrorMessage),
                _ => Results.Problem(result.ErrorMessage)
            };
        }

        var response = new RegisterUserResponse(
            result.Value!.Id,
            result.Value.Email,
            result.Value.DisplayName,
            result.Value.CreatedUtc);

        return Results.Created($"/users/{result.Value.Id}", response);
    }

    private static async Task<IResult> LoginAsync(
        LoginRequest request,
        LoginHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorType switch
            {
                TicagaErrorType.Validation => Results.ValidationProblem(result.ValidationErrors!),

                TicagaErrorType.Unauthorized => Results.Json(
                    new { message = result.ErrorMessage ?? "Invalid email or password." },
                    statusCode: StatusCodes.Status401Unauthorized),

                TicagaErrorType.Conflict => Results.Conflict(new
                {
                    message = result.ErrorMessage
                }),

                TicagaErrorType.NotFound => Results.NotFound(new
                {
                    message = result.ErrorMessage
                }),

                _ => Results.Problem(result.ErrorMessage)
            };
        }

        var response = new LoginResponse(
            result.Value!.AccessToken,
            result.Value.ExpiresUtc,
            result.Value.UserId,
            result.Value.Email,
            result.Value.DisplayName);

        return Results.Ok(response);
    }

    private static async Task<IResult> GetCurrentUserAsync(
        ClaimsPrincipal claimsPrincipal,
        GetCurrentUserHandler handler,
        CancellationToken cancellationToken)
    {
        var userIdValue = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? claimsPrincipal.FindFirstValue("sub");

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = await handler.HandleAsync(userId, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorType switch
            {
                TicagaErrorType.Unauthorized => Results.Unauthorized(),
                _ => Results.Problem(result.ErrorMessage)
            };
        }

        var response = new CurrentUserResponse(
            result.Value!.Id,
            result.Value.Email,
            result.Value.DisplayName,
            result.Value.CreatedUtc);

        return Results.Ok(response);
    }
}