import {Component, inject} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {GreetingModel} from "../../../models/greeting.model";
import {GreetingService} from "../../../services/greeting.service";
import {HttpErrorResponse} from "@angular/common/http";
import {ToastrService} from "ngx-toastr";
import {TranslateService} from "@ngx-translate/core";

@Component({
  selector: 'app-greeting-modal',
  templateUrl: './greeting-modal.component.html',
  styleUrl: './greeting-modal.component.scss'
})
export class GreetingModalComponent {

  private readonly _greetingService: GreetingService = inject(GreetingService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _translate: TranslateService = inject(TranslateService);

  public greetingForm: FormGroup = new FormGroup({
    serverNumber: new FormControl<number | null>(null, [Validators.required]),
    allianceName: new FormControl<string>('', [Validators.required]),
    playerName: new FormControl<string>(''),
    comment: new FormControl<string>('', [Validators.maxLength(200)])
  });

  get f() {
    return this.greetingForm.controls;
  }

  onSubmit(): void {
    if (this.greetingForm.invalid) {
      return;
    }

    const greeting: GreetingModel = this.greetingForm.value;

    this.greetingForm.reset();
    const element = document.getElementById('close-modal')!;
    element.click();

    this._greetingService.insertGreeting(greeting).subscribe({
      next: (response => {
        if (response) {
          this._toastr.success(this._translate.instant('Greeting.Modal.Toastr.Success'), this._translate.instant('Greeting.Modal.Toastr.Label'));
        }
      }),
      error: (error: HttpErrorResponse) => {
        this._toastr.error(this._translate.instant('Greeting.Modal.Toastr.Error'), this._translate.instant('Greeting.Modal.Toastr.Success'));
        console.error(error);
      }
    });
  }

  onCancel() {
    this.greetingForm.reset();
  }
}
