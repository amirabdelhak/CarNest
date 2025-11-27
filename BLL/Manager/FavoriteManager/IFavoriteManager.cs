using Presentation.DTOs.Responses;
using System.Collections.Generic;

namespace BLL.Manager.FavoriteManager
{
    public interface IFavoriteManager
    {
        FavoriteResponse AddToFavorites(string customerId, string carId);
        void RemoveFromFavorites(string customerId, string carId);
        IEnumerable<FavoriteResponse> GetCustomerFavorites(string customerId);
    }
}
