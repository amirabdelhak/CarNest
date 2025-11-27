using BLL.Manager.BodyTypeManager;
using BLL.Manager.CarManager;
using BLL.Manager.FavoriteManager;
using BLL.Manager.FuelTypeManager;
using BLL.Manager.LocationManager;
using BLL.Manager.MakeManager;
using BLL.Manager.ModelManager;
using DAL.Context;
using DAL.Entity;
using DAL.Repository;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //remeber to change the DBConnection FIRST//////////////////////////////////////////////////////////
            builder.Services.AddDbContext<CarNestDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MostafaDB")));


            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<CarNestDBContext>()
                .AddDefaultTokenProviders();

            // JWT Authentication Configuration
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                };
            });

            // CORS Configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            // Dependency Injection
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register Managers
            builder.Services.AddScoped<IModelManager, ModelManager>();
            builder.Services.AddScoped<IMakeManager, MakeManager>();
            builder.Services.AddScoped<IBodyTypeManager, BodyTypeManager>();
            builder.Services.AddScoped<ILocationManager, LocationManager>();
            builder.Services.AddScoped<IFuelTypeManager, FuelTypeManager>();
            builder.Services.AddScoped<ICarManager>(sp =>
            {
                var unitOfWork = sp.GetRequiredService<IUnitOfWork>();
                var env = sp.GetRequiredService<IWebHostEnvironment>();
                return new CarManager(unitOfWork, env.WebRootPath);
            });
            builder.Services.AddScoped<IFavoriteManager, FavoriteManager>();



            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
