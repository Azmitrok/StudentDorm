import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-bookings',
  templateUrl: './bookings.component.html'
})
export class BookingsComponent {
  public bookings: Booking[];
  public appointments: Appointment[];
  public currentDate: Date = new Date(2019, 2, 22);

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Booking[]>(baseUrl + 'api/Bookings/Index').subscribe(result => {
      
      this.bookings = this.datesConvert( result );
      let b = this.bookings;
    }, error => console.error(error));

    this.appointments = this.getAppointments();
    
  }

  private datesConvert(bookings: Booking[]) {
    for (let booking of bookings) {
      booking.startDateObj = new Date(booking.startDate);
      booking.endDateObj = new Date(booking.endDate);
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

  

  
}

export class Appointment {
  text: string;
  startDate: Date;
  endDate: Date;
  allDay?: boolean;
}

export class Booking {
  id: number;
  roomid: number;
  startDate: string;
  endDate: string;
  startDateObj: Date;
  endDateObj: Date;
  usedPalces: number;
 // gender: string;
  gender: Gender;  
}

export enum Gender {
  Male = 0,
  Female
}
