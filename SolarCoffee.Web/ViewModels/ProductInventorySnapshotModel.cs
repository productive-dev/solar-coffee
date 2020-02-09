using System;
using System.Collections.Generic;

namespace SolarCoffee.Web.ViewModels {
    /// <summary>
    /// Snapshot view model
    /// </summary>
    public class ProductInventorySnapshotModel {
        public List<int> QuantityOnHand { get; set; }
        public int ProductId { get; set; }
    }
    
    /// <summary>
    /// Snapshot history in format suitable for graphing
    /// </summary>
    public class SnapshotResponse {
        public List<ProductInventorySnapshotModel> ProductInventorySnapshots { get; set; }
        public List<DateTime> Timeline { get; set; }
    }
}