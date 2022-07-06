import { Component } from '@angular/core';
import {NavbarService} from "./navbar.service";
import {SignalR} from "./signal-r";
import {environment} from "../environments/environment";
import {Router} from "@angular/router";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ParkingApp';
  constructor(public navService: NavbarService, private router: Router) {
    this.navService.show();
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigateByUrl('/');

  }

  ngOnInit() {

  }

  isLoggedIn() {
    return localStorage.getItem('token') != null;
  }
}
