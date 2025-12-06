using DAL.Entity;
using Microsoft.AspNetCore.Http;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;
using System.Collections.Generic;

namespace BLL.Manager.CarManager
{
    public interface ICarManager
    {
        PagedResponse<CarResponse> GetAll(PaginationRequest request);
        PagedResponse<CarResponse> GetCarsByVendor(PaginationRequest request, string vendorId);
        CarDetailResponse? GetById(string id);
        CarResponse Add(CarRequest request, string? adminId, string? vendorId, IFormFileCollection? images);
        CarResponse Update(string id, CarRequest request, string userId, string userRole, IFormFileCollection? newImages, List<string>? imagesToDelete);
        void Delete(string id, string userId, string userRole);
    }
}
