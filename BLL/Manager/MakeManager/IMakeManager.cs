using DAL.Entity;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Manager.MakeManager
{
    public interface IMakeManager
    {
        IEnumerable<MakeResponse> GetAll();
        MakeResponse? GetById(int id);
        MakeResponse Add(MakeRequest request);
        MakeResponse Update(Make make);
        void Delete(int id);
    }
}
