using Ticaga.Api.Endpoints.Auth;
using Ticaga.Api.Endpoints.Rooms;
using Ticaga.Api.Endpoints.Users;

namespace Ticaga.Api;

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