using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace DAL.Repository.CarImageRepository
{
    public interface ICarImageRepository : IGenericRepository<CarImage>
    {
        List<CarImage> GetByCarId(string carId);
    }
}
