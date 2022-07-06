import { Component, OnInit } from '@angular/core';
import {ParkingLot} from "../parking-lot";
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {User} from "../user";
import {LicensePlate} from "../license-plate";
import {SweetAlert2Module} from "@sweetalert2/ngx-sweetalert2";
import Swal from "sweetalert2";

@Component({
  selector: 'app-reserve',
  templateUrl: './reserve.component.html',
  styleUrls: ['./reserve.component.css']
})
export class ReserveComponent implements OnInit {

  public parkingLots: Array<ParkingLot>;
  public licensePlates: Array<string>;
  public user: User;
  constructor(private http: HttpClient, private router: Router) {
    this.getParkingLots();
    this.getUser();
  }

  ngOnInit(): void {

  }

  getParkingLots(){
    this.http.get<Array<ParkingLot>>(environment.apiUrl + 'api/ParkingLot').subscribe(res => {
      this.parkingLots = res;
      console.log(res);
    }, err => console.log(err));
  }

  getUser() {
    const headers = environment.headers;
    this.http.get<User>(environment.apiUrl + 'get-user', {headers}).subscribe(res => {
      this.user = res;
      this.licensePlates = this.user.licensePlates;
    }, err => console.log(err));
  }

  isSelected(id: number): boolean {
    const selected = parseInt(localStorage.getItem('parkingLot'));
    if (selected == id){
      return true;
    }
    return false;
  }

  makeReservation(parkingLot: HTMLSelectElement, licensePlate: HTMLSelectElement, size: HTMLSelectElement, day: HTMLInputElement) {
    const headers = environment.headers;
    let formData = {
      parkingLotId: parkingLot.value,
      size: size.value,
      licensePlateText: licensePlate.value,
      day: day.value
    };

    this.http.post(environment.apiUrl + 'api/Reservation', formData, {headers}).subscribe(res =>{

      Swal.fire('Sikeres foglalás!','','success').then(() => {
        this.router.navigateByUrl('/reservations').then();
      });
      console.log(res);
    }, err => {
      Swal.fire('Hiba!',err.error,'error');
    });

  }

  getCurrentDate() {
    return new Date().toISOString().slice(0, 10);
  }


}
