import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {TrainGuideModel} from "../models/trainGuide.model";

@Injectable({
  providedIn: 'root'
})
export class TrainGuideService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + 'trainGuides';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  public getTrainGuide(): Observable<TrainGuideModel> {
    return this._httpClient.get<TrainGuideModel>(this._serviceUrl);
  }
}
