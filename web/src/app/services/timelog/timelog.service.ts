import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TimelogService {
  private apiUrl = `${environment.apiUrl}v1/timelog`;

  constructor(private http: HttpClient) { }

  saveTime(param: any): Observable<any> {
    return this.http.post<string>(this.apiUrl, param)
      .pipe(map((res: any) => {
        return res;
      }),
        catchError((err) => {
          return throwError(() => err);
        })
      );
  }

  getCurrentLog(userId: any, date: any) {
    return this.http.get<string>(`${this.apiUrl}/current?userId=${userId}&date=${date}`)
      .pipe(map((res: any) => {
        return res;
      }),
        catchError((err) => {
          return throwError(() => err);
        })
      );
  }

  getLogs(userId: any): Observable<any> {
    return this.http.get<string>(`${this.apiUrl}?userId=${userId}`)
      .pipe(map((res: any) => {
        return res;
      }),
        catchError((err) => {
          return throwError(() => err);
        })
      );
  }
}
