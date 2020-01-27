using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Services.Product;

namespace SolarCoffee.Services.Order {
    public class OrderService : IOrderService {

        private readonly SolarDbContext _db;
        private readonly ILogger<OrderService> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;

        public OrderService(
            SolarDbContext db,
            ILogger<OrderService> logger,
            IProductService productService,
            IInventoryService inventoryService
            ) {
            _db = db;
            _logger = logger;
            _productService = productService;
            _inventoryService = inventoryService;
        }
        
        /// <summary>
        /// Gets all SalesOrders in the system
        /// </summary>
        /// <returns></returns>
        public List<SalesOrder> GetOrders() {
            return _db.SalesOrders
                .Include(so => so.Customer)
                    .ThenInclude(customer => customer.PrimaryAddress)
                .Include(so => so.SalesOrderItems)
                    .ThenInclude(item => item.Product)
                .ToList();
        }

        /// <summary>
        /// Creates an open SalesOrder
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public ServiceResponse<bool> GenerateOpenOrder(SalesOrder order) {
            var now = DateTime.UtcNow;
            
            _logger.LogInformation("Generating new order");
            
            foreach (var item in order.SalesOrderItems) {
                item.Product = _productService
                    .GetProductById(item.Product.Id);
                var inventoryId = _inventoryService
                    .GetByProductId(item.Product.Id).Id;
                _inventoryService
                    .UpdateUnitsAvailable(inventoryId, -item.Quantity);
            }

            try {
                _db.SalesOrders.Add(order);
                _db.SaveChanges();

                return new ServiceResponse<bool> {
                    IsSuccess = true,
                    Data = true,
                    Message = "Open order created",
                    Time = now 
                };
            }

            catch (Exception e) {
                return new ServiceResponse<bool> {
                    IsSuccess = false,
                    Data = false,
                    Message = e.StackTrace,
                    Time = now
                };
            }
        }

        /// <summary>
        /// Marks an open SalesOrder as paid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResponse<bool> MarkFulfilled(int id) {
            var now = DateTime.UtcNow;
            var order = _db.SalesOrders.Find(id);
            order.UpdatedOn = now;
            order.IsPaid = true;

            try {
                _db.SalesOrders.Update(order);
                _db.SaveChanges();

                return new ServiceResponse<bool> {
                    IsSuccess = true,
                    Data = true,
                    Message = $"Order {order.Id} closed: Invoice paid in full.",
                    Time = now
                };
            }
            catch (Exception e) {
                return new ServiceResponse<bool> {
                    IsSuccess = false,
                    Data = false,
                    Message = e.StackTrace,
                    Time = now
                };
            }
        }
    }
}