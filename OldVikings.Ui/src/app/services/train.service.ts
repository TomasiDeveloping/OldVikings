import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
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

}
