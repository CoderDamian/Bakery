using AppliationService.Contracts;
using AppliationService.Mappings;
using AppliationService.Services;
using DataPersistence;
using DataPersistence.Contracts;
using DataPersistence.Repositories;
using DomainModel.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.Text;

namespace RESTFul.Extensions
{
    public static class ServicesExtension
    {
        public static void AddBakeryService(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
        }

        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt => {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidAudience = configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]!))
                    };
            });
        }

        public static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(TokenProfile));
            services.AddAutoMapper(typeof(UserProfile));
        }

        public static void AddDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionStrings:OracleDB"];

            services.AddDbContext<BakeryDbContext>(opt => opt.UseOracle(connectionString));
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
