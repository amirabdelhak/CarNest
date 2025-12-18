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
        Task<IEnumerable<BodyTypeResponse>> GetAllAsync();
        Task<BodyTypeResponse?> GetByIdAsync(int id);
        Task<BodyTypeResponse> AddAsync(BodyTypeRequest request);
        Task<BodyTypeResponse> UpdateAsync(BodyType bodyType);
        Task DeleteAsync(int id);
    }
}
