import {Component, inject} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {MemberService} from "../../services/member.service";
import {ToastrService} from "ngx-toastr";
import {Router} from "@angular/router";
import {TranslateService} from "@ngx-translate/core";

@Component({
  selector: 'app-member-login',
  templateUrl: './member-login.component.html',
  styleUrl: './member-login.component.scss'
})
export class MemberLoginComponent {

  private readonly _memberService: MemberService = inject(MemberService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _router: Router = inject(Router);
  private readonly _translate: TranslateService = inject(TranslateService);


  public loginForm: FormGroup = new FormGroup({
    password: new FormControl<string>('', [Validators.required]),
  });

  get f() {
    return this.loginForm.controls;
  }

  public onLogin(): void {
    if (this.loginForm.invalid) {
      return;
    }
    const password = this.f['password'].value;

    if (password === this._memberService.currentKey) {
      this._memberService.setKey(password);
      const redirectUrl: string = this._memberService.redirectUrl ?? '/home';

      const url: URL = new URL(window.location.origin + redirectUrl);
      const path: string = url.pathname;
      const queryParams: any = {};
      url.searchParams.forEach((value: string, key: string): void => {
        queryParams[key] = value;
      });
      const fragment: string | undefined = url.hash ? url.hash.substring(1) : undefined;

      this._router.navigate([path], { queryParams, fragment }).then();
    } else {
      this._toastr.error(this._translate.instant('MemberLogin.Toastr.Error'), this._translate.instant('MemberLogin.Toastr.Title'));
    }
  }

}
