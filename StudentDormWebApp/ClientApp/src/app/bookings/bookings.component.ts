import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-bookings',
  templateUrl: './bookings.component.html'
})
export class BookingsComponent {
  public bookings: Bookings[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Bookings[]>(baseUrl + 'api/Bookings/').subscribe(result => {
      this.bookings = result;
    }, error => console.error(error));
  }
}

interface Bookings {
  room: string;
  startDate: Date;
  endDate: Date;
  usedPalces: number;
  gender: string;
}
