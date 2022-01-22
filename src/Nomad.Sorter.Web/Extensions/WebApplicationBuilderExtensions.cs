using Serilog;

namespace Nomad.Sorter.Web.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();
        return builder;
    }
}