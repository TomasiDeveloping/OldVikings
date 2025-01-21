import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'weekDay'
})
export class WeekDayPipe implements PipeTransform {

  transform(value: number): string {
    switch (value) {
      case 0: return 'Alliances.TrainConductor.Sunday';
      case 1: return 'Alliances.TrainConductor.Monday';
      case 2: return 'Alliances.TrainConductor.Tuesday';
      case 3: return 'Alliances.TrainConductor.Wednesday';
      case 4: return 'Alliances.TrainConductor.Thursday';
      case 5: return 'Alliances.TrainConductor.Friday';
      case 6: return 'Alliances.TrainConductor.Saturday';
      default: return value.toString();
    }
  }

}
