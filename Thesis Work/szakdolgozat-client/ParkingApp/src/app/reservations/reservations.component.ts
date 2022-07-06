import { Component, OnInit } from '@angular/core';
import {Reservation} from "../reservation";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {Router} from "@angular/router";
import Swal from "sweetalert2";
import {SignalR} from "../signal-r";

@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.css']
})
export class ReservationsComponent implements OnInit {

  public reservations: Array<Reservation>;
  private signalR: SignalR;
  private signalREntrance: SignalR;
  private signalRExit: SignalR;
  constructor(private http: HttpClient, private router: Router) {
    this.signalR = new SignalR(environment.hub);
    this.signalR.register('SpotCalculationNeeded', () => {
      this.ngOnInit();
      return true;
    });
    this.signalR.register('EntranceMessageReceived', () => {
      this.ngOnInit();
      return true;
    });
    this.signalR.register('VehicleExited', () => {
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
    this.http.get<Array<Reservation>>(environment.apiUrl + "api/Reservation/user", {headers}).subscribe(res => {
      this.reservations = res;
      console.log(this.reservations);
    });
  }

  deleteReservation(reservationId) {
    const headers = environment.headers;
    this.http.delete(environment.apiUrl + "api/Reservation/" + reservationId, {headers}).subscribe(res => {
      Swal.fire('Sikeres törlés!','','success').then(() => {
        this.ngOnInit();
      });
    }, err => console.log(err));
  }

  editReservation(reservationId) {
    sessionStorage.setItem('reservationToEdit', reservationId.toString());
    this.router.navigateByUrl('/edit-reservation').then();
  }
}
