using System.Collections.Generic;
using DAL.Entity;

namespace BLL.Manager.CarImageManager
{
    public interface ICarImageManager
    {
        IEnumerable<CarImage> GetByCarId(string carId);
        CarImage? GetById(int imageId);
        CarImage Add(string carId, string imageUrl);
        void Delete(int imageId);
    }
}
