import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthService } from 'src/app/services/auth/auth.service';
import { TimelogService } from 'src/app/services/timelog/timelog.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {
  timeLogs: any[] = [];

  constructor(private readonly timeLogService: TimelogService,
    private readonly spinner: NgxSpinnerService,
    private readonly authService: AuthService,
  ) { }

  ngOnInit(): void {
    this.loadList();
  }

  loadList() {
    var user = this.authService.userSubject.value;
    this.spinner.show();
    this.timeLogService.getLogs(user.Id).subscribe(res => {
      this.timeLogs = res;
      this.spinner.hide();
    }, err => {
      this.spinner.hide();
    })
  }

  formatDuration(decimalHours: number): string {
    if (decimalHours) {
      const totalMinutes = Math.round(decimalHours * 60);
      const hours = Math.floor(totalMinutes / 60);
      const minutes = totalMinutes % 60;

      const hourPart = hours > 0 ? `${hours} hour${hours > 1 ? 's' : ''}` : '';
      const minutePart = minutes > 0 ? `${minutes} min${minutes > 1 ? 's' : ''}` : '';
      return [hourPart, minutePart].filter(Boolean).join(' ');
    }

    return 'N/A'
  }
}
