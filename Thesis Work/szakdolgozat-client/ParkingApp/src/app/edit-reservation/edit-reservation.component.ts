import { Component, OnInit } from '@angular/core';
import {environment} from "../../environments/environment";
import {User} from "../user";
import {HttpClient} from "@angular/common/http";
import {LicensePlate} from "../license-plate";
import {Reservation} from "../reservation";
import {Router} from "@angular/router";
import Swal from "sweetalert2";

@Component({
  selector: 'app-edit-reservation',
  templateUrl: './edit-reservation.component.html',
  styleUrls: ['./edit-reservation.component.css']
})
export class EditReservationComponent implements OnInit {

  public licensePlates: Array<string>;
  public user: User;
  public reservation: Reservation;
  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.getUser();
    this.getReservations();
  }

  getReservations() {
    const headers = environment.headers;
    let reservationId = parseInt(sessionStorage.getItem('reservationToEdit'));
    this.http.get<Array<Reservation>>(environment.apiUrl + "api/Reservation/user", {headers}).subscribe(res => {
      this.reservation = res.filter(x=>x.reservationID == reservationId)[0];
      console.log(this.reservation);
    });
  }

  getUser() {
    const headers = environment.headers;
    this.http.get<User>(environment.apiUrl + 'get-user', {headers}).subscribe(res => {
      this.user = res;
      this.licensePlates = this.user.licensePlates;
    }, err => console.log(err));
  }

  getCurrentDate() {
    return new Date().toISOString().slice(0, 10);
  }


  editReservation(licensePlate: HTMLSelectElement, day: HTMLInputElement) {
    let reservationID = parseInt(sessionStorage.getItem('reservationToEdit'));
    const headers = environment.headers;
    const formData = {
      reservationID: reservationID,
      licensePlateText: licensePlate.value,
      day: day.value
    };
    this.http.put(environment.apiUrl + "api/Reservation", formData, {headers}).subscribe(res => {
      sessionStorage.removeItem('reservationToEdit');
      Swal.fire('Sikeres módosítás!','','success').then(() => {
        this.router.navigateByUrl('/reservations').then();
      });
    }, err => console.log(err));

  }
}
