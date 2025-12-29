using Presentation.DTOs.Requests;

namespace Presentation.DTOs.Responses
{
    public class CarDetailResponse : CarResponse
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int BodyTypeId { get; set; }
        public int FuelId { get; set; }
        public int LocId { get; set; }

        public string? PublisherName { get; set; }
        public string? PublisherPhone { get; set; }
        public string? PublisherEmail { get; set; }
        public string? OwnerId { get; set; }
    }
}
