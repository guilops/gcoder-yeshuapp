using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ManagerTruck.API.Configuration.ExtensionsMethods
{
    public static class DocumentationExtensions
    {
        public static WebApplicationBuilder AddDocumentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
            return builder;
        }

        public static WebApplication UseDocumentationAndSecurity(this WebApplication app)
        {
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
            app.MapControllers().RequireAuthorization();
           
            return app;
        }

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
                        Title = $"VUC Locadora API - Versão {desc.ApiVersion}",
                        Version = desc.ApiVersion.ToString(),
                        Description = "Documentação gerada automaticamente",
                        Contact = new OpenApiContact
                        {
                            Name = "Guilherme Tech Solutions",
                            Email = "guilhermelopes_dev@hotmail.com"
                        },
                    });
                }

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
    }
}
