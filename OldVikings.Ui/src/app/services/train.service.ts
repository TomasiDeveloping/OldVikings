import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {PlayerModel} from "../models/player.model";
import {WeeklyScheduleModel} from "../models/weeklySchedule.model";

@Injectable({
  providedIn: 'root'
})
export class TrainService {

  private readonly _serviceUrl: string = environment.apiBaseUrl;
  private readonly _httpClient: HttpClient = inject(HttpClient);

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
