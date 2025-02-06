import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {PlayerMvp} from "../models/playerMvp.model";

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  private readonly _serviceUrl: string = environment.playerManagerApiUrl + 'Players/Mvp';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getMvpPlayers(playerType: string): Observable<PlayerMvp[]> {
    const headers = new HttpHeaders(
      {'X-Api-Key': environment.apiKey}
    );
    let params = new HttpParams();
    params = params.append('allianceId', environment.allianceId);
    params = params.append('playerType', playerType);
    return this._httpClient.get<PlayerMvp[]>(this._serviceUrl, {params: params, headers: headers});
  }
}
