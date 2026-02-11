import {WeeklyScheduleDayModel} from "./weeklyScheduleDay.model";

export interface WeeklyScheduleModel {
  id: string;
  weekStartDate: string;
  createdAt: Date,
  days: WeeklyScheduleDayModel[];
}
