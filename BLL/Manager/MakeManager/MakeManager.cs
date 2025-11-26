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

        public IEnumerable<MakeResponse> GetAll()
        {
            var data = UnitOfWork.MakeRepo.GetAll();
            return data.Select(l => l.ToResponse());
        }

        public MakeResponse? GetById(int id)
        {
            var data = UnitOfWork.MakeRepo.GetById(id);
            return data?.ToResponse();
        }

        public MakeResponse Add(MakeRequest request)
        {
            var entity = request.ToEntity();
            UnitOfWork.MakeRepo.Add(entity);
            UnitOfWork.Save();
            return entity.ToResponse();
        }

        public MakeResponse Update(Make make)
        {
            UnitOfWork.MakeRepo.Update(make);
            UnitOfWork.Save();
            return make.ToResponse();
        }

        public void Delete(int id)
        {
            var item = UnitOfWork.MakeRepo.GetById(id);
            if (item != null)
            {
                UnitOfWork.MakeRepo.Delete(item);
                UnitOfWork.Save();
            }
        }
    }
}
