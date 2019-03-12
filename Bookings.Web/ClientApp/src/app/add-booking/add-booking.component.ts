import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Booking, BookingsComponent } from '../bookings/bookings.component';

@Component({
  selector: 'app-add-booking',
  templateUrl: './add-booking.component.html'
})
export class AddBookingComponent {
  public booking: Booking;
  private httpClient;
  private baseUrl;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.booking = new Booking();
    this.httpClient = http;
    this.baseUrl = baseUrl;
  }

  public createBooking(booking: Booking) {
    return this.httpClient.post(this.baseUrl + 'api/Bookings/Add', booking);
  }
}


