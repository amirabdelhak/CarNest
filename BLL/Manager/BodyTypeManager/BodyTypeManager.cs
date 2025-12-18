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

namespace BLL.Manager.BodyTypeManager
{
    public class BodyTypeManager : IBodyTypeManager
    {
        private readonly IUnitOfWork UnitOfWork;

        public BodyTypeManager(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }

        public async Task<IEnumerable<BodyTypeResponse>> GetAllAsync()
        {
            var data = await UnitOfWork.BodyTypeRepo.GetAllAsync();
            return data.Select(b => b.ToResponse());
            // لو عايزين تجيب العربيات:
            // return unit.BodyTypeRepo.GetAll(q => q.Include(b => b.Cars));
        }

        public async Task<BodyTypeResponse?> GetByIdAsync(int id)
        {
            var data = await UnitOfWork.BodyTypeRepo.GetByIdAsync(id);
            return data?.ToResponse();
        }

        public async Task<BodyTypeResponse> AddAsync(BodyTypeRequest request)
        {
            var data = request.ToEntity();
            UnitOfWork.BodyTypeRepo.Add(data);
            await UnitOfWork.SaveAsync();
            return data.ToResponse();
        }

        public async Task<BodyTypeResponse> UpdateAsync(BodyType bodyType)
        {
            UnitOfWork.BodyTypeRepo.Update(bodyType);
            await UnitOfWork.SaveAsync();
            return bodyType.ToResponse();
        }

        public async Task DeleteAsync(int id)
        {
            var bt = await UnitOfWork.BodyTypeRepo.GetByIdAsync(id);
            if (bt != null)
            {
                UnitOfWork.BodyTypeRepo.Delete(bt);
                await UnitOfWork.SaveAsync();
            }
        }
    }
}
