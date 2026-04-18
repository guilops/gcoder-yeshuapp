using Serilog;

namespace ManagerTruck.API.Configuration.ExtensionsMethods
{
    public static class SerilogExtensions
    {
        public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((hostingContext, configuration) =>
            {
                configuration.ReadFrom.Configuration(hostingContext.Configuration)
                             .Enrich.FromLogContext()
                             .Enrich.WithMachineName()
                             .Enrich.WithThreadId()
                             .WriteTo.Console()
                             .CreateLogger();
            });

            builder.Host.UseSerilog();

            return builder;
        }
    }
}