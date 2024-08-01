import {AfterViewChecked, Component, ElementRef, ViewChild} from '@angular/core';
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-vs-duell',
  templateUrl: './vs-duell.component.html',
  styleUrl: './vs-duell.component.scss'
})
export class VsDuellComponent implements AfterViewChecked {

  @ViewChild('day1') day1!: ElementRef;
  @ViewChild('day2') day2!: ElementRef;
  @ViewChild('day3') day3!: ElementRef;
  @ViewChild('day4') day4!: ElementRef;
  @ViewChild('day5') day5!: ElementRef;
  @ViewChild('day6') day6!: ElementRef;

  private openDay: null | undefined;

  constructor(private _route: ActivatedRoute) {
    // Subscribe to route query params
    this._route.queryParams.subscribe(params => {
      this.openDay = params['openDay'];
    });
  }

  ngAfterViewChecked() {
    if (this.openDay) {
      this.openAccordion(this.openDay);
      this.openDay = null; // Clear to avoid repeated execution
    }
  }

  private openAccordion(dayId: string) {
    const accordionMap = {
      day1: this.day1,
      day2: this.day2,
      day3: this.day3,
      day4: this.day4,
      day5: this.day5,
      day6: this.day6
    };

    // @ts-ignore
    const elementRef = accordionMap[dayId];
    if (elementRef && elementRef.nativeElement) {
      const element = elementRef.nativeElement;
      const bsCollapse = new (window as any).bootstrap.Collapse(element, {
        toggle: true
      });
    }
  }
}
