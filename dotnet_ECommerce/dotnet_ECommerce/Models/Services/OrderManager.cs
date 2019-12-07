﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_ECommerce.Data;
using dotnet_ECommerce.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace dotnet_ECommerce.Models.Services
{
    public class OrderManager : IOrder
    {
        private readonly StoreDbContext _context;

        public OrderManager(StoreDbContext context)
        {
            _context = context;
        }

        public async Task SaveOrderAsync(Order order)
        {
            await _context.Order.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetLatestOrderForUserAsync(string userId)
        {
            var orders = await GetOrdersByUserIdAsync(userId);
            return orders.OrderByDescending(order => order.ID).FirstOrDefault();
        }

        public async Task SaveOrderItemsAsync(IList<OrderItems> orderItems)
        {
            _context.OrderItems.AddRange(orderItems);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId) => await _context.Order.Where(order => order.UserID == userId).ToListAsync();

        public async Task<IEnumerable<OrderItems>> GetOrderItemsByOrderIdAsync(int orderId) => await _context.OrderItems.Where(orderItems => orderItems.OrderID == orderId).Include(x => x.Product).ToListAsync();

        public async Task UpdateOrderItemsAsync(OrderItems orderItems)
        {
            _context.OrderItems.Update(orderItems);
            await _context.SaveChangesAsync();
        }
    }
}
