import { ICustomer } from "@/types/Customer";
import { ILineItem } from "@/types/IInvoice";

export interface ISalesOrder {
  id: number;
  createdOn: Date;
  updatedOn?: Date;
  customer: ICustomer;
  isPaid: boolean;
  salesOrderItems: ILineItem[];
}
