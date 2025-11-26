using DAL.Entity;
using DAL.Repository;
using DAL.Repository.CarImageRepository;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<Admin> AdminRepo { get; }
        IGenericRepository<Vendor> BuyerRepo { get; }
        IGenericRepository<Customer> CustomerRepo { get; }
        IGenericRepository<Car> CarRepo { get; }
        IGenericRepository<BodyType> BodyTypeRepo { get; }
        IGenericRepository<FuelType> FuelTypeRepo { get; }
        IGenericRepository<LocationCity> LocationCityRepo { get; }
        IGenericRepository<Make> MakeRepo { get; }
        IGenericRepository<Model> ModelRepo { get; }
        IGenericRepository<Favorite> FavoriteRepo { get; }
        IGenericRepository<CarImage> CarImageRepo { get; }

        void Save();
    }
}
