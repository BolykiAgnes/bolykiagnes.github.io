import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import Swal from "sweetalert2";

@Component({
  selector: 'app-add-plate',
  templateUrl: './add-plate.component.html',
  styleUrls: ['./add-plate.component.css']
})
export class AddPlateComponent implements OnInit {

  constructor(private router: Router, private http: HttpClient ) { }

  ngOnInit(): void {
  }

  addPlates(plates: HTMLInputElement) {
    let plateArray = [];
    if (plates.value.split(',').length > 0) {
      plateArray = plates.value.split(',');
    }
    else{
      plateArray.push(plates.value);
    }
    const headers = environment.headers;
    this.http.post(environment.apiUrl + 'add-plates', plateArray, {headers}).subscribe(res => {
      Swal.fire('Sikeres hozzáadás!','A rendszám(ok) sikeresen fel lettek véve.','success').then(() => {
        this.router.navigateByUrl('/select');
      });
    }, err => Swal.fire('Sikeres hozzáadás!',err.error,'error'));

  }
}
