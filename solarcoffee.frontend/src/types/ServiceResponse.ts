export interface IServiceResponse<T> {
  isSuccess: boolean;
  message: string;
  time: Date;
  data: T;
}
