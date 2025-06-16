import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {R4RoleModel} from "../models/r4Role.model";

@Injectable({
  providedIn: 'root'
})
export class R4RoleService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + 'r4Roles';
  private readonly _httpClient: HttpClient = inject(HttpClient);


  getR4Roles(): Observable<R4RoleModel[]> {
    return this._httpClient.get<R4RoleModel[]>(this._serviceUrl);
  }

}
