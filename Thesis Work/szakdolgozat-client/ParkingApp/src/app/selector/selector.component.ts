import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {ParkingLot} from "../parking-lot";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {SignalR} from "../signal-r";

@Component({
  selector: 'app-selector',
  templateUrl: './selector.component.html',
  styleUrls: ['./selector.component.css']
})
export class SelectorComponent implements OnInit {

  public parkingLots: Array<ParkingLot>;
  private signalR: SignalR;
  constructor(private router: Router, private http: HttpClient) {
    this.signalR = new SignalR(environment.hub);
    this.signalR.register('SpotCalculationNeeded', () => {
      this.ngOnInit();
      return true;
    });
    this.signalR.start();
  }

  ngOnInit(): void {
    this.getParkingLots();
  }

  setParkingLot(number: number) {
    localStorage.setItem('parkingLot', number.toString());
    this.router.navigateByUrl('/reserve').then();

  }

  getParkingLots(){
    this.http.get<Array<ParkingLot>>(environment.apiUrl + 'api/ParkingLot').subscribe(res => {
      this.parkingLots = res;
      console.log(res);
    }, err => console.log(err));
  }

  getAvailableSpots(id: number) {
    let available = 0;
    this.http.get<number>(environment.apiUrl + 'api/ParkingLot/AvailableSpots/' + id).subscribe(res => {
      available = res;
      console.log(res);
    }, err => console.log(err));
    return available;
  }

  isLoggedIn() {
      return localStorage.getItem('token') != null;
  }
}
