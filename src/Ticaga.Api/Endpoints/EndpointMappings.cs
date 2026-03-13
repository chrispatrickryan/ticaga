using Ticaga.Api.Rooms;
using Ticaga.Api.Users;

namespace Ticaga.Api.Endpoints;

public static class EndpointMappings
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapUserEndpoints();
        app.MapRoomEndpoints();

        return app;
    }
}