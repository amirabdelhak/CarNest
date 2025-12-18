using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using DAL.Entity;
using DAL.UnitOfWork;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;
using Presentation.Mappings;

namespace BLL.Manager.MakeManager
{
    public class MakeManager : IMakeManager
    {
        private readonly IUnitOfWork UnitOfWork;

        public MakeManager(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }

        public async Task<IEnumerable<MakeResponse>> GetAllAsync()
        {
            var data = await UnitOfWork.MakeRepo.GetAllAsync();
            return data.Select(l => l.ToResponse());
        }

        public async Task<MakeResponse?> GetByIdAsync(int id)
        {
            var data = await UnitOfWork.MakeRepo.GetByIdAsync(id);
            return data?.ToResponse();
        }

        public async Task<MakeResponse> AddAsync(MakeRequest request)
        {
            var entity = request.ToEntity();
            UnitOfWork.MakeRepo.Add(entity);
            await UnitOfWork.SaveAsync();
            return entity.ToResponse();
        }

        public async Task<MakeResponse> UpdateAsync(Make make)
        {
            UnitOfWork.MakeRepo.Update(make);
            await UnitOfWork.SaveAsync();
            return make.ToResponse();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await UnitOfWork.MakeRepo.GetByIdAsync(id);
            if (item != null)
            {
                UnitOfWork.MakeRepo.Delete(item);
                await UnitOfWork.SaveAsync();
            }
        }
    }
}
