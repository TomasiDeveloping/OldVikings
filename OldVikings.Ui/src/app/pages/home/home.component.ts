import {Component, inject, OnInit} from '@angular/core';
import {TranslateService} from "@ngx-translate/core";
import {VsDayModel} from "../../models/vsDay.model";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit{

  private readonly _translate: TranslateService = inject(TranslateService);

  public vsDay!: VsDayModel;

  ngOnInit() {
    const currentDay: Date = new Date();
    this.vsDay = this.getCurrentVsDay(currentDay.getDay());
  }

  getCurrentVsDay(currentDay: number): VsDayModel {
    const day: VsDayModel = {link: '', description: ''};
    if (currentDay === 0) {
      day.link = '';
      day.description = '';
      } else {
      day.description = this._translate.instant(`Home.Vs.Day${currentDay}.Description`);
      day.link = this._translate.instant(`Home.Vs.Day${currentDay}.Link`);
    }
    return day;
  }
}
