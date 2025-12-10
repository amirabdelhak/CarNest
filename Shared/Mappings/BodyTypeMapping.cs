using DAL.Entity;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;

namespace Presentation.Mappings
{
    public static class BodyTypeMapping
    {
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
    }
}
