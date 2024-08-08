import {Component, inject, OnInit} from '@angular/core';
import {TranslateService} from "@ngx-translate/core";
import { ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.scss'
})
export class NavigationComponent implements OnInit{
  version: string = '1.6.0';
  isShown: boolean = false;
  siteLanguage: string = 'Deutsch';
  languageList = [
    {code: 'de', label: 'Deutsch'},
    {code: 'en', label: 'English'},
    {code: 'fr', label: 'Français'},
    {code: 'it', label: 'Italiano'},
    {code: 'nl', label: 'Néerlandais'},
    {code: 'no', label: 'Norvégien'},
    {code: 'dk', label: 'Danois'},
    {code: 'es', label: 'Español'},
    {code: 'se', label: 'Svenska'}
  ];

  private readonly translate: TranslateService = inject(TranslateService);
  private readonly toastr: ToastrService = inject(ToastrService);


  ngOnInit() {
    this.translate.addLangs(['de', 'en', 'fr', 'it','nl', 'no', 'dk', 'es', 'se']);
    let userLanguage = this.translate.getBrowserLang();
    if (!userLanguage || !this.languageList.some(x => x.code === userLanguage)) {
      if (userLanguage) {
        this.toastr.info(`Your language “${userLanguage}” is unfortunately not yet supported`, 'Language Support');
      }
      userLanguage = 'en';
    }
    this.translate.setDefaultLang(userLanguage);
  }

  changeSiteLanguage(code: string) {
    this.isShown = false;
    this.translate.use(code);
    this.siteLanguage = this.languageList.find((x) => x.code === code)?.label!;
  }
}
