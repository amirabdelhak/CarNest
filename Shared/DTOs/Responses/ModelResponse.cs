using Presentation.DTOs.Requests;

namespace Presentation.DTOs.Responses
{
    public class ModelResponse : ModelRequest
    {
        public int ModelId { get; set; }
        // MakeId and ModelName are inherited from ModelRequest
    }
}