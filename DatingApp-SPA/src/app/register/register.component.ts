import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertfyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // @Input() valuesFromHome: any;
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  constructor(private authservice: AuthService, private alertify: AlertfyService) {}

  ngOnInit() {}
  register() {
    this.authservice.register(this.model).subscribe(
      next => {
        this.alertify.success('register in successfully');
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  cancel() {
    this.cancelRegister.emit(false);
    this.alertify.message('cancelled');
  }
}
