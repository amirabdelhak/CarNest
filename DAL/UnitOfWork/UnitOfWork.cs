using DAL.Context;
using DAL.Entity;
using DAL.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider serviceProvider;
        private readonly CarNestDBContext dbcontext;

        private IGenericRepository<Admin> adminRepo;
        private IGenericRepository<Vendor> buyerRepo;
        private IGenericRepository<Customer> customerRepo;
        private IGenericRepository<Car> carRepo;
        private IGenericRepository<BodyType> bodyTypeRepo;
        private IGenericRepository<FuelType> fuelTypeRepo;
        private IGenericRepository<LocationCity> locationCityRepo;
        private IGenericRepository<Make> makeRepo;
        private IGenericRepository<Model> modelRepo;
        private IGenericRepository<Favorite> favoriteRepo;

        public UnitOfWork(CarNestDBContext dbcontext, IServiceProvider serviceProvider)
        {
            this.dbcontext = dbcontext;
            this.serviceProvider = serviceProvider;
        }

        public IGenericRepository<Admin> AdminRepo
            => adminRepo ??= serviceProvider.GetRequiredService<IGenericRepository<Admin>>();

        public IGenericRepository<Vendor> VendorRepo
            => buyerRepo ??= serviceProvider.GetRequiredService<IGenericRepository<Vendor>>();

        public IGenericRepository<Customer> CustomerRepo
            => customerRepo ??= serviceProvider.GetRequiredService<IGenericRepository<Customer>>();

        public IGenericRepository<Car> CarRepo
            => carRepo ??= serviceProvider.GetRequiredService<IGenericRepository<Car>>();

        public IGenericRepository<BodyType> BodyTypeRepo
            => bodyTypeRepo ??= serviceProvider.GetRequiredService<IGenericRepository<BodyType>>();

        public IGenericRepository<FuelType> FuelTypeRepo
            => fuelTypeRepo ??= serviceProvider.GetRequiredService<IGenericRepository<FuelType>>();

        public IGenericRepository<LocationCity> LocationCityRepo
            => locationCityRepo ??= serviceProvider.GetRequiredService<IGenericRepository<LocationCity>>();

        public IGenericRepository<Make> MakeRepo
            => makeRepo ??= serviceProvider.GetRequiredService<IGenericRepository<Make>>();

        public IGenericRepository<Model> ModelRepo
            => modelRepo ??= serviceProvider.GetRequiredService<IGenericRepository<Model>>();

        public IGenericRepository<Favorite> FavoriteRepo
            => favoriteRepo ??= serviceProvider.GetRequiredService<IGenericRepository<Favorite>>();

        public void Save()
        {
            dbcontext.SaveChanges();
        }
    }
}
