using DAL.Entity;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;


namespace Presentation.Mappings
{
    public static class DtoMappings
    {
        // BodyType
        public static BodyTypeResponse ToResponse(this BodyType e) =>
            new BodyTypeResponse
            {
                BodyId = e.BodyId,
                Name = e.Name
            };

        public static BodyType ToEntity(this BodyTypeRequest r) =>
            new BodyType
            {
                Name = r.Name
            };

        // Model
        public static ModelResponse ToResponse(this Model e) =>
            new ModelResponse
            {
                ModelId = e.ModelId,
                ModelName = e.ModelName
            };

        public static Model ToEntity(this ModelRequest r) =>
            new Model
            {
                ModelName = r.ModelName
            };

        // Make
        public static MakeResponse ToResponse(this Make e) =>
            new MakeResponse
            {
                MakeId = e.MakeId,
                MakeName = e.MakeName
            };

        public static Make ToEntity(this MakeRequest r) =>
            new Make
            {
                MakeName = r.MakeName
            };

        // FuelType
        public static FuelTypeResponse ToResponse(this FuelType e) =>
            new FuelTypeResponse
            {
                FuelId = e.FuelId,
                Name = e.Name
            };

        public static FuelType ToEntity(this FuelTypeRequest r) =>
            new FuelType
            {
                Name = r.Name
            };

        // Location
        public static LocationCityResponse ToResponse(this LocationCity e) =>
            new LocationCityResponse
            {
                LocId = e.LocId,
                Name = e.Name
            };

        public static LocationCity ToEntity(this LocationCityRequest r) =>
            new LocationCity
            {
                Name = r.Name
            };


        //    // Car (list/detail)
        //    public static CarListResponse ToListResponse(this Car e) =>
        //        new CarListResponse
        //        {
        //            CarId = e.CarId,
        //            Year = e.Year,
        //            Price = e.Price,
        //            Description = e.Description,
        //            MakeId = e.MakeId,
        //            ModelId = e.ModelId,
        //            BodyTypeId = e.BodyTypeId,
        //            FuelId = e.FuelId,
        //            LocId = e.LocId
        //        };

        //    public static CarDetailResponse ToDetailResponse(this Car e) =>
        //        new CarDetailResponse
        //        {
        //            CarId = e.CarId,
        //            Year = e.Year,
        //            Price = e.Price,
        //            Description = e.Description,
        //            MakeId = e.MakeId,
        //            MakeName = e.Make?.MakeName,
        //            ModelId = e.ModelId,
        //            ModelName = e.Model?.ModelName,
        //            BodyTypeId = e.BodyTypeId,
        //            BodyTypeName = e.BodyType?.Name,
        //            FuelId = e.FuelId,
        //            FuelName = e.FuelType?.Name,
        //            LocId = e.LocId,
        //            LocName = e.LocationCity?.Name,
        //            AdminId = e.AdminId,
        //            BuyerId = e.BuyerId,
        //            CreatedDate = e.CreatedDate
        //        };

        //    public static Car ToEntity(this CarCreateRequest r) =>
        //        new Car
        //        {
        //            CarId = r.CarId,
        //            Year = r.Year,
        //            Price = r.Price,
        //            Description = r.Description,
        //            MakeId = r.MakeId,
        //            ModelId = r.ModelId,
        //            BodyTypeId = r.BodyTypeId,
        //            FuelId = r.FuelId,
        //            LocId = r.LocId,
        //            AdminId = r.AdminId,
        //            BuyerId = r.BuyerId,
        //            CreatedDate = System.DateTime.UtcNow
        //        };

        //    public static void UpdateFrom(this Car e, CarUpdateRequest r)
        //    {
        //        e.Year = r.Year;
        //        e.Price = r.Price;
        //        e.Description = r.Description;
        //        e.MakeId = r.MakeId;
        //        e.ModelId = r.ModelId;
        //        e.BodyTypeId = r.BodyTypeId;
        //        e.FuelId = r.FuelId;
        //        e.LocId = r.LocId;
        //    }
    }
}