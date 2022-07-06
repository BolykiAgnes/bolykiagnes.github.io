import {Component, OnDestroy, OnInit} from '@angular/core';
import {NavbarService} from "../navbar.service";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import Swal from "sweetalert2";
import {SignalR} from "../signal-r";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-entrance',
  templateUrl: './entrance.component.html',
  styleUrls: ['./entrance.component.css']
})
export class EntranceComponent implements OnInit, OnDestroy {

  public message: string;
  private signalR: SignalR;
  constructor(private nav: NavbarService, private http: HttpClient, private route: ActivatedRoute) {
    this.nav.hide()
    this.signalR = new SignalR(environment.hub);
    this.signalR.register('EntranceMessageReceived', () => {
      this.ngOnInit();
      return true;
    });
    this.signalR.start();
  }

  ngOnInit(): void {
    this.http.get<{message: string}>(environment.apiUrl + 'api/ParkingLot/message/' + this.route.snapshot.paramMap.get('id')).subscribe(res => {
      this.message = res.message;
      console.log(this.message);
      Swal.fire(
        {
          title: 'Üzenet',
          html: this.message,
          timer: 15000,
          timerProgressBar: true,
          showConfirmButton: false,
          //grow: "column"
        }
      ).then();
    }, error => console.log(error));

  }

  ngOnDestroy(): void {
    this.nav.show();
  }


}
