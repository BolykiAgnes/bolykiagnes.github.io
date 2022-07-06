import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegisterComponent } from './register/register.component';
import {HttpClientModule} from "@angular/common/http";
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { SelectorComponent } from './selector/selector.component';
import { AddPlateComponent } from './add-plate/add-plate.component';
import { ReserveComponent } from './reserve/reserve.component';
import { ReservationsComponent } from './reservations/reservations.component';
import { EditReservationComponent } from './edit-reservation/edit-reservation.component';
import { EntranceComponent } from './entrance/entrance.component';
import { ParkingSpotComponent } from './parking-spot/parking-spot.component';
import { AvailableSpotsComponent } from './available-spots/available-spots.component';
import { AdminComponent } from './admin/admin.component';

@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    LoginComponent,
    HomeComponent,
    SelectorComponent,
    AddPlateComponent,
    ReserveComponent,
    ReservationsComponent,
    EditReservationComponent,
    EntranceComponent,
    ParkingSpotComponent,
    AvailableSpotsComponent,
    AdminComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
