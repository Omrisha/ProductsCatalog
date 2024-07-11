using System.Text.Json.Serialization;
using DomainModel.Impl;
using DomainModel.Impl.MappingProfile;
using DomainModel.Interfaces;
using EntityModel;
using Infrastructure.PersistenceModel.MongoDB;
using Infrastructure.PersistenceModel.MongoDB.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PersistanceModel.MongoDB;

namespace ApplicationModel;

public static class Extensions
{
    public static IServiceCollection AddAppExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        
        services.RegisterSwagger();
        
        services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));

        services.Configure<WebApiOptions>(configuration.GetSection(nameof(WebApiOptions)));
        
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICatalogRepository, CatalogRepository>();
        
        services.AddAutoMapper(typeof(DomainModelMappingProfile));
        
        services.AddScoped<IProductService, ProductService>();
        
        return services;
    }
    
    /// <summary>
    /// Configure web application.
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/> instance.</param>
    /// <returns>A given <see cref="WebApplication"/> instance.</returns>
    public static WebApplication ConfigureWebApplication(this WebApplication app)
    {
        app.UseForwardedHeaders();

        app.UseRouting();
        app.UseDefaultFiles();
        app.UseStaticFiles();

        string[] allowedOrigins = app.Services.GetRequiredService<IOptionsMonitor<WebApiOptions>>().CurrentValue.AllowedOrigins;
        app.UseCors(
            options => options
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins(allowedOrigins));

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.ConfigObject.AdditionalItems.Add("syntaxHighlight", false);
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Docs");
            c.RoutePrefix = "docs";
        });

        app.UseCors("AllowedOrigins");

        app.MapControllers();

        return app;
    }
    
    private static IServiceCollection RegisterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(setup =>
        {
            setup.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ProductsCatalog Api",
                Description = "ProductsCatalog API",
                Contact = new OpenApiContact
                {
                    Name = "ProductsCatalog",
                    Email = "omri.shapira@gmail.com",
                    Url = new("https://www.osphotohub.com/"),
                },
            });

            OpenApiSecurityScheme jwtSecurityScheme = new()
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "Please insert JWT with Bearer into field",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme,
                },
            };

            setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() },
            });
        });

        return services;
    }
}