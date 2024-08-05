import {CanActivateFn, Router, RouterStateSnapshot} from '@angular/router';
import {MemberService} from "../services/member.service";
import {inject} from "@angular/core";
import {ToastrService} from "ngx-toastr";
import {TranslateService} from "@ngx-translate/core";

export const memberGuard: CanActivateFn = (_ , state: RouterStateSnapshot): boolean => {

  const memberService: MemberService = inject(MemberService);
  const toastr: ToastrService = inject(ToastrService);
  const router: Router = inject(Router);
  const translate: TranslateService = inject(TranslateService);

  const key: string | null = memberService.getKey();
  const redirectUrl: string = state.url;

  if (key) {
    if (key === memberService.currentKey) {
      return true;
    } else {
      toastr.warning(translate.instant('MemberLogin.PasswordExpired.Information'), translate.instant('MemberLogin.PasswordExpired.Title'));
      memberService.setRedirectUri(redirectUrl);
      router.navigate(['/member-login']).then();
      return false;
    }
  } else {
    memberService.setRedirectUri(redirectUrl);
    router.navigate(['/member-login']).then();
    return false;
  }
};
