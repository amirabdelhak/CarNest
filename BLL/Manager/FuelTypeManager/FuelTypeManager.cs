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

        public async Task<IEnumerable<FuelTypeResponse>> GetAllAsync()
        {
            var data = await UnitOfWork.FuelTypeRepo.GetAllAsync();
            return data.Select(f => f.ToResponse());
        }

        public async Task<FuelTypeResponse?> GetByIdAsync(int id)
        {
            var data = await UnitOfWork.FuelTypeRepo.GetByIdAsync(id);
            return data?.ToResponse();
        }

        public async Task<FuelTypeResponse> AddAsync(FuelTypeRequest request)
        {
            var entity = request.ToEntity();
            UnitOfWork.FuelTypeRepo.Add(entity);
            await UnitOfWork.SaveAsync();
            return entity.ToResponse();
        }

        public async Task<FuelTypeResponse> UpdateAsync(FuelType entity)
        {
            UnitOfWork.FuelTypeRepo.Update(entity);
            await UnitOfWork.SaveAsync();
            return entity.ToResponse();
        }

        public async Task DeleteAsync(int id)
        {
            var obj = await UnitOfWork.FuelTypeRepo.GetByIdAsync(id);
            if (obj != null)
            {
                UnitOfWork.FuelTypeRepo.Delete(obj);
                await UnitOfWork.SaveAsync();
            }
        }
    }
}
