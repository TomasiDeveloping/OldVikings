import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'counterCharacters'
})
export class CounterCharactersPipe implements PipeTransform {

  transform(value: string): number {
    if (value) {
      return value.length
    }

    return 0;
  }

}
