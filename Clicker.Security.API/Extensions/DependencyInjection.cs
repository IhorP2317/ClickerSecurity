using System.Text;
using Carter;
using Clicker.Security.API.Filters;
using Clicker.Security.API.Middlewares;
using Clicker.Security.BL.Abstractions;
using Clicker.Security.BL.Implementations;
using Clicker.Security.DAL.Data;
using Clicker.Security.DAL.Models;
using Clicker.Security.Domain.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Clicker.Security.API.Extensions;

public static class DependencyInjection
{

    public static void ConfigureSqlConnection(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(opts =>
            opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
    }
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));
        
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddCarter();
        
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddScoped<ValidateModelFilter>();
        services.AddTransient<GlobalExceptionHandlingMiddleware>();
        
    }
    public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration) {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["AuthSettings:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = configuration["AuthSettings:Audience"],

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["AuthSettings:SecretKey"])),
                };
            });
    }

    public static void ConfigureCors(this IServiceCollection services) 
    {
        services.AddCors(options => {
            options.AddPolicy("AllowAny", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}