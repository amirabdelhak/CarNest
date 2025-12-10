namespace Presentation.DTOs.Responses
{
    public class FavoriteResponse
    {
        public string CarId { get; set; }
        public DateTime SavedAt { get; set; }
        public CarDetailResponse? Car { get; set; }
    }
}
