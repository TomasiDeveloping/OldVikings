import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import {TranslationRequestModel} from "../models/translationRequest.model";
import {Observable} from "rxjs";
import {TranslationResponseModel} from "../models/translationResponse.model";

@Injectable({
  providedIn: 'root'
})
export class DeeplTranslateService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + 'Translation';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  translateText(translateRequest: TranslationRequestModel): Observable<TranslationResponseModel> {
    return this._httpClient.post<TranslationResponseModel>(this._serviceUrl, translateRequest);
  }
}
