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
        Task<CarResponse> AddAsync(CarRequest request, string? adminId, string? vendorId, IFormFileCollection? images, IFormFile? licenseImage);
        Task<CarResponse> UpdateAsync(string id, CarRequest request, string userId, string userRole, IFormFileCollection? newImages, List<string>? imagesToDelete, IFormFile? licenseImage);
        Task DeleteAsync(string id, string userId, string userRole);
        Task<PagedResponse<CarResponse>> GetPendingCarsAsync(PaginationRequest request);
        Task<PagedResponse<CarResponse>> GetRejectedCarsAsync(PaginationRequest request);
        Task UpdateStatusAsync(string id, CarStatus status);
    }
}
