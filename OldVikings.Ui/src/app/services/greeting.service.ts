import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {GreetingModel} from "../models/greeting.model";

@Injectable({
  providedIn: 'root'
})
export class GreetingService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + 'greetings';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  public getGreetings(): Observable<GreetingModel[]> {
    return this._httpClient.get<GreetingModel[]>(this._serviceUrl);
  }

  public insertGreeting(greeting: GreetingModel): Observable<GreetingModel> {
    return this._httpClient.post<GreetingModel>(this._serviceUrl, greeting);
  }
}
