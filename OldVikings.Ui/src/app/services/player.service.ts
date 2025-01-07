import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {PlayerMvp} from "../models/playerMvp.model";

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  private readonly _serviceUrl: string = environment.playerManagerApiUrl + 'Players/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getMvpPlayers(): Observable<PlayerMvp[]> {
    return this._httpClient.get<PlayerMvp[]>(this._serviceUrl + 'GetAllianceMvpPlayers/' + '5EC07910-AD78-45DF-9FF8-08DD0A24FE1E');
  }
}
