using Microsoft.Extensions.DependencyInjection;
using Ticaga.Application.Features.Auth.Login;
using Ticaga.Application.Features.Auth.Register;
using Ticaga.Application.Features.Rooms.CreateRoom;
using Ticaga.Application.Features.Rooms.GetRoomById;
using Ticaga.Application.Features.Users.GetCurrentUser;
using Ticaga.Application.Features.Users.GetUserById;

namespace Ticaga.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Auth
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginHandler>();

        // Users
        services.AddScoped<GetCurrentUserHandler>();
        services.AddScoped<GetUserByIdHandler>();

        // Rooms
        services.AddScoped<GetRoomByIdHandler>();
        services.AddScoped<CreateRoomHandler>();

        return services;
    }
}