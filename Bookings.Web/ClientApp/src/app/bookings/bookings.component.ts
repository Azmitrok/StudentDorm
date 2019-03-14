import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-bookings',
  templateUrl: './bookings.component.html'
})
export class BookingsComponent {
  public bookings: Booking[];  

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Booking[]>(baseUrl + 'api/Bookings/Index').subscribe(result => {
      this.bookings = result;      
    }, error => console.error(error));
  }


  
}

export class Booking {
  id: number;
  roomid: number;
  startDate: Date;
  endDate: Date;
  usedPalces: number;
  gender: string;  
}
