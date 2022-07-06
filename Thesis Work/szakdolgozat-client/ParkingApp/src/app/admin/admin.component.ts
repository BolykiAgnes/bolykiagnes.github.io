import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Reservation} from "../reservation";
import {environment} from "../../environments/environment";
import Swal from "sweetalert2";
import {Router} from "@angular/router";
import {SignalR} from "../signal-r";

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  reservations: Array<Reservation>;
  private signalR: SignalR;

  constructor(private http: HttpClient, private router: Router) {
    this.signalR = new SignalR(environment.hub);
    this.signalR.register('SpotCalculationNeeded', () => {
      this.ngOnInit();
      return true;
    });
    this.signalR.start();

  }

  ngOnInit(): void {
    this.getReservations();
  }

  getReservations() {
    const headers = environment.headers;
    this.http.get<Array<Reservation>>(environment.apiUrl + "api/Reservation/", {headers}).subscribe(res => {
      this.reservations = res;
      console.log(this.reservations);
    });
  }

  setArrived(reservationID: any) {
    const headers = environment.headers;
    const obj = {
      reservationId: reservationID,
      status: 1
    };
    this.http.put(environment.apiUrl + 'api/Reservation/editStatus', obj, {headers}).subscribe(res => {
      Swal.fire('Sikeres módosítás!','','success').then(() => {
        this.router.navigateByUrl('/admin').then();
      }, err => Swal.fire('Hiba!',err.error,'error'));
    });


  }

  setExited(reservationID: any) {
    const headers = environment.headers;
    const obj = {
      reservationId: reservationID,
      status: 2
    };
    this.http.put(environment.apiUrl + 'api/Reservation/editStatus', obj, {headers}).subscribe(res => {
      Swal.fire('Sikeres módosítás!','','success').then(() => {
        this.router.navigateByUrl('/admin').then();
      }, err => Swal.fire('Hiba!',err.error,'error'));
    });

  }

  delete(reservationID: any) {
    const headers = environment.headers;
    this.http.delete(environment.apiUrl + 'api/Reservation/delete/' + reservationID, {headers}).subscribe(res => {
      Swal.fire('Sikeres törlés!','','success');
    }, err => Swal.fire('Hiba!',err.error,'error'));



  }
}
