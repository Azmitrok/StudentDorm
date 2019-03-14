import { Component, Inject, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, HttpParams } from '@angular/common/http';
import { NgbDateStruct, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-free-rooms',
  templateUrl: './free-rooms.component.html'
})
export class FreeRoomsComponent {
  public rooms: Room[];
  private httpClient: HttpClient;
  private baseUrl : string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
  }

  public getFreeRooms(startDate: Date, endDate: Date) {
    let params = new HttpParams()
      .set('startDate', startDate.toDateString())
      .set('endDate', endDate.toDateString());

    this.httpClient.get<Room[]>(this.baseUrl + 'api/Bookings/FreeRooms', { params: params }).subscribe(result => {
      this.rooms = result;
    }, error => console.error(error));
  }

}

export class Room {
  number: number;
  availableSpaces: number;
}
