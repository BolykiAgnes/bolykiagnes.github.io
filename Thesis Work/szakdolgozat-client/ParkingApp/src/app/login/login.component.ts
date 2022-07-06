import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {Token} from "../token";
import {Router} from "@angular/router";
import Swal from "sweetalert2";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
  }

  login(email: HTMLInputElement, password: HTMLInputElement) {
    let loginData = {
      email: email.value,
      password: password.value
    };
    this.http.post<Token>(environment.apiUrl + 'login', loginData).subscribe(res => {
      localStorage.setItem('token', res.token);
      Swal.fire('Sikeres bejelentkezés!','Átirányítunk a főoldalra.','success').then(() => {
        this.router.navigateByUrl('/select').then();
      });


    }, err => {
      Swal.fire('Hiba!',err.error,'error').then(() => {

      });
    });
  }
}
