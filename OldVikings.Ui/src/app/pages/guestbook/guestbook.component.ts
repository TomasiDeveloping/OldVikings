import {Component, inject, OnInit} from '@angular/core';
import {GreetingService} from "../../services/greeting.service";
import {GreetingModel} from "../../models/greeting.model";
import {TranslateService} from "@ngx-translate/core";
import {DeeplTranslateService} from "../../services/deeplTranslate.service";
import {TranslationRequestModel} from "../../models/translationRequest.model";
import {ToastrService} from "ngx-toastr";
import {DomSanitizer, SafeHtml} from "@angular/platform-browser";

@Component({
  selector: 'app-guestbook',
  templateUrl: './guestbook.component.html',
  styleUrl: './guestbook.component.scss'
})
export class GuestbookComponent implements OnInit{

  private readonly _greetingService: GreetingService = inject(GreetingService);
  private readonly _translateService: TranslateService = inject(TranslateService);
  private readonly _deeplTranslateService: DeeplTranslateService = inject(DeeplTranslateService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _sanitizer: DomSanitizer = inject(DomSanitizer);

  public greetings: GreetingModel[] = [];
  public isLoading: boolean = false;
  public page: number = 1;

  ngOnInit() {
    this.getGreetings();
  }

  getGreetings(){
    this._greetingService.getGreetings().subscribe({
      next: ((response => {
        if (response) {
          this.greetings = response;
        }
      }))
    });
  }

  onTranslate(greeting: GreetingModel) {
    this.isLoading = true;
    const request: TranslationRequestModel = {
      text: greeting.comment,
      language: this._translateService.currentLang
    };
    this._deeplTranslateService.translateText(request).subscribe({
      next: ((response) => {
        if (response) {
          const greetingToFind: GreetingModel | undefined = this.greetings.find(x => x.id === greeting?.id);
          if (greetingToFind) {
            greetingToFind.comment = response.translatedText;
          }
          this.isLoading = false;
        }
      }),
      error: error => {
        this.isLoading = false;
        this._toastr.error('Der Dienst funktioniert aktuell nicht');
      }
    })
  }

  onPageChange(event: number) {
    this.page = event;
  }

  getSafeHtml(comment: string) : SafeHtml {
    return this._sanitizer.bypassSecurityTrustHtml(comment);
  }
}
