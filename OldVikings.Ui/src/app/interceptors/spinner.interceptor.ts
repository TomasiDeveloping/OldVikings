import { HttpInterceptorFn } from '@angular/common/http';
import {inject} from "@angular/core";
import {SpinnerService} from "../services/spinner.service";
import {finalize} from "rxjs";

export const spinnerInterceptor: HttpInterceptorFn = (req, next) => {

  if (req.url.includes('Translation')) {
    return next(req);
  }
  const spinnerService = inject(SpinnerService);

  spinnerService.busy();

  return next(req).pipe(
    finalize(() => {
      spinnerService.idle();
    })
  )
};