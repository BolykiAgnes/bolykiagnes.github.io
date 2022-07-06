import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {RegisterComponent} from "./register/register.component";
import {LoginComponent} from "./login/login.component";
import {HomeComponent} from "./home/home.component";
import {SelectorComponent} from "./selector/selector.component";
import {AddPlateComponent} from "./add-plate/add-plate.component";
import {ReserveComponent} from "./reserve/reserve.component";
import {ReservationsComponent} from "./reservations/reservations.component";
import {EditReservationComponent} from "./edit-reservation/edit-reservation.component";
import {EntranceComponent} from "./entrance/entrance.component";
import {ParkingSpotComponent} from "./parking-spot/parking-spot.component";
import {AvailableSpotsComponent} from "./available-spots/available-spots.component";
import {AdminComponent} from "./admin/admin.component";

const routes: Routes = [
  {path: 'register', component: RegisterComponent},
  {path: 'login', component: LoginComponent},
  {path: 'home', component: HomeComponent},
  {path: 'select', component: SelectorComponent},
  {path: 'add-plates', component: AddPlateComponent},
  {path: 'reserve', component: ReserveComponent},
  {path: 'reservations', component: ReservationsComponent},
  {path: 'edit-reservation', component: EditReservationComponent},
  {path: 'entrance/:id', component: EntranceComponent},
  {path: 'spot/:id', component: ParkingSpotComponent},
  {path: 'availableSpots/:id', component: AvailableSpotsComponent},
  {path: 'admin', component: AdminComponent},
  {path: '**', component: SelectorComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
