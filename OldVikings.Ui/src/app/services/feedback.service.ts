import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {FeedbackModel} from "../models/feedback.model";
import {FeedbackStatus, FeedbackStatusCode} from "../helpers/feedbackStatus";
import {FeedbackHistoryModel} from "../models/feedbackHistory.model";
import {CreateFeedbackModel} from "../models/createFeedback.model";

@Injectable({
  providedIn: 'root'
})
export class FeedbackService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + 'feedbacks';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getFeedbacksByStatus(statuses: FeedbackStatus[]): Observable<FeedbackModel[]> {
    let params = new HttpParams();

    statuses.forEach(s => {
      const code = FeedbackStatusCode[s];
      params = params.append('status', code.toString());
    })
    return this._httpClient.get<FeedbackModel[]>(this._serviceUrl, {params: params});
  }

  getFeedback(id: string): Observable<FeedbackModel> {
    return this._httpClient.get<FeedbackModel>(this._serviceUrl + '/' + id);
  }

  getHistory(id: string): Observable<FeedbackHistoryModel[]> {
    return this._httpClient.get<FeedbackHistoryModel[]>(this._serviceUrl + '/' + id + '/history');
  }

  vote(id: string): Observable<boolean> {
    return this._httpClient.put<boolean>(this._serviceUrl + '/' + id + '/vote', {});
  }

  createFeedback(feedback: CreateFeedbackModel): Observable<FeedbackModel> {
    return this._httpClient.post<FeedbackModel>(this._serviceUrl, feedback);
  }
}
