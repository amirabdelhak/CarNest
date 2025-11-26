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
        IEnumerable<FuelTypeResponse> GetAll();
        FuelTypeResponse? GetById(int id);
        FuelTypeResponse Add(FuelTypeRequest request);
        FuelTypeResponse Update(FuelType fuel);
        void Delete(int id);
    }
}
