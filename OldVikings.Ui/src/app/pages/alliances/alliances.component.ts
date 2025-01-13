import {Component, inject, OnInit} from '@angular/core';
import {TrainGuideService} from "../../services/train-guide.service";
import {TrainGuideModel} from "../../models/trainGuide.model";

@Component({
  selector: 'app-alliances',
  templateUrl: './alliances.component.html',
  styleUrl: './alliances.component.scss'
})
export class AlliancesComponent implements OnInit {

  private readonly _trainService: TrainGuideService = inject(TrainGuideService);

  public trainGuide: TrainGuideModel | null = null;

  ngOnInit() {
    this.getTrainGuide();
  }

  getTrainGuide() {
    this._trainService.getTrainGuide().subscribe({
      next: ((response) => {
        if (response) {
          this.trainGuide = response;
        }
      }),
      error: (error) => {
        console.log(error);
      }
    });
  }

}
