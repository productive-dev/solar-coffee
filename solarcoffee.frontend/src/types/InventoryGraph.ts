export interface IInventoryTimeline {
  productInventorySnapshots: ISnapshot[];
  timeline: Date[];
}

export interface ISnapshot {
  productId: number;
  quantityOnHand: number[];
}
