using DAL.Entity;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Manager.BodyTypeManager
{
    public interface IBodyTypeManager
    {
        IEnumerable<BodyTypeResponse> GetAll();
        BodyTypeResponse? GetById(int id);
        BodyTypeResponse Add(BodyTypeRequest request);
        BodyTypeResponse Update(BodyType bodyType);
        void Delete(int id);
    }
}
