using BLL.Manager.BodyTypeManager;
using BLL.Manager.CarImageManager;
using BLL.Manager.FuelTypeManager;
using BLL.Manager.LocationManager;
using BLL.Manager.MakeManager;
using BLL.Manager.ModelManager;
using DAL.Context;
using DAL.Entity;
using DAL.Repository;
using DAL.Repository.CarImageRepository;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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


            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<CarNestDBContext>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // register specific car image repository and manager
            builder.Services.AddScoped<ICarImageRepository, CarImageRepository>();
            builder.Services.AddScoped<ICarImageManager, CarImageManager>();

            builder.Services.AddScoped<IModelManager, ModelManager>();
            builder.Services.AddScoped<IMakeManager, MakeManager>();
            builder.Services.AddScoped<IBodyTypeManager, BodyTypeManager>();
            builder.Services.AddScoped<ILocationManager, LocationManager>();
            builder.Services.AddScoped<IFuelTypeManager, FuelTypeManager>();



            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
