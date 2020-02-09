import axios from "axios";

/**
 * Order Service.
 * Provides UI business logic associated with sales orders.
 */
export class OrderService {
  API_URL = process.env.VUE_APP_API_URL;

  public async getOrders(): Promise<any> {
    let result: any = await axios.get(`${this.API_URL}/order/`);
    return result.data;
  }

  public async markOrderComplete(id: number): Promise<any> {
    let result: any = await axios.patch(`${this.API_URL}/order/complete/${id}`);
    return result.data;
  }
}
