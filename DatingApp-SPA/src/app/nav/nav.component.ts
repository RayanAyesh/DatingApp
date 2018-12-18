import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertfyService } from '../_services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(
    public authservice: AuthService,
    private alertify: AlertfyService
  ) {}

  ngOnInit() {}

  login() {
    this.authservice.login(this.model).subscribe(
      next => {
        this.alertify.success('logged in successfully');
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  loggedIn() {
    return this.authservice.loggedIn();
  }
  logout() {
    localStorage.removeItem('token');
    this.alertify.message('logged out');
  }
}
