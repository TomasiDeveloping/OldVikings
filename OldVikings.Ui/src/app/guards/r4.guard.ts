import {CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";
import {AuthenticationService} from "../services/authentication.service";
import {map} from "rxjs";

export const r4Guard: CanActivateFn = () => {
  const router: Router = inject(Router);
  const authService: AuthenticationService = inject(AuthenticationService);

  return authService.isR4LoggedIn$.pipe(
    map(isLoggedIn => {
      if (!isLoggedIn) {
        router.navigate(['/r4-login']).then();
      }
      return isLoggedIn;
    })
  )

};
