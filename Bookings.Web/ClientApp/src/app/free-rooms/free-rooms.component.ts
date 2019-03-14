import { Component, Inject, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, HttpParams } from '@angular/common/http';

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

  public findFreeRooms(startDate: string, endDate: string) {
    let params = new HttpParams()
      .set('startDate', startDate)
      .set('endDate', endDate);

    this.httpClient.get<Room[]>(this.baseUrl + 'api/Bookings/FreeRooms', { params: params }).subscribe(result => {
      this.rooms = result;
    }, error => console.error(error));
  }

}

export class Room {
  id: number;
  number: number;
  availableSpaces: number;
}
