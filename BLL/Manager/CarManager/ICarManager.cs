using DAL.Entity;
using Microsoft.AspNetCore.Http;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;
using System.Collections.Generic;

namespace BLL.Manager.CarManager
{
    public interface ICarManager
    {
        IEnumerable<CarResponse> GetAll();
        IEnumerable<CarResponse> GetCarsByVendor(string vendorId);
        CarDetailResponse? GetById(string id);
        CarResponse Add(CarRequest request, IFormFileCollection? images);
        CarResponse Update(string id, CarRequest request, string userId, string userRole, IFormFileCollection? newImages, List<string>? imagesToDelete);
        void Delete(string id, string userId, string userRole);
    }
}
