import {Component, OnDestroy, OnInit, Renderer2} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {SpotStatusViewModel} from "../spot-status-view-model";
import {environment} from "../../environments/environment";
import Swal from "sweetalert2";
import {NavbarService} from "../navbar.service";
import {SignalR} from "../signal-r";

@Component({
  selector: 'app-parking-spot',
  templateUrl: './parking-spot.component.html',
  styleUrls: ['./parking-spot.component.css']
})
export class ParkingSpotComponent implements OnInit, OnDestroy {

  spotId: string;
  public spotStatus: SpotStatusViewModel;
  private signalR: SignalR;
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

  ngOnDestroy(): void {
    this.nav.show();
  }

  ngOnInit(): void {
    this.nav.hide();
    this.spotId = this.route.snapshot.paramMap.get('id');
    this.getStatus();
  }

  getStatus() {
    this.http.get<SpotStatusViewModel>(environment.apiUrl + 'api/ParkingLot/Spot/' + this.spotId).subscribe(res => {
      this.spotStatus = res;
    }, err => console.log(err));
  }



}
