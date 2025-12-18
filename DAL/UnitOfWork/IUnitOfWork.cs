using DAL.Entity;
using DAL.Repository;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<Admin> AdminRepo { get; }
        IGenericRepository<Vendor> VendorRepo { get; }
        IGenericRepository<Customer> CustomerRepo { get; }
        IGenericRepository<Car> CarRepo { get; }
        IGenericRepository<BodyType> BodyTypeRepo { get; }
        IGenericRepository<FuelType> FuelTypeRepo { get; }
        IGenericRepository<LocationCity> LocationCityRepo { get; }
        IGenericRepository<Make> MakeRepo { get; }
        IGenericRepository<Model> ModelRepo { get; }
        IGenericRepository<Favorite> FavoriteRepo { get; }


        Task SaveAsync();
    }
}
