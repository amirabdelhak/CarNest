using Azure.Core;
using DAL.Entity;
using DAL.UnitOfWork;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;
using Presentation.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Manager.LocationManager
{
    public class LocationManager : ILocationManager
    {
        private readonly IUnitOfWork UnitOfWork;

        public LocationManager(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }

        public async Task<IEnumerable<LocationCityResponse>> GetAllAsync()
        {
            var data = await UnitOfWork.LocationCityRepo.GetAllAsync();
            return data.Select(l => l.ToResponse());
        }

        public async Task<LocationCityResponse?> GetByIdAsync(int id)
        {
            var data = await UnitOfWork.LocationCityRepo.GetByIdAsync(id);
            return data?.ToResponse();
        }

        public async Task<LocationCityResponse> AddAsync(LocationCityRequest request)
        {
            var entity = request.ToEntity();
            UnitOfWork.LocationCityRepo.Add(entity);
            await UnitOfWork.SaveAsync();
            return entity.ToResponse();
        }

        public async Task<LocationCityResponse> UpdateAsync(LocationCity entity)
        {
            UnitOfWork.LocationCityRepo.Update(entity);
            await UnitOfWork.SaveAsync();
            return entity.ToResponse();
        }

        public async Task DeleteAsync(int id)
        {
            var loc = await UnitOfWork.LocationCityRepo.GetByIdAsync(id);
            if (loc != null)
            {
                UnitOfWork.LocationCityRepo.Delete(loc);
                await UnitOfWork.SaveAsync();
            }
        }
    }
}
