import {Component, OnDestroy, OnInit} from '@angular/core';
import {SpotStatusViewModel} from "../spot-status-view-model";
import {ActivatedRoute} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {NavbarService} from "../navbar.service";
import {environment} from "../../environments/environment";
import {SignalR} from "../signal-r";

@Component({
  selector: 'app-available-spots',
  templateUrl: './available-spots.component.html',
  styleUrls: ['./available-spots.component.css']
})
export class AvailableSpotsComponent implements OnInit, OnDestroy {

  parkingLotId: string;
  public availableSpots: number;
  private signalR: SignalR
  constructor(private route: ActivatedRoute, private http: HttpClient, private nav: NavbarService) {
    this.signalR = new SignalR(environment.hub);
    this.signalR.register('SpotCalculationNeeded', () => {
      this.ngOnInit();
      return true;
    });
    this.signalR.register('VehicleExited', () => {
      this.ngOnInit();
      return true;
    });
    this.signalR.start();
  }

  getAvailableSpots() {
    this.http.get<number>(environment.apiUrl + 'api/ParkingLot/AvailableSpots/' + this.parkingLotId).subscribe(res => {
      this.availableSpots = res;
      console.log(this.availableSpots);
    }, err => console.log(err));
  }

  ngOnInit(): void {
    this.nav.hide();
    this.parkingLotId = this.route.snapshot.paramMap.get('id');
    this.getAvailableSpots();
  }

  ngOnDestroy(): void {
    this.nav.show();
  }

}
