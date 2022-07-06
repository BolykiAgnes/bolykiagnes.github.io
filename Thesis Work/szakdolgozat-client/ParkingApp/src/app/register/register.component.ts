import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {Router} from "@angular/router";
import Swal from "sweetalert2";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
  }

  register(firstName: HTMLInputElement, lastName: HTMLInputElement, phone: HTMLInputElement, plates: HTMLInputElement, email: HTMLInputElement, password: HTMLInputElement) {
    let plateArray = [];
    if (plates.value.split(',').length > 0) {
      plateArray = plates.value.split(',');
    }
    else{
      plateArray.push(plates.value);
    }

    let formData = {
      Email: email.value,
      firstName: firstName.value,
      lastName: lastName.value,
      phoneNumber: phone.value,
      password: password.value,
      licensePlates: plateArray
    };

    this.http.post(environment.apiUrl + 'register', formData).subscribe(res=>{
      Swal.fire('Sikeres regisztráció!','Most már bejelentkezhetsz.','success').then(() => {
        this.router.navigateByUrl('/login').then();
      });
    }, error => {console.log(error)});

  }
}
