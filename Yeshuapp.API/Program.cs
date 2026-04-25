using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using System.Text.Json;
using Yeshuapp.Context;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------
// 🔹 Swagger
// ----------------------------
builder.Services.AddSwaggerGen(); // 👈 registra o SwaggerProvider
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

// ----------------------------
// 🔹 API Versioning
// ----------------------------
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// ----------------------------
// 🔹 Controllers + JSON Config
// ----------------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);


// ----------------------------
// 🔹 EF Core + MySQL
// ----------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// ----------------------------
// 🔹 Identity + Roles
// ----------------------------
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// ----------------------------
// 🔹 JWT Auth
// ----------------------------
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// ----------------------------
// 🔹 CORS
// ----------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(
                builder.Configuration["Jwt:Issuer"].ToString(),
                "https://localhost:7179",
                "http://localhost:5173",
                "https://localhost:5001")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

/*healthchecks*/
builder.Services.AddHealthChecks()
    .AddMySql(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        name: "mysql",
        failureStatus: HealthStatus.Unhealthy
    )
    .AddUrlGroup(
        new Uri(builder.Configuration["Jwt:Issuer"].ToString()),
        name: "frontend",
        failureStatus: HealthStatus.Unhealthy
    );

builder.Services.AddHealthChecks()
.AddCheck(
    "self",
    () => HealthCheckResult.Healthy(),
    tags: new[] { "live" }
);

var app = builder.Build();

// ----------------------------
// 🔹 Pipeline
// ----------------------------
app.UseCors("CorsPolicy");

var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    foreach (var desc in apiVersionProvider.ApiVersionDescriptions)
    {
        c.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
    }
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// por padrão exige auth em tudo, exceto se marcar [AllowAnonymous]
app.MapControllers().RequireAuthorization();

// 🔹 Seed de usuário Admin (executado ao iniciar)
await IdentitySeed.SeedAdminAsync(app);

app.MapHealthChecks("/healthCheck", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var result = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration,
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                error = e.Value.Exception?.Message,
                duration = e.Value.Duration.ToString()
            })
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
    }
});

app.Run();

// ----------------------------
// 🔹 Classes auxiliares
// ----------------------------
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var desc in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(desc.GroupName, new OpenApiInfo
            {
                Title = $"Yeshuapp API - Versão {desc.ApiVersion}",
                Version = desc.ApiVersion.ToString(),
                Description = "Documentação gerada automaticamente",
                Contact = new OpenApiContact
                {
                    Name = "Guilherme Tech Solutions",
                    Email = "guilherme.glsantos@gmail.com"
                },
            });
        }

        // 🔹 Configuração de segurança para JWT
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Insira o token JWT no formato: Bearer {token}"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                Array.Empty<string>()
            }
        });
    }
}

public static class IdentitySeed
{
    public static async Task SeedAdminAsync(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string adminEmail = "admin@igreja.com";
        string adminPassword = "Admin@123";

        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole("Admin"));

        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            await userManager.CreateAsync(admin, adminPassword);
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
