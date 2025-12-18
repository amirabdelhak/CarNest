using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entity;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs.Responses;
using Presentation.Mappings;

using System.Threading.Tasks;

namespace BLL.Manager.FavoriteManager
{
    public class FavoriteManager : IFavoriteManager
    {
        private readonly IUnitOfWork UnitOfWork;

        public FavoriteManager(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }

        public async Task<FavoriteResponse> AddToFavoritesAsync(string customerId, string carId)
        {
            // Check if already exists
            var existingList = await UnitOfWork.FavoriteRepo.GetAllAsync(query =>
                query.Where(f => f.CustomerId == customerId && f.CarId == carId)
            );
            var existing = existingList.FirstOrDefault();

            if (existing != null)
            {
                throw new Exception("Car is already in favorites");
            }

            var favorite = new Favorite
            {
                CustomerId = customerId,
                CarId = carId,
                SavedAt = DateTime.UtcNow
            };

            UnitOfWork.FavoriteRepo.Add(favorite);
            await UnitOfWork.SaveAsync();

            // Get the car details with Model and Make through Model
            var carList = await UnitOfWork.CarRepo.GetAllAsync(query =>
                query.Where(c => c.CarId == carId)
                     .Include(c => c.Model).ThenInclude(m => m.Make)
                     .Include(c => c.BodyType)
                     .Include(c => c.FuelType)
                     .Include(c => c.LocationCity)
                     .Include(c => c.Admin)
                     .Include(c => c.Vendor)
            );
            var car = carList.FirstOrDefault();

            return new FavoriteResponse
            {
                CarId = favorite.CarId,
                SavedAt = favorite.SavedAt,
                Car = car?.ToDetailResponse()
            };
        }

        public async Task RemoveFromFavoritesAsync(string customerId, string carId)
        {
            var favorite = await UnitOfWork.FavoriteRepo.GetByIdAsync(customerId, carId);
            if (favorite != null)
            {
                UnitOfWork.FavoriteRepo.Delete(favorite);
                await UnitOfWork.SaveAsync();
            }
        }

        public async Task<IEnumerable<FavoriteResponse>> GetCustomerFavoritesAsync(string customerId)
        {
            var favorites = await UnitOfWork.FavoriteRepo.GetAllAsync(query =>
                query.Where(f => f.CustomerId == customerId)
                     .Include(f => f.Car)
                        .ThenInclude(c => c.Model)
                        .ThenInclude(m => m.Make)
                     .Include(f => f.Car)
                        .ThenInclude(c => c.BodyType)
                     .Include(f => f.Car)
                        .ThenInclude(c => c.FuelType)
                     .Include(f => f.Car)
                        .ThenInclude(c => c.LocationCity)
                     .Include(f => f.Car)
                        .ThenInclude(c => c.Admin)
                     .Include(f => f.Car)
                        .ThenInclude(c => c.Vendor)
            );

            return favorites.Select(f => new FavoriteResponse
            {
                CarId = f.CarId,
                SavedAt = f.SavedAt,
                Car = f.Car?.ToDetailResponse()
            });
        }
    }
}
