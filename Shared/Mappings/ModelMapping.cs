using DAL.Entity;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;

namespace Presentation.Mappings
{
    public static class ModelMapping
    {
        public static ModelResponse ToResponse(this Model e) =>
            new ModelResponse
            {
                ModelId = e.ModelId,
                ModelName = e.ModelName,
                MakeId = e.MakeId
            };

        public static Model ToEntity(this ModelRequest r) =>
            new Model
            {
                ModelName = r.ModelName,
                MakeId = r.MakeId
            };
    }
}
