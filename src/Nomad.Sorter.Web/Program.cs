using Nomad.Sorter.Application.Extensions;
using Nomad.Sorter.Domain.Extensions;
using Nomad.Sorter.Infrastructure.Extensions;
using Nomad.Sorter.Web.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddNomadSorterApplication();
services.AddNomadSorterDomain();
services.AddNomadSorterInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Nomad.Sorter");

app.MapDemoRoutes();

app.Run();

public partial class Program {}
