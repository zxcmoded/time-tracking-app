import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent implements OnInit {
  form = new FormGroup({
    userName: new FormControl('', Validators.required),
    passWord: new FormControl('', Validators.required),
  });

  constructor(private readonly authService: AuthService,
    private readonly spinner: NgxSpinnerService,
    private readonly router: Router,
  ) {
  }

  ngOnInit(): void {
  }

  onSubmit() {
    if (this.form.valid) {
      this.spinner.show();
      this.authService.authenticate(this.form.getRawValue()).subscribe(() => {
        this.spinner.hide();
        void this.router.navigate(['/timelog/form']);
      }, error => {
        this.spinner.hide();
      });
    }
  }
}

