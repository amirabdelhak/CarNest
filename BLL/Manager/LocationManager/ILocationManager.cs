using DAL.Entity;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Manager.LocationManager
{
    public interface ILocationManager
    {
        Task<IEnumerable<LocationCityResponse>> GetAllAsync();
        Task<LocationCityResponse?> GetByIdAsync(int id);
        Task<LocationCityResponse> AddAsync(LocationCityRequest request);
        Task<LocationCityResponse> UpdateAsync(LocationCity location);
        Task DeleteAsync(int id);
    }
}
