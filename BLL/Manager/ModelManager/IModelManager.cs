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
        Task<IEnumerable<ModelResponse>> GetAllAsync();
        Task<ModelResponse?> GetByIdAsync(int id);
        Task<IEnumerable<ModelResponse>> GetByMakeIdAsync(int MakeId);
        Task<ModelResponse> AddAsync(ModelRequest request);
        Task<ModelResponse> UpdateAsync(Model model);
        Task DeleteAsync(int id);
    }
}
