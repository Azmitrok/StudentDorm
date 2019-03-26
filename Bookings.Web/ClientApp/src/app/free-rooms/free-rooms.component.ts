import { Component, Inject, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Gender } from '../bookings/bookings.component';


@Component({
  selector: 'app-free-rooms',
  templateUrl: './free-rooms.component.html'
})
export class FreeRoomsComponent {
  public rooms: Room[];
  private httpClient: HttpClient;
  private baseUrl: string;
  public startDate: Date;
  public endDate: Date;
  public selectedGender: Gender;
  public usedPlaces: number;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.httpClient = http;
    this.baseUrl = baseUrl;

    this.startDate = new Date();
    this.endDate = new Date();
    this.endDate.setDate(this.endDate.getDate() + 1);

    this.selectedGender = Gender.Female;
    this.usedPlaces = 1;

    this.findFreeRooms(this.startDate.toDateString(), this.endDate.toDateString(), this.selectedGender, this.usedPlaces);
  }

  public findFreeRooms(startDate: string, endDate: string, gender: Gender, usedPlaces: number) {
    let params = new HttpParams()
      .set('startDate', startDate)
      .set('endDate', endDate)
      .set('usedPlaces', usedPlaces.toString())
      .set('gender', gender.toString());

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
