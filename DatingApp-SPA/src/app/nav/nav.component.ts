import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AletifyService } from '../_services/alertify.service';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = { };

  constructor(private authService: AuthService, private alertify: AletifyService ) { }

  ngOnInit() {
    console.log(this.model);
  }
  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Logged in successfully');
    }, error => {
      this.alertify.error(error);
    });
  }
  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token;
  }
  logout() {
      localStorage.removeItem('token');
     this.alertify.message('logged out');

  }
}
