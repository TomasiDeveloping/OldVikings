import {inject, Injectable} from '@angular/core';
import {NgxSpinnerService} from "ngx-spinner";

@Injectable({
  providedIn: 'root'
})
export class SpinnerService {

  private readonly _ngxSpinnerService: NgxSpinnerService = inject(NgxSpinnerService);

  private busyRequestCount: number = 0;

  busy() {
    this.busyRequestCount++;
    this._ngxSpinnerService.show(undefined, {
      type: 'ball-8bits',
      bdColor: 'rgba(0, 0, 0, 0.8)',
      color: '#fd8d04'
    }).then();
  }

  idle(): void {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this._ngxSpinnerService.hide().then();
    }
  }
}
