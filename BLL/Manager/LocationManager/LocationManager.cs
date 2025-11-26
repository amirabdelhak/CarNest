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

        public IEnumerable<LocationCityResponse> GetAll()
        {
            var data = UnitOfWork.LocationCityRepo.GetAll();
            return data.Select(l => l.ToResponse());
        }

        public LocationCityResponse? GetById(int id)
        {
            var data = UnitOfWork.LocationCityRepo.GetById(id);
            return data?.ToResponse();
        }

        public LocationCityResponse Add(LocationCityRequest request)
        {
            var entity = request.ToEntity();
            UnitOfWork.LocationCityRepo.Add(entity);
            UnitOfWork.Save();
            return entity.ToResponse();
        }

        public LocationCityResponse Update(LocationCity entity)
        {
            UnitOfWork.LocationCityRepo.Update(entity);
            UnitOfWork.Save();
            return entity.ToResponse();
        }

        public void Delete(int id)
        {
            var loc = UnitOfWork.LocationCityRepo.GetById(id);
            if (loc != null)
            {
                UnitOfWork.LocationCityRepo.Delete(loc);
                UnitOfWork.Save();
            }
        }
    }
}
