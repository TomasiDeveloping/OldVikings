import {Component, inject} from '@angular/core';
import {AuthenticationService} from "../../../services/authentication.service";
import {Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-r4-login',
  templateUrl: './r4-login.component.html',
  styleUrl: './r4-login.component.scss'
})
export class R4LoginComponent {
  password: string = '';
  error: boolean =  false;

  private readonly _authService: AuthenticationService = inject(AuthenticationService);
  private readonly _router: Router = inject(Router);
  private readonly _toastr: ToastrService = inject(ToastrService);

  login() {
    this.error = false;

    this._authService.r4Login(this.password).subscribe({
      next: () => {
        this._router.navigate(['/r4']).then();
      },
      error: () => {
        this._toastr.error('Login failed.', 'Login');
        this.password = '';
      }
    });
  }
}
