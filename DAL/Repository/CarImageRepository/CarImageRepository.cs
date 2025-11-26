using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.CarImageRepository
{
    public class CarImageRepository : GenericRepository<CarImage>, ICarImageRepository
    {
        public CarImageRepository(CarNestDBContext dbcontext) : base(dbcontext)
        {

        }

        public List<CarImage> GetByCarId(string carId)
        {
            return dbcontext.Set<CarImage>()
                            .AsNoTracking()
                            .Where(i => i.CarId == carId)
                            .ToList();
        }
    }
}
