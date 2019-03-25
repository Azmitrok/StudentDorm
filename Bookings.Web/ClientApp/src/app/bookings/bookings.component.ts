import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Room } from '../free-rooms/free-rooms.component';


@Component({
  selector: 'app-bookings',
  templateUrl: './bookings.component.html'
})
export class BookingsComponent {
  public bookings: Booking[];
  public filteredBookings: Booking[];
  //public appointments: Appointment[];
  public currentDate: Date = new Date(2019, 2, 22);
  public allRooms: Room[];
  public resourcesData: Resource[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    http.get<Room[]>(baseUrl + 'api/Bookings/AllRooms').subscribe(result => {
      this.allRooms = result;
    }, error => console.error(error));

    http.get<Booking[]>(baseUrl + 'api/Bookings/Index').subscribe(result => {
      
      this.bookings = this.prepareBookings(result);
      this.filteredBookings = this.bookings;
      
      
    }, error => console.error(error));

    //this.appointments = this.getAppointments();
    this.resourcesData = this.getResources();
    
  }

  onRoomChange(newRoom) {

    this.filteredBookings = this.bookings.filter(b => b.roomId === +newRoom);
        
  }

  private prepareBookings(bookings: Booking[]) {
    for (let booking of bookings) {
      booking.startDateObj = new Date(booking.startDate);
      booking.endDateObj = new Date(booking.endDate);
      booking.scheduleText = "Room: " + booking.room.number + " ( " + booking.usedPlaces + " )"
    }

    return bookings;
  }

  private getAppointments() {
    let appointments: Appointment[] = [
      {
        text: "student1",
        startDate: new Date(2019, 2, 23, 9, 30),
        endDate: new Date(2019, 2, 23, 19, 30)
      },
      {
        text: "stud2",
        startDate: new Date(2019, 3, 23, 19, 30),
        endDate: new Date(2019, 3, 23, 22, 10)
      }
    ];

    return appointments;
  }

  private getResources() {
    let resourcesData: Resource[] = [
      {
        text: "Male",
        id: 0,
        color: "#1D69D2"
      }, {
        text: "Female",
        id: 1,
        color: "#DC5084"
      }
    ]

    return resourcesData;
  }

  

  
}

export class Resource {
  text: string;
  id: number;
  color: string;
}

export class Appointment {
  text: string;
  startDate: Date;
  endDate: Date;
  allDay?: boolean;
}

export class Booking {
  id: number;
  roomId: number;
  room: Room;
  startDate: string;
  endDate: string;
  startDateObj: Date;
  endDateObj: Date;
  usedPlaces: number;
 // gender: string;
  gender: Gender;
  scheduleText: string;
}

export enum Gender {
  Male = 0,
  Female
}
