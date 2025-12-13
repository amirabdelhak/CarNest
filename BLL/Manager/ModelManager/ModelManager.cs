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

        public IEnumerable<ModelResponse> GetAll()
        {
            var data = UnitOfWork.ModelRepo.GetAll();
            return data.Select(f => f.ToResponse());
        }

        public ModelResponse? GetById(int id)
        {
            var data = UnitOfWork.ModelRepo.GetById(id);
            return data?.ToResponse();
        }

        public IEnumerable<ModelResponse>? GetByMakeId(int makeId)
        {
            var data = UnitOfWork.ModelRepo.GetAll()
                .Where(m => m.MakeId == makeId);
            return data.Select(f => f.ToResponse());
        }

        public ModelResponse Add(ModelRequest request)
        {
            var entity = request.ToEntity();
            UnitOfWork.ModelRepo.Add(entity);
            UnitOfWork.Save();
            return entity.ToResponse();
        }

        public ModelResponse Update(Model entity)
        {
            UnitOfWork.ModelRepo.Update(entity);
            UnitOfWork.Save();
            return entity.ToResponse();
        }

        public void Delete(int id)
        {
            var item = UnitOfWork.ModelRepo.GetById(id);
            if (item != null)
            {
                UnitOfWork.ModelRepo.Delete(item);
                UnitOfWork.Save();
            }
        }
    }
}
