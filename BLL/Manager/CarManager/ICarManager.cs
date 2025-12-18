using DAL.Entity;
using Microsoft.AspNetCore.Http;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Manager.CarManager
{
    public interface ICarManager
    {
        Task<PagedResponse<CarResponse>> GetAllAsync(PaginationRequest request);
        Task<PagedResponse<CarResponse>> GetCarsByVendorAsync(PaginationRequest request, string vendorId);
        Task<CarDetailResponse?> GetByIdAsync(string id);
        Task<CarResponse> AddAsync(CarRequest request, string? adminId, string? vendorId, IFormFileCollection? images);
        Task<CarResponse> UpdateAsync(string id, CarRequest request, string userId, string userRole, IFormFileCollection? newImages, List<string>? imagesToDelete);
        Task DeleteAsync(string id, string userId, string userRole);
    }
}
