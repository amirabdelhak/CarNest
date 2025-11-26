using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using DAL.Repository.CarImageRepository;

using DAL.UnitOfWork;

namespace BLL.Manager.CarImageManager
{
    public class CarImageManager : ICarImageManager
    {
        private readonly IUnitOfWork unitOfWork;

        public CarImageManager(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<CarImage> GetByCarId(string carId)
        {
            return unitOfWork.CarImageRepo.GetAll()
                       .Where(i => i.CarId == carId)
                       .ToList();
        }

        public CarImage? GetById(int imageId)
        {
            return unitOfWork.CarImageRepo.GetById(imageId);
        }

        public CarImage Add(string carId, string imageUrl)
        {
            var entity = new CarImage
            {
                CarId = carId,
                ImageUrl = imageUrl
            };

            unitOfWork.CarImageRepo.Add(entity);
            unitOfWork.Save();
            return entity;
        }

        public void Delete(int imageId)
        {
            var item = unitOfWork.CarImageRepo.GetById(imageId);
            if (item != null)
            {
                unitOfWork.CarImageRepo.Delete(item);
                unitOfWork.Save();
            }
        }
    }
}
