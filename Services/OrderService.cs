using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using user_bff.Models;
using user_bff.Helpers;

namespace user_bff.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAll(Guid UserId);
        Order GetById(long id);
        Order Create(Order order);
        Order Update(long id, Order Order);
        OrderStatus GetStatusById(long id);
    }

    public class OrderService : IOrderService
    {
        private DBContext _context;

        public OrderService(DBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Use: Get list of order 
        /// </summary>
        /// <returns>Object</returns>
        public IEnumerable<Order> GetAll(Guid UserId)
        {
            // return all user except deleted
            List<Order> _order = _context.Orders
                .Include(x => x.RestaurantInfo)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.OrderItemOptions)
                .Where(x => x.CognitoUserId == UserId)
                .OrderByDescending(x => x.CreatedDate)
                .Where(m => m.RestaurantInfo.RestaurantId == m.RestaurantId)
                .ToList();

            if (_order.Count == 0)
            {
                throw new AppException("No order found.");
            }

            return _order;
        }

        /// <summary>
        /// Use: Get order information by order id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Object</returns>
        public Order GetById(long id)
        {
            // return user info based on ID
            Order _order = _context.Orders
                .Include(x => x.RestaurantInfo)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.OrderItemOptions)
                .AsNoTracking()
                .Where(m => m.OrderId == id && m.RestaurantInfo.RestaurantId == m.RestaurantId)
                .FirstOrDefault();

            if (_order == null) 
            {
                throw new AppException("Order not found, invalid order id.");
            }
            else if (_order.PickupDate == null)
            {
                System.TimeSpan duration = new System.TimeSpan(0, 1, 0, 0);

                _order.PickupDate = Convert.ToDateTime(_order.CreatedDate).Add(duration); 
            }

            return _order;
        }

        /// <summary>
        /// method using by API endpoint: /api/order/create
        /// Use: create new pending order 
        /// Input: order information
        /// </summary>
        /// <returns>Object</returns>
        public Order Create(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();

            return GetById(order.OrderId);
        }

        /// <summary>
        /// method using by API endpoint: /api/order/update/{id}
        /// Use: update order information or status
        /// Input: order information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderParam"></param>
        /// <returns>Object</returns>
        public Order Update(long id, Order orderParam)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
                throw new AppException("Order not found.");

            _context.Entry(order).State = EntityState.Detached;

            orderParam.Status = order.Status;

            if (orderParam.PickupDate != null) 
            {
                orderParam.PickupDate = order.PickupDate;
            }

            // update Order information
            _context.Orders.Update(order);
            _context.SaveChanges();

            //_context.Dispose();
            return GetById(order.OrderId);
        }

        /// <summary>
        /// Use: Get order status by order id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Object</returns>
        public OrderStatus GetStatusById(long id)
        {
            // return user info based on ID
            Order _order = _context.Orders
                .AsNoTracking()
                .Where(m => m.OrderId == id)
                .FirstOrDefault();

            if (_order == null)
            {
                throw new AppException("Order not found, invalid order id.");
            }
            else if (_order.PickupDate == null)
            {
                System.TimeSpan duration = new System.TimeSpan(0, 1, 0, 0);

                _order.PickupDate = Convert.ToDateTime(_order.CreatedDate).Add(duration);
            }

            OrderStatus _status = new OrderStatus();
            _status.OrderId = _order.OrderId;
            _status.PickupName = _order.PickupName;
            _status.PickupDate = _order.PickupDate;
            _status.Status = _order.Status;
            _status.Paid = _order.Paid;


            return _status;
        }

    }
}
