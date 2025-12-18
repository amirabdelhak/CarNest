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
        Task<IEnumerable<MakeResponse>> GetAllAsync();
        Task<MakeResponse?> GetByIdAsync(int id);
        Task<MakeResponse> AddAsync(MakeRequest request);
        Task<MakeResponse> UpdateAsync(Make make);
        Task DeleteAsync(int id);
    }
}
