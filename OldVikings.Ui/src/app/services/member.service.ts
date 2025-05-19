import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MemberService {

  private readonly _localStorageKey = 'oldVikingsKey';
  private readonly _currentKey = 'Valhalla!25';

  private redirectUri: string | null = null;

  get currentKey(): string {
    return this._currentKey;
  }

  get redirectUrl(): string | null {
    return this.redirectUri;
  }

  public getKey(): string | null {
    return localStorage.getItem(this._localStorageKey);
  }

  public setKey(key: string): void {
    localStorage.setItem(this._localStorageKey, key);
  }

  setRedirectUri(redirectUri: string): void {
    this.redirectUri = redirectUri;
  }

}
