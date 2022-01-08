using MediatR;
using Nomad.Sorter.Application.Commands;

namespace Nomad.Sorter.Web.Endpoints;

public static class DemoEndpoints
{
    public static IEndpointRouteBuilder MapDemoRoutes(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/demo/parcel-pre-advice",
            (ParcelPreAdviceCommand command, IMediator mediator) => mediator.Publish(command));

        return routes;
    }
}