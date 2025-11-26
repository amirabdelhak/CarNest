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

        public IEnumerable<BodyTypeResponse> GetAll()
        {
            var data = UnitOfWork.BodyTypeRepo.GetAll();
            return data.Select(b => b.ToResponse());
            // لو عايزين تجيب العربيات:
            // return unit.BodyTypeRepo.GetAll(q => q.Include(b => b.Cars));
        }

        public BodyTypeResponse? GetById(int id)
        {
            var data= UnitOfWork.BodyTypeRepo.GetById(id);
            return data.ToResponse();
        }

        public BodyTypeResponse Add(BodyTypeRequest request)
        {
            var data = request.ToEntity();
            UnitOfWork.BodyTypeRepo.Add(data);
            UnitOfWork.Save();
            return data.ToResponse();
        }

        public BodyTypeResponse Update(BodyType bodyType)
        {
            UnitOfWork.BodyTypeRepo.Update(bodyType);
            UnitOfWork.Save();
            return bodyType.ToResponse();
        }

        public void Delete(int id)
        {
            var bt = UnitOfWork.BodyTypeRepo.GetById(id);
            if (bt != null)
            {
                UnitOfWork.BodyTypeRepo.Delete(bt);
                UnitOfWork.Save();
            }
        }
    }
}
