import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { jwtDecode } from "jwt-decode";
import { BehaviorSubject, Observable, map, catchError, throwError } from "rxjs";
import { environment } from "src/environments/environment";


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}v1/Auth`;
  public userSubject = new BehaviorSubject<any>(null); // Initial value: null

  constructor(private http: HttpClient) {
    const token = localStorage.getItem('token');
    if (token) {
      this.setUser(token);
    }
  }

  authenticate(credentials: any): Observable<any> {
    return this.http.post<string>(this.apiUrl, credentials)
      .pipe(map((res: any) => {
        var token = res.message;
        this.saveToken(token);
        this.setUser(token)
        return token;
      }),
        catchError((err) => {
          return throwError(() => err);
        })
      );
  }

  logout() {
    localStorage.clear();
    this.clearUser();
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    const helper = new JwtHelperService();
    const isExpired = helper.isTokenExpired(token);
    return !isExpired;
  }

  private saveToken(token: string) {
    localStorage.setItem('token', token);
  }

  private setUser(token: string): void {
    const user: any = jwtDecode(token);
    this.userSubject.next(user);
  }

  private clearUser(): void {
    this.userSubject.next(null);
  }
}

