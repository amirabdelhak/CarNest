using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using DAL.UnitOfWork;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;
using Presentation.Mappings;

namespace BLL.Manager.ModelManager
{
    public class ModelManager : IModelManager
    {
        private readonly IUnitOfWork UnitOfWork;

        public ModelManager(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }

        public async Task<IEnumerable<ModelResponse>> GetAllAsync()
        {
            var data = await UnitOfWork.ModelRepo.GetAllAsync();
            return data.Select(f => f.ToResponse());
        }

        public async Task<ModelResponse?> GetByIdAsync(int id)
        {
            var data = await UnitOfWork.ModelRepo.GetByIdAsync(id);
            return data?.ToResponse();
        }

        public async Task<IEnumerable<ModelResponse>> GetByMakeIdAsync(int makeId)
        {
            var data = await UnitOfWork.ModelRepo.GetAllAsync(q => q.Where(m => m.MakeId == makeId));
            return data.Select(f => f.ToResponse());
        }

        public async Task<ModelResponse> AddAsync(ModelRequest request)
        {
            var entity = request.ToEntity();
            UnitOfWork.ModelRepo.Add(entity);
            await UnitOfWork.SaveAsync();
            return entity.ToResponse();
        }

        public async Task<ModelResponse> UpdateAsync(Model entity)
        {
            UnitOfWork.ModelRepo.Update(entity);
            await UnitOfWork.SaveAsync();
            return entity.ToResponse();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await UnitOfWork.ModelRepo.GetByIdAsync(id);
            if (item != null)
            {
                UnitOfWork.ModelRepo.Delete(item);
                await UnitOfWork.SaveAsync();
            }
        }
    }
}
