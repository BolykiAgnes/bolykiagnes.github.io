export class Reservation {
  public reservationID: number;
  public spotId: number;
  public userId: number;
  public licensePlateText: string;
  public status: string;
  public parkingLotName: string;
  public day: Date;
  public createTimeStamp: Date;
  public arrivalTimeStamp: Date;
  public exitTimeStamp: Date;
}
