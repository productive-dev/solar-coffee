using System;

namespace SolarCoffee.Data.Models {
    public class ProductInventorySnapshot {
        public int Id { get; set; }
        public DateTime SnapshotTime { get; set; }
        public int QuantityOnHand { get; set; }
        public Product Product { get; set; }
    }
}