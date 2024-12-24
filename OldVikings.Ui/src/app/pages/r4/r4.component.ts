import { Component } from '@angular/core';

@Component({
  selector: 'app-r4',
  templateUrl: './r4.component.html',
  styleUrl: './r4.component.scss'
})
export class R4Component {

  public players: {
    name: string;
    create: string;
    headquarter: number;
    vsPoints: number;
    marshal: number;
    marshalPercent: string;
    desertStorm: number;
    desertPercent: string;
  }[] = [
    { name: "Skoll Fenris", create: "21.11.2024", headquarter: 26, vsPoints: 26826475405, marshal: 16, marshalPercent: "94.12%", desertStorm: 5, desertPercent: "100.00%" },
    { name: "AskerErsen", create: "21.11.2024", headquarter: 29, vsPoints: 26025770730, marshal: 15, marshalPercent: "88.24%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Käptain Jack", create: "21.11.2024", headquarter: 29, vsPoints: 25876634575, marshal: 17, marshalPercent: "100.00%", desertStorm: 3, desertPercent: "60.00%" },
    { name: "Argosy", create: "21.11.2024", headquarter: 29, vsPoints: 22819910560, marshal: 9, marshalPercent: "52.94%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "The Don90", create: "21.11.2024", headquarter: 29, vsPoints: 22177620215, marshal: 9, marshalPercent: "52.94%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "koras22", create: "21.11.2024", headquarter: 26, vsPoints: 21196881035, marshal: 7, marshalPercent: "41.18%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Kfkx", create: "21.11.2024", headquarter: 28, vsPoints: 20431821195, marshal: 7, marshalPercent: "41.18%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Nelimator", create: "21.11.2024", headquarter: 27, vsPoints: 19375035830, marshal: 8, marshalPercent: "47.06%", desertStorm: 1, desertPercent: "20.00%" },
    { name: "socketeer", create: "21.11.2024", headquarter: 27, vsPoints: 18271051835, marshal: 15, marshalPercent: "88.24%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "andy murray", create: "21.11.2024", headquarter: 29, vsPoints: 17755635295, marshal: 13, marshalPercent: "76.47%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Ymir 71", create: "21.11.2024", headquarter: 27, vsPoints: 16670171610, marshal: 8, marshalPercent: "47.06%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Il Predestinato 16", create: "21.11.2024", headquarter: 27, vsPoints: 15552331170, marshal: 6, marshalPercent: "35.29%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Captain BlackBeard", create: "21.11.2024", headquarter: 29, vsPoints: 15157350620, marshal: 7, marshalPercent: "41.18%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Power 1988", create: "21.11.2024", headquarter: 26, vsPoints: 14908075710, marshal: 7, marshalPercent: "41.18%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Jeroenos", create: "21.11.2024", headquarter: 26, vsPoints: 14652910215, marshal: 10, marshalPercent: "58.82%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Bylli88", create: "21.11.2024", headquarter: 26, vsPoints: 13563530665, marshal: 14, marshalPercent: "82.35%", desertStorm: 3, desertPercent: "60.00%" },
    { name: "WilsOn", create: "21.11.2024", headquarter: 26, vsPoints: 12711489475, marshal: 3, marshalPercent: "17.65%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "M3ssirv3", create: "21.11.2024", headquarter: 26, vsPoints: 12620656945, marshal: 8, marshalPercent: "47.06%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Lestat76", create: "21.11.2024", headquarter: 26, vsPoints: 12549056345, marshal: 5, marshalPercent: "29.41%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Maxxlux", create: "21.11.2024", headquarter: 27, vsPoints: 12304032285, marshal: 8, marshalPercent: "47.06%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "dahäd", create: "21.11.2024", headquarter: 28, vsPoints: 12256197005, marshal: 11, marshalPercent: "64.71%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Viking king Micand", create: "21.11.2024", headquarter: 29, vsPoints: 12001568285, marshal: 11, marshalPercent: "64.71%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Fooyi", create: "21.11.2024", headquarter: 24, vsPoints: 11469016000, marshal: 9, marshalPercent: "52.94%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Commander KJ", create: "21.11.2024", headquarter: 25, vsPoints: 11174621220, marshal: 8, marshalPercent: "47.06%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "OpalSpider", create: "21.11.2024", headquarter: 21, vsPoints: 10964984615, marshal: 3, marshalPercent: "17.65%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Lozzzzz", create: "21.11.2024", headquarter: 25, vsPoints: 10946206330, marshal: 4, marshalPercent: "23.53%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Backup Jack", create: "21.11.2024", headquarter: 25, vsPoints: 10844108155, marshal: 12, marshalPercent: "70.59%", desertStorm: 5, desertPercent: "100.00%" },
    { name: "Jors102", create: "21.11.2024", headquarter: 28, vsPoints: 10798307180, marshal: 9, marshalPercent: "52.94%", desertStorm: 3, desertPercent: "60.00%" },
    { name: "Tom0593", create: "21.11.2024", headquarter: 22, vsPoints: 10710629425, marshal: 3, marshalPercent: "17.65%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Buuuuuuuuu", create: "21.11.2024", headquarter: 29, vsPoints: 10206093480, marshal: 11, marshalPercent: "64.71%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Chimakun", create: "21.11.2024", headquarter: 25, vsPoints: 10188738435, marshal: 11, marshalPercent: "64.71%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Jhonny08", create: "21.11.2024", headquarter: 27, vsPoints: 10165433985, marshal: 6, marshalPercent: "35.29%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "60TUHOK", create: "21.11.2024", headquarter: 28, vsPoints: 10050515685, marshal: 8, marshalPercent: "47.06%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Oerwoud", create: "21.11.2024", headquarter: 25, vsPoints: 9870327160, marshal: 12, marshalPercent: "70.59%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Gour the eternal", create: "21.11.2024", headquarter: 27, vsPoints: 9815628130, marshal: 7, marshalPercent: "41.18%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Ben123321", create: "21.11.2024", headquarter: 26, vsPoints: 9625301400, marshal: 4, marshalPercent: "23.53%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Dissolver", create: "21.11.2024", headquarter: 27, vsPoints: 9544472350, marshal: 9, marshalPercent: "52.94%", desertStorm: 2, desertPercent: "40.00%" },
    { name: "Kaliisi02", create: "21.11.2024", headquarter: 25, vsPoints: 9240315490, marshal: 6, marshalPercent: "35.29%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "眀仔", create: "21.11.2024", headquarter: 28, vsPoints: 8806768740, marshal: 4, marshalPercent: "23.53%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Torulf", create: "21.11.2024", headquarter: 26, vsPoints: 8692672985, marshal: 14, marshalPercent: "82.35%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Mr Twister", create: "21.11.2024", headquarter: 24, vsPoints: 8576332040, marshal: 11, marshalPercent: "64.71%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Ttiant", create: "21.11.2024", headquarter: 27, vsPoints: 8009471545, marshal: 8, marshalPercent: "47.06%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "breker", create: "21.11.2024", headquarter: 26, vsPoints: 7691977675, marshal: 14, marshalPercent: "82.35%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Nadav don", create: "21.11.2024", headquarter: 28, vsPoints: 7428034910, marshal: 3, marshalPercent: "17.65%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Atg0000", create: "21.11.2024", headquarter: 27, vsPoints: 7353611460, marshal: 13, marshalPercent: "76.47%", desertStorm: 4, desertPercent: "80.00%" },
    { name: "okok192", create: "21.11.2024", headquarter: 27, vsPoints: 7273992980, marshal: 4, marshalPercent: "23.53%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "marsal", create: "21.11.2024", headquarter: 28, vsPoints: 7145046110, marshal: 6, marshalPercent: "35.29%", desertStorm: 1, desertPercent: "20.00%" },
    { name: "Jetlife Headquaters", create: "21.11.2024", headquarter: 23, vsPoints: 6634878660, marshal: 13, marshalPercent: "76.47%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Flak Donbass", create: "21.11.2024", headquarter: 24, vsPoints: 6423749795, marshal: 8, marshalPercent: "47.06%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Calvin Lau", create: "21.11.2024", headquarter: 25, vsPoints: 6323069165, marshal: 8, marshalPercent: "47.06%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Dommyynator", create: "21.11.2024", headquarter: 25, vsPoints: 6229184880, marshal: 3, marshalPercent: "17.65%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "ferdi29", create: "21.11.2024", headquarter: 25, vsPoints: 5822048395, marshal: 7, marshalPercent: "41.18%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Arrow24", create: "21.11.2024", headquarter: 27, vsPoints: 5573357350, marshal: 3, marshalPercent: "17.65%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Janlovesoem", create: "21.11.2024", headquarter: 28, vsPoints: 5482497960, marshal: 6, marshalPercent: "35.29%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "GK1212", create: "21.11.2024", headquarter: 25, vsPoints: 5436033305, marshal: 6, marshalPercent: "35.29%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Volokasay", create: "21.11.2024", headquarter: 28, vsPoints: 5278416105, marshal: 5, marshalPercent: "29.41%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "cloudlena", create: "21.11.2024", headquarter: 24, vsPoints: 5085526540, marshal: 8, marshalPercent: "47.06%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Ikiop", create: "21.11.2024", headquarter: 23, vsPoints: 4940694530, marshal: 7, marshalPercent: "41.18%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Cat7", create: "21.11.2024", headquarter: 24, vsPoints: 4721242380, marshal: 11, marshalPercent: "64.71%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "wolf 313", create: "21.11.2024", headquarter: 27, vsPoints: 4316073390, marshal: 3, marshalPercent: "17.65%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Toyboy1", create: "21.11.2024", headquarter: 26, vsPoints: 4172799770, marshal: 10, marshalPercent: "58.82%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Rowan 88", create: "21.11.2024", headquarter: 25, vsPoints: 3806394945, marshal: 3, marshalPercent: "17.65%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Digga1985", create: "21.11.2024", headquarter: 25, vsPoints: 3678752995, marshal: 6, marshalPercent: "35.29%", desertStorm: 1, desertPercent: "20.00%" },
    { name: "Mirac5561", create: "21.11.2024", headquarter: 26, vsPoints: 3628462830, marshal: 2, marshalPercent: "11.76%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "cuchlain", create: "21.11.2024", headquarter: 25, vsPoints: 3479403685, marshal: 3, marshalPercent: "17.65%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Reece95", create: "21.11.2024", headquarter: 28, vsPoints: 3417545020, marshal: 8, marshalPercent: "47.06%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Gruoninga", create: "21.11.2024", headquarter: 23, vsPoints: 3240614035, marshal: 8, marshalPercent: "47.06%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "olimopa", create: "21.11.2024", headquarter: 26, vsPoints: 3187363575, marshal: 3, marshalPercent: "17.65%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "KeisariFIN", create: "21.11.2024", headquarter: 25, vsPoints: 2993513340, marshal: 6, marshalPercent: "35.29%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Mago26", create: "21.11.2024", headquarter: 24, vsPoints: 2977560370, marshal: 5, marshalPercent: "29.41%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "theDash", create: "21.11.2024", headquarter: 24, vsPoints: 2318689670, marshal: 5, marshalPercent: "29.41%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Kobus777", create: "21.11.2024", headquarter: 23, vsPoints: 2255353705, marshal: 6, marshalPercent: "35.29%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Sug kuk", create: "21.11.2024", headquarter: 24, vsPoints: 1754896825, marshal: 6, marshalPercent: "35.29%", desertStorm: 0, desertPercent: "0.00%" },
  ];


  getRowClass(index: number): string {

    if (index < 40) {
      return 'custom-success';
    } else if (index < 60) {
      return 'custom-warning';
    } else {
      return 'custom-danger';
    }
  }



}
