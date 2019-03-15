import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { _throw as throwError } from 'rxjs/observable/throw';
import { catchError, retry, tap } from 'rxjs/operators';

import { Booking, BookingsComponent } from '../bookings/bookings.component';
import { Room } from '../free-rooms/free-rooms.component';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Component({
  selector: 'app-add-booking',
  templateUrl: './add-booking.component.html'
})


export class AddBookingComponent {
  public booking: Booking;
  public selectedRoom: Room;
  public allRooms: Room[];
  private httpClient: HttpClient;
  private baseUrl;
  private router: Router;
  private errorMessage: string;


  constructor(router: Router, http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.booking = new Booking();
    this.httpClient = http;
    this.baseUrl = baseUrl;
    this.router = router;
    http.get<Room[]>(baseUrl + 'api/Bookings/AllRooms').subscribe(result => {
      this.allRooms = result;
    }, error => console.error(error));


  }

  public setSelectedRoom(room: Room) {
    this.selectedRoom = room;

  }

  public addBooking(booking: Booking) {
    this.booking.roomid = +this.selectedRoom;
    let url = this.baseUrl + 'api/Bookings/';

    var isAdded = this.httpClient.post<Booking>(url, booking, httpOptions).subscribe(
      response => {
        if (response)
          this.router.navigateByUrl('/bookings');
        else
          this.errorMessage = "The room is busy on this period.";
      },
      err => console.log(err),
    );



  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      //this.log('${operation} failed: ${error.message}');

      // Let the app keep running by returning an empty result.
      //return ok(result as T);
      return null;
    };
  }

  private log(message: string) {
    //this.messageService.add('HeroService: ${message}');
    let s = message;
    console.log(message);
  }
}


