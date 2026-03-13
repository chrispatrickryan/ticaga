using Ticaga.Domain.Users;

namespace Ticaga.Api.Users;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/users").WithTags("Users");

        group.MapPost("/", CreateUserAsync)
            .WithName("CreateUser")
            .WithSummary("Creates a new user.")
            .WithDescription("Creates a new user with a unique display name and saves it to the database.")
            .Produces<UserResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);

        group.MapGet("/{id:guid}", GetUserByIdAsync)
            .WithName("GetUserById")
            .WithSummary("Retrieves a user by ID.")
            .WithDescription("Retrieves a user by their unique ID. Returns 404 if the user does not exist.")
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    private static async Task<IResult> CreateUserAsync(
        CreateUserRequest request, 
        IUserRepository userRepository, 
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.DisplayName))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["DisplayName"] = ["DisplayName is required and cannot be whitespace."]
            });
        }

        var normalizedDisplayName = request.DisplayName.Trim();
        var displayNameExists = await userRepository.ExistsByDisplayNameAsync(normalizedDisplayName, cancellationToken);

        if (displayNameExists)
        {
            return Results.Conflict(new
            {
                Message = $"A user with the display name '{normalizedDisplayName}' already exists."
            });
        }

        var user = new User(Guid.NewGuid(), normalizedDisplayName, DateTime.UtcNow);
        await userRepository.AddAsync(user, cancellationToken);

        var response = new UserResponse(user.Id, user.DisplayName, user.CreatedUtc);
        return Results.Created($"/users/{user.Id}", response);
    }

    private static async Task<IResult> GetUserByIdAsync(
        Guid id, 
        IUserRepository userRepository, 
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);

        if (user is null)
        {
            return Results.NotFound(new
            {
                Message = $"No user found with ID '{id}'."
            });
        }

        var response = new UserResponse(user.Id, user.DisplayName, user.CreatedUtc);
        return Results.Ok(response);
    }
}
