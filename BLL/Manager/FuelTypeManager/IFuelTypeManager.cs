using DAL.Entity;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Manager.FuelTypeManager
{
    public interface IFuelTypeManager
    {
        Task<IEnumerable<FuelTypeResponse>> GetAllAsync();
        Task<FuelTypeResponse?> GetByIdAsync(int id);
        Task<FuelTypeResponse> AddAsync(FuelTypeRequest request);
        Task<FuelTypeResponse> UpdateAsync(FuelType fuel);
        Task DeleteAsync(int id);
    }
}
