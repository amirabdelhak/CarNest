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
        IEnumerable<LocationCityResponse> GetAll();
        LocationCityResponse? GetById(int id);
        LocationCityResponse Add(LocationCityRequest request);
        LocationCityResponse Update(LocationCity location);
        void Delete(int id);
    }
}
