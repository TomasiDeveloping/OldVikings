import { HttpInterceptorFn } from '@angular/common/http';
import {AuthenticationService} from "../services/authentication.service";
import {inject} from "@angular/core";

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authService: AuthenticationService = inject(AuthenticationService);

  const token = authService.getR4AccessToken();

  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    })
  }
  return next(req);
};
