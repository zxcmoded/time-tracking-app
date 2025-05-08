import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, interval, map } from 'rxjs';
import { AuthService } from 'src/app/services/auth/auth.service';
import { TimelogService } from 'src/app/services/timelog/timelog.service';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.scss']
})
export class FormComponent implements OnInit {
  inLabel: any = null;
  outLabel: any = null;
  userId = '';

  currentTimeSubject = new BehaviorSubject<Date>(new Date());

  constructor(private readonly authService: AuthService,
    private readonly timelogService: TimelogService,
    private readonly spinner: NgxSpinnerService,
    private readonly toastr: ToastrService,
  ) {
    setInterval(() => {
      this.currentTimeSubject.next(new Date());
    }, 1000);
  }

  ngOnInit(): void {
    this.userId = this.authService.userSubject.value['Id'];
    this.spinner.show();
    this.timelogService.getCurrentLog(this.userId, new Date().toLocaleString()).subscribe(res => {
      if (res) {
        this.inLabel = res.timeIn;
        this.outLabel = res.timeOut;
      }
      this.spinner.hide();
    }, err => {
      this.spinner.hide();
    })
  }

  async clockIn() {
    var param = {
      UserId: this.userId,
      TimeIn: this.currentTimeSubject.value.toLocaleString(),
      DateCreated: new Date().toLocaleString()
    }

    this.spinner.show();
    var result = await this.timelogService.saveTime(param).toPromise();
    if (result.success) {
      this.inLabel = param.TimeIn;
    }
    this.spinner.hide();
  }

  async clockOut() {
    var param = {
      UserId: this.userId,
      TimeOut: this.currentTimeSubject.value.toLocaleString(),
      DateCreated: new Date().toLocaleString()
    }

    this.spinner.show();
    var result = await this.timelogService.saveTime(param).toPromise();
    if (result.success) {
      this.outLabel = param.TimeOut;
    } else {
      this.toastr.error(result.message);
    }
    this.spinner.hide();
  }
}

