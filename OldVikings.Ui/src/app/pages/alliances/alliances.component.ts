import {Component, inject, OnInit} from '@angular/core';
import {TrainGuideService} from "../../services/train-guide.service";
import {TrainGuideModel} from "../../models/trainGuide.model";
import {TranslateService} from "@ngx-translate/core";

@Component({
  selector: 'app-alliances',
  templateUrl: './alliances.component.html',
  styleUrl: './alliances.component.scss'
})
export class AlliancesComponent implements OnInit {

  private readonly _trainService: TrainGuideService = inject(TrainGuideService);
  private readonly _translateService: TranslateService = inject(TranslateService);

  public trainGuide: TrainGuideModel | null = null;
  public today = new Date();
  public tomorrow = new Date(new Date().setDate(new Date().getDate() + 1));
  public dayAfterTomorrow = new Date(new Date().setDate(new Date().getDate() + 2));

  ngOnInit() {
    this.getTrainGuide();
    console.log(this.today.getDay());
  }

  getTrainGuide() {
    this._trainService.getTrainGuide().subscribe({
      next: ((response) => {
        if (response) {
          this.trainGuide = response;
          if (this.trainGuide.currentPlayer === 'MVP') {
            this.trainGuide.currentPlayer = this._translateService.instant('PlayerMvp.Title')
          }
          if (this.trainGuide.nextPlayer === 'MVP') {
            this.trainGuide.nextPlayer = this._translateService.instant('PlayerMvp.Title')
          }
        }
      }),
      error: (error) => {
        console.log(error);
      }
    });
  }

}
