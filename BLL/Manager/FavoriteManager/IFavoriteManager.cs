using Presentation.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Manager.FavoriteManager
{
    public interface IFavoriteManager
    {
        Task<FavoriteResponse> AddToFavoritesAsync(string customerId, string carId);
        Task RemoveFromFavoritesAsync(string customerId, string carId);
        Task<IEnumerable<FavoriteResponse>> GetCustomerFavoritesAsync(string customerId);
    }
}
