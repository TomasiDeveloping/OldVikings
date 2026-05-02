import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {BehaviorSubject, tap} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private _isR4LoggedIn$ = new BehaviorSubject<boolean>(false);
  private readonly _r4TokenKey = "r4Token";

  private readonly _serviceUrl: string = environment.apiBaseUrl + 'Authorizations';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  constructor() {
    const token = sessionStorage.getItem(this._r4TokenKey);
    this._isR4LoggedIn$.next(!!token);
  }

  get isR4LoggedIn$() {
    return this._isR4LoggedIn$.asObservable();
  }

  r4Login(password: string) {
    return this._httpClient.post<{token: string}>(this._serviceUrl + '/r4-login', {password: password})
      .pipe(
        tap(res => {
          sessionStorage.setItem(this._r4TokenKey, res.token);
          this._isR4LoggedIn$.next(true);
        })
      );
  }

  logout() {
    sessionStorage.removeItem(this._r4TokenKey);
    this._isR4LoggedIn$.next(false);
  }

  getR4AccessToken() : string | null {
    return sessionStorage.getItem(this._r4TokenKey);
  }
}
