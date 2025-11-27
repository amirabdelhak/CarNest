using Presentation.DTOs.Requests;

namespace Presentation.DTOs.Responses
{
    public class CarResponse : CarRequest
    {
        public string CarId { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string>? ImageUrls { get; set; }
    }
}
