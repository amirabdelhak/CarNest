using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;

namespace BLL.Manager.ModelManager
{
    public interface IModelManager
    {
        IEnumerable<ModelResponse> GetAll();
        ModelResponse? GetById(int id);
        IEnumerable<ModelResponse> GetByMakeId(int MakeId);
        ModelResponse Add(ModelRequest request);
        ModelResponse Update(Model model);
        void Delete(int id);
    }
}
