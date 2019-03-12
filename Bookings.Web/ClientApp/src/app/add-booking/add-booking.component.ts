import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Booking, BookingsComponent } from '../bookings/bookings.component';

@Component({
  selector: 'app-bookings',
  templateUrl: './bookings.component.html'
})
export class AddBookingComponent {
  public booking: Booking;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.booking = new Booking();
  }

  public createBooking(booking: Booking) {
    //return http.post('${this.apiURL}/customers/', customer);
  }
}


