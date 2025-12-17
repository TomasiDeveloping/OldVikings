import {Component, ElementRef, inject, OnInit, ViewChild} from '@angular/core';
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
  @ViewChild('player') player!: ElementRef<HTMLAudioElement>;
  isPlaying = false;

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

  togglePlay() {
    const audio = this.player.nativeElement;

    if (audio.paused) {
      audio.play();
      this.isPlaying = true;
    } else {
      audio.pause();
      this.isPlaying = false;
    }
  }
}
