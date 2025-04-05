using System.Text;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Middleware;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
// builder.Services.AddControllers();
// builder.Services.AddDbContext<DataContext>(opt=>
// {
//     opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
// });
// builder.Services.AddCors();

// builder.Services.AddScoped<ITokenService,TokenService>();

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options=>{
//         var tokenKey=builder.Configuration["TokenKey"]?? throw new Exception("TokenKey not found");
//         options.TokenValidationParameters=new TokenValidationParameters{
//             ValidateIssuerSigningKey=true,
//             IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
//             ValidateIssuer=false,
//             ValidateAudience=false
//         };
//     });


var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
//app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().WithOrigins("http://localhost:4200","https://localhost:4200")); having issue with CORS
app.UseCors(x => x.WithOrigins("http://localhost:4200", "https://localhost:4200") // Allow specific origins
    .AllowAnyHeader()  // Allow any headers (needed for JSON requests)
    .AllowAnyMethod()  // Allow GET, POST, PUT, DELETE, etc.
    .AllowCredentials());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using var scope=app.Services.CreateScope();

var services=scope.ServiceProvider;
try{
    var context=services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}
catch(Exception ex)
{
    var logger=services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex,"An error occurred during migrations");
}

app.Run();
