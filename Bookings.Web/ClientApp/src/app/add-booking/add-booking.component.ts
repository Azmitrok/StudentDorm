import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable} from 'rxjs';
import { _throw as throwError } from 'rxjs/observable/throw';
import { catchError, retry } from 'rxjs/operators';

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
    let url = this.baseUrl + 'api/Bookings/';
    return this.httpClient.post(url, booking);
  }

  public addBooking(booking: Booking): Observable<Booking> {
    let url = this.baseUrl + 'api/Bookings/';
    return this.httpClient.post(url, booking)
      .pipe(
        //catchError(this.handleError(null))
      );
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    // return an observable with a user-facing error message
    return throwError(
      'Something bad happened; please try again later.');
  };
}


