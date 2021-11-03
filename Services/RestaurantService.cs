using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_bff.Helpers;
using user_bff.Models;

namespace user_bff.Services
{
    public interface IRestaurantService
    {
        /// <summary>
        /// Use: Get restaurant list
        /// </summary>
        /// <returns>Object</returns>
        IEnumerable<Restaurant> GetAll();

        /// <summary>
        /// Use: Get restaurant search result
        /// </summary>
        /// <param name="SearchText">Search Text</param>
        /// <returns>Object</returns>
        IEnumerable<Restaurant> GetSearch(string SearchText);

        /// <summary>
        /// Use: Get restaurant by Id
        /// </summary>
        /// <param name="restaurantCode">restaurant code</param>
        /// <returns>Object</returns>
        Restaurant GetById(Guid restaurantCode);
    }

    public class RestaurantService : IRestaurantService
    {
        private DBContext _context;
        public RestaurantService(DBContext context)
        {
            _context = context;
        }

        ///<inheritdoc/>
        public IEnumerable<Restaurant> GetAll()
        {
            try
            {
                List<Restaurant> restro = _context.Restaurant
                    .Include(x => x.RestaurantOpenDays)
                    .ToList();

                if (restro.Count == 0)
                {
                    throw new AppException("No restaurant found.");
                }

                return restro;
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }

        ///<inheritdoc/>
        public IEnumerable<Restaurant> GetSearch(string SearchText)
        {
            try
            {
                List<Restaurant> restro = _context.Restaurant
                    .Include(x => x.RestaurantOpenDays)
                    .Where(x => x.Name.Contains(SearchText) || x.Description.Contains(SearchText) || x.Address.Contains(SearchText) || x.Category.Contains(SearchText))
                    .ToList();

                if (restro.Count == 0)
                {
                    throw new AppException("No restaurant found.");
                }

                return restro;
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }

        ///<inheritdoc/>
        public Restaurant GetById(Guid restaurantID)
        {
            try {
                Restaurant restro = _context.Restaurant
                    .Include(x => x.RestaurantOpenDays)
                    .Where(x => x.RestaurantId == restaurantID)
                    .FirstOrDefault();

                if (restro == null) {
                    throw new AppException("Restaurant is not valid.");
                }

                return restro;
            }
            catch (Exception ex) {
                throw new AppException(ex.Message);
            }
        }

    }
}
