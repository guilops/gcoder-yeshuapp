using ManagerTruck.API.Configuration.ExtensionsMethods;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();
builder.AddVersioning();
builder.AddDbContextEFCore();
builder.AddSecurityControllers();

var app = builder.Build();

app.UseCorsPipeline();
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseDocumentationAndSecurity();
app.RunAppDbContext();
await EFCoreExtensions.SeedAdminAsync(app);
app.Run();

