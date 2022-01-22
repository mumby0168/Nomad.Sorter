using Convey.CQRS.Commands;
using Nomad.Sorter.Application.Commands;

namespace Nomad.Sorter.Web.Endpoints;

public static class BayEndpoints
{
    public static IEndpointRouteBuilder MapBayEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/bays", CreateBay);
        return builder;
    }

    private static Task CreateBay(CreateBayCommand createBayCommand, ICommandDispatcher commandDispatcher) =>
        commandDispatcher.SendAsync(createBayCommand);
}