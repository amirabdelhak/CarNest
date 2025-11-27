using DAL.Entity;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;

namespace Presentation.Mappings
{
    public static class MakeMapping
    {
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
    }
}
