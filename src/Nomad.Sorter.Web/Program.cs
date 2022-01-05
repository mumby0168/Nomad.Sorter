using Nomad.Sorter.Application.Extensions;
using Nomad.Sorter.Domain.Extensions;
using Nomad.Sorter.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddNomadSorterApplication();
services.AddNomadSorterDomain();
services.AddNomadSorterInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Nomad.Sorter");

app.Run();
