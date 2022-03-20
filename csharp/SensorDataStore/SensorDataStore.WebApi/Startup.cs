using DataStore.Library.Abstractions;
using DataStore.Library.Data;
using DataStore.Library.DbAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

namespace SensorDataStore.WebApi;

internal static class Startup
{
    internal static void ConfigureServices(this IServiceCollection services)
    {
        services.AddControllers();
        //services.AddAuthorization();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test01", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."

            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
        });
        //services.AddSwaggerGen(c =>
        //{
        //    var securityScheme = new OpenApiSecurityScheme
        //    {
        //        Name = "JWT Authentication", 
        //        Description = "Enter JWT Bearer token",
        //        In = ParameterLocation.Header, 
        //        Type = SecuritySchemeType.Http, 
        //        Scheme = "bearer", 
        //        BearerFormat = "JWT", 
        //        Reference = new OpenApiReference()
        //        {
        //            Type = ReferenceType.SecurityScheme,
        //            Id = "Bearer"
        //        }
        //    },

        //});

        services.AddAuthorization(options =>
        {
            options.AddPolicy("GetAccess", policy =>
            {
               // policy.RequireClaim(ClaimTypes.Name, "string");
                //policy.RequireUserName("string");
                policy.RequireRole("admin");
            });
        });

        services.AddLogging(x => x.ConfigureLogger());
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                   // ValidAudience = "Client", //.Configuration["Jwt:Audience"],
                    //ValidIssuer = "Server", //.Configuration["Jwt:Issuer"],
                    IssuerSigningKey =
                       new SymmetricSecurityKey(Encoding.UTF8.GetBytes("GetTheKeyFromTheConfigurationAfterExtraction"))

                };
            }
        );

       

        services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        }));
        services
            .AddSingleton<ICouchbaseDataAccess, CouchbaseDataAccess>()
            .AddScoped<ISensorData, SensorData>();
    }

    internal static ILoggingBuilder ConfigureLogger(this ILoggingBuilder loggingBuilder)
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string logPath = Path.Combine(basePath, "logs", "my_logNew.log");
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
            .WriteTo.Console()
            .MinimumLevel.Debug()                                               //ToDo make dependent on the current environment.
            .CreateLogger();
        loggingBuilder.AddSerilog(Log.Logger);
        return loggingBuilder;
    }
}
