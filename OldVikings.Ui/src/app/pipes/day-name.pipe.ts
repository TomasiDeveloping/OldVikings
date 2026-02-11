import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dayName'
})
export class DayNamePipe implements PipeTransform {

  transform(value: string | null): string {
    if (value === null) return '';
    switch (value.toLowerCase()) {
      case 'monday': return 'Train.WeekPlan.Day.Monday';
      case 'tuesday': return 'Train.WeekPlan.Day.Tuesday';
      case 'wednesday': return 'Train.WeekPlan.Day.Wednesday';
      case 'thursday': return 'Train.WeekPlan.Day.Thursday';
      case 'friday': return 'Train.WeekPlan.Day.Friday';
      case 'saturday': return 'Train.WeekPlan.Day.Saturday';
      case 'sunday': return 'Train.WeekPlan.Day.Sunday';
      default: return '';
    }
  }

}
