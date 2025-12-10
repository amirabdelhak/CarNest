using DAL.Entity;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Manager.ModelManager
{
    public interface IModelManager
    {
        IEnumerable<ModelResponse> GetAll();
        ModelResponse? GetById(int id);
        ModelResponse Add(ModelRequest request);
        ModelResponse Update(Model model);
        void Delete(int id);
    }
}
