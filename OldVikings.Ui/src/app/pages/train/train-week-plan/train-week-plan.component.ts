import {Component, Input} from '@angular/core';
import {WeeklyScheduleModel} from "../../../models/weeklySchedule.model";

@Component({
  selector: 'app-train-week-plan',
  templateUrl: './train-week-plan.component.html',
  styleUrl: './train-week-plan.component.scss'
})
export class TrainWeekPlanComponent {
  @Input({required:true}) weekPlan!: WeeklyScheduleModel;
}
