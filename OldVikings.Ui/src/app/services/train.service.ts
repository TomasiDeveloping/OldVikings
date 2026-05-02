import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {PlayerModel} from "../models/player.model";
import {WeeklyScheduleModel} from "../models/weeklySchedule.model";
import {TrainConductorModel} from "../models/trainConductor.model";
import {TrainVipModel} from "../models/trainVip.model";

@Injectable({
  providedIn: 'root'
})
export class TrainService {

  private readonly _serviceUrl: string = environment.apiBaseUrl;
  private readonly _httpClient: HttpClient = inject(HttpClient);


  getConductors(): Observable<TrainConductorModel[]> {
    return this._httpClient.get<TrainConductorModel[]>(this._serviceUrl + 'trains/conductor');
  }

  updateConductor(playerId: string, conductor: TrainConductorModel) : Observable<TrainConductorModel> {
    return this._httpClient.put<TrainConductorModel>(this._serviceUrl + 'trains/conductor/' + playerId, conductor);
  }

  updateVip(playerId: string, conductor: TrainVipModel) : Observable<TrainVipModel> {
    return this._httpClient.put<TrainVipModel>(this._serviceUrl + 'trains/vip/' + playerId, conductor);
  }

  getVips(): Observable<TrainVipModel[]> {
    return this._httpClient.get<TrainVipModel[]>(this._serviceUrl + 'trains/vip');
  }

  createPlayer(playerName: string): Observable<PlayerModel> {
    return this._httpClient.post<PlayerModel>(this._serviceUrl + 'players', {playerName});
  }

  updatePlayer(playerId: string, player: PlayerModel): Observable<PlayerModel> {
    return this._httpClient.put<PlayerModel>(this._serviceUrl + 'players/update/' + playerId, player);
  }

  deletePlayer(playerId: string) {
    return this._httpClient.delete(this._serviceUrl + 'players/' + playerId);
  }

  getPlayers(): Observable<PlayerModel[]> {
    return this._httpClient.get<PlayerModel[]>(this._serviceUrl + "players");
  }

  registerPlayer(playerId: string): Observable<boolean> {
    return this._httpClient.put<boolean>(this._serviceUrl + "players/" + playerId, {});
  }

  getCurrentWeek(): Observable<WeeklyScheduleModel> {
    return this._httpClient.get<WeeklyScheduleModel>(this._serviceUrl + "schedules/current-week");
  }

  getNextWeek(date: string): Observable<WeeklyScheduleModel> {
    return this._httpClient.get<WeeklyScheduleModel>(this._serviceUrl + `schedules/next-week?date=${date}`);
  }

  getWeeks(page: number, pageSize: number, playerName?: string, year?: number): Observable<WeeklyScheduleModel[]> {
    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);


    if (playerName) params = params.set('playerName', playerName);
    if (year) params = params.set('year', year.toString());

    return this._httpClient.get<WeeklyScheduleModel[]>(this._serviceUrl + 'schedules/history', { params });
  }

}
