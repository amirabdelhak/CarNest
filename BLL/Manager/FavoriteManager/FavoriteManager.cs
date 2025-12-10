using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entity;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs.Responses;
using Presentation.Mappings;

namespace BLL.Manager.FavoriteManager
{
    public class FavoriteManager : IFavoriteManager
    {
        private readonly IUnitOfWork UnitOfWork;

        public FavoriteManager(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }

        public FavoriteResponse AddToFavorites(string customerId, string carId)
        {
            // Check if already exists
            var existing = UnitOfWork.FavoriteRepo.GetAll(query =>
                query.Where(f => f.CustomerId == customerId && f.CarId == carId)
            ).FirstOrDefault();

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
            UnitOfWork.Save();

            // Get the car details with Model and Make through Model
            var car = UnitOfWork.CarRepo.GetAll(query =>
                query.Where(c => c.CarId == carId)
                     .Include(c => c.Model).ThenInclude(m => m.Make)
                     .Include(c => c.BodyType)
                     .Include(c => c.FuelType)
                     .Include(c => c.LocationCity)
                     .Include(c => c.Admin)
                     .Include(c => c.Vendor)
            ).FirstOrDefault();

            return new FavoriteResponse
            {
                CarId = favorite.CarId,
                SavedAt = favorite.SavedAt,
                Car = car?.ToDetailResponse()
            };
        }

        public void RemoveFromFavorites(string customerId, string carId)
        {
            var favorite = UnitOfWork.FavoriteRepo.GetById(customerId, carId);
            if (favorite != null)
            {
                UnitOfWork.FavoriteRepo.Delete(favorite);
                UnitOfWork.Save();
            }
        }

        public IEnumerable<FavoriteResponse> GetCustomerFavorites(string customerId)
        {
            var favorites = UnitOfWork.FavoriteRepo.GetAll(query =>
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
