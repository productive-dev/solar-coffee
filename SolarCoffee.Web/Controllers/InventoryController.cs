using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers {
    [ApiController]
    public class InventoryController : ControllerBase {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(
            ILogger<InventoryController> logger,
            IInventoryService inventoryService
        ) {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        [HttpGet("/api/inventory")]
        public ActionResult GetCurrentInventory() {
            _logger.LogInformation("Getting all inventory...");
            
            var inventory = _inventoryService.GetCurrentInventory()
                .Select(pi => new ProductInventoryModel {
                    Id = pi.Id,
                    Product = ProductMapper.SerializeProductModel(pi.Product),
                    IdealQuantity = pi.IdealQuantity,
                    QuantityOnHand = pi.QuantityOnHand
                })
                .OrderBy(inv => inv.Product.Name)
                .ToList();

            return Ok(inventory);
        }

        [HttpPatch("/api/inventory")]
        public ActionResult UpdateInventory([FromBody] ShipmentModel shipment) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var id = shipment.ProductId;
            var adjustment = shipment.Adjustment;
            var inventory = _inventoryService.UpdateUnitsAvailable(id, adjustment);
            return Ok(inventory);
        }

        [HttpGet("/api/inventory/snapshot")]
        public ActionResult GetSnapshotHistory() {
            
            // ReSharper disable once InvalidXmlDocComment
            /**
             * {
             *     timeline: [1, 2, 3 .. n],
             *     inventory: [{ id: 1, qty: [43, 21, 32 .. n ] }, { id: 2, qty: [43, 12, 43 .. n]}]
             * }
             */
            
            _logger.LogInformation("Getting snapshot history");

            try {
                var snapshotHistory = _inventoryService.GetSnapshotHistory();
                
                // Get distinct points in time a snapshot was collected
                var timelineMarkers = snapshotHistory 
                    .Select(t => t.SnapshotTime)
                    .Distinct()
                    .ToList();
                
                // Get quantities grouped by id.
                var snapshots = snapshotHistory
                    .GroupBy(hist => hist.Product, hist => hist.QuantityOnHand,
                        (key, g) => new ProductInventorySnapshotModel {
                            ProductId = key.Id,
                            QuantityOnHand = g.ToList()
                        })
                    .OrderBy(hist => hist.ProductId)
                    .ToList();
                
                var viewModel = new SnapshotResponse {
                    Timeline = timelineMarkers,
                    ProductInventorySnapshots = snapshots
                };
                
                return Ok(viewModel);
            }
            catch (Exception e) {
                _logger.LogError("Error getting snapshot history.");
                _logger.LogError(e.StackTrace);
                return BadRequest("Error retrieving history");
            }
        }
    }
}