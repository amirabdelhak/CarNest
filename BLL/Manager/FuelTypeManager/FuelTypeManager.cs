using DAL.Entity;
using DAL.UnitOfWork;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;
using Presentation.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Manager.FuelTypeManager
{
    public class FuelTypeManager : IFuelTypeManager
    {
        private readonly IUnitOfWork UnitOfWork;

        public FuelTypeManager(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }

        public IEnumerable<FuelTypeResponse> GetAll()
        {
            var data = UnitOfWork.FuelTypeRepo.GetAll();
            return data.Select(f => f.ToResponse());
        }

        public FuelTypeResponse? GetById(int id)
        {
            var data = UnitOfWork.FuelTypeRepo.GetById(id);
            return data?.ToResponse();
        }

        public FuelTypeResponse Add(FuelTypeRequest request)
        {
            var entity = request.ToEntity();
            UnitOfWork.FuelTypeRepo.Add(entity);
            UnitOfWork.Save();
            return entity.ToResponse();
        }

        public FuelTypeResponse Update(FuelType entity)
        {
            UnitOfWork.FuelTypeRepo.Update(entity);
            UnitOfWork.Save();
            return entity.ToResponse();
        }

        public void Delete(int id)
        {
            var obj = UnitOfWork.FuelTypeRepo.GetById(id);
            if (obj != null)
            {
                UnitOfWork.FuelTypeRepo.Delete(obj);
                UnitOfWork.Save();
            }
        }
    }
}
