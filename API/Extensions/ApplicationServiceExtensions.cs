using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services,
        IConfiguration configuration)
        {

            services.AddControllers();
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            //Repository 
            services.AddScoped<IUserRepository,UserRepository>();
            //PhotoUpload
            services.AddScoped<IPhotoServices,PhotoServices>();
            services.AddAutoMapper(typeof(MapperProfile)); 
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

            return services;
        }
    }
}