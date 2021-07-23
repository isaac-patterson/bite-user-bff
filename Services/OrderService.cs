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
        IEnumerable<Order> GetAll();
        Order GetById(long id);
        Order Create(Order order);
        Order Update(int id, Order Order);
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
        public IEnumerable<Order> GetAll()
        {
            // return all user except deleted
            return _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.OrderItemOptions)
                //.FirstOrDefault(x => x.OrderId == id);
                .OrderByDescending(x => x.CreatedDate);
        }

        /// <summary>
        /// Use: Get order information by order id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Object</returns>
        public Order GetById(long id)
        {
            // return user info based on ID
            return _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.OrderItemOptions)
                .AsNoTracking()
                .Where(m => m.OrderId == id)
                .FirstOrDefault();
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

            return order;
        }

        /// <summary>
        /// method using by API endpoint: /api/order/update/{id}
        /// Use: update order information or status
        /// Input: order information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderParam"></param>
        /// <returns>Object</returns>
        public Order Update(int id, Order orderParam)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
                throw new AppException("Order not found.");

            _context.Entry(order).State = EntityState.Detached;

            orderParam.Status = order.Status;

            // update Order information
            _context.Orders.Update(order);
            _context.SaveChanges();

            _context.Dispose();
            return order;
        }

   }
}
