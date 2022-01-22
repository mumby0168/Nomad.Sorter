using CleanArchitecture.Exceptions.AspNetCore;
using Nomad.Sorter.Application.Extensions;
using Nomad.Sorter.Domain.Extensions;
using Nomad.Sorter.Infrastructure.Extensions;
using Nomad.Sorter.Web.Endpoints;
using Nomad.Sorter.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilog();

var services = builder.Services;

services.AddCleanArchitectureExceptionsHandler(options =>
{
    options.ApplicationName = "Nomad.Sorter";
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddNomadSorterApplication();
services.AddNomadSorterDomain();
services.AddNomadSorterInfrastructure(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseCleanArchitectureExceptionsHandler();

app.MapGet("/", () => "Nomad.Sorter");
app.MapBayEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

public partial class Program {}
