﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_bff.Helpers;
using user_bff.Models;

namespace user_bff.Services
{
    public interface ICouponService
    {
        /// <summary>
        /// Use: Get coupon by Id
        /// </summary>
        /// <param name="couponCode">coupon code</param>
        /// <returns>Object</returns>
        Coupon GetById(string couponCode);

        /// <summary>
        /// Use: Check coupon exists
        /// </summary>
        /// <param name="couponCode"></param>
        /// <returns></returns>
        bool IsCouponExists(string couponCode);
    }

    public class CouponService : ICouponService
    {
        private DBContext _context;
        public CouponService(DBContext context)
        {
            _context = context;
        }

        ///<inheritdoc/>
        public Coupon GetById(string couponCode)
        {
            if (_context.Coupon.Any(x => x.DiscountCode == couponCode && x.ExpiryDate > DateTime.UtcNow))
            {
                throw new AppException("Coupon code is exipred.");
            }
            return _context.Coupon.FirstOrDefault(x => x.DiscountCode == couponCode);
        }

        ///<inheritdoc/>
        public bool IsCouponExists(string couponCode)
        {
            return _context.Coupon.Any(x => x.DiscountCode == couponCode);
        }
    }
}
