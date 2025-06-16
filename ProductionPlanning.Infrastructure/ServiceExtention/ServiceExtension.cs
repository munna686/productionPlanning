using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProductionPlanning.Core.Interfaces;
using ProductionPlanning.Core.Model;
using ProductionPlanning.Infrastructure.DbContext;
using ProductionPlanning.Infrastructure.Repos;
using ProductionPlanning.Service.Interface;
using ProductionPlanning.Service.Services;
using System.Text;

namespace ProductionPlanning.Infrastructure.ServiceExtention
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InventoryDataContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("Inv"));
            });
            //configure identity
            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 4;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<InventoryDataContext>()
            .AddDefaultTokenProviders();
            // Add Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Add Repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBomRepository, BomRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IRawMaterialRepository, RawMaterialRepository>();
            services.AddScoped<IBomLogRepository, BomLogRepository>();

            //Add Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IBomService, BomService>();
            services.AddScoped<IAuthService, AuthService>();
            // Add Authentication and JWT Configuration
            // Add Authentication with JWT Bearer
            var jwtSettings = configuration.GetSection("Jwt");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                };
            });

            // Add Authorization
            services.AddAuthorization();

            services.AddControllers();
            return services;
        }
    }
}
