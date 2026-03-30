using Ticaga.Api.Features.Auth;
using Ticaga.Api.Features.Rooms;
using Ticaga.Api.Features.Users;

namespace Ticaga.Api.Endpoints;

public static class EndpointMappings
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAuthEndpoints();
        app.MapUserEndpoints();
        app.MapRoomEndpoints();

        return app;
    }
}