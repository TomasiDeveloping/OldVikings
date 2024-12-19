import { Component } from '@angular/core';

@Component({
  selector: 'app-r4',
  templateUrl: './r4.component.html',
  styleUrl: './r4.component.scss'
})
export class R4Component {

  public players: {
    name: string,
    create: string,
    headquarter: number,
    vsPoints: number,
    marshal: number,
    marshalPercent: string,
    desertStorm: number,
    desertPercent: string
  }[] = [
    { name: "AskerErsen", create: "21.11.2024", headquarter: 29, vsPoints: 15770739480, marshal: 13, marshalPercent: "86.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Skoll Fenris", create: "21.11.2024", headquarter: 26, vsPoints: 14814249300, marshal: 14, marshalPercent: "93.33%", desertStorm: 4, desertPercent: "100.00%" },
    { name: "Käptain Jack", create: "21.11.2024", headquarter: 29, vsPoints: 14494956420, marshal: 15, marshalPercent: "100.00%", desertStorm: 3, desertPercent: "75.00%" },
    { name: "Argosy", create: "21.11.2024", headquarter: 29, vsPoints: 13273070760, marshal: 7, marshalPercent: "46.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Kfkx", create: "21.11.2024", headquarter: 28, vsPoints: 12690509160, marshal: 5, marshalPercent: "33.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "koras22", create: "21.11.2024", headquarter: 26, vsPoints: 12582969960, marshal: 6, marshalPercent: "40.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "The Don90", create: "21.11.2024", headquarter: 29, vsPoints: 12473482200, marshal: 9, marshalPercent: "60.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Nelimator", create: "21.11.2024", headquarter: 26, vsPoints: 10789359720, marshal: 7, marshalPercent: "46.67%", desertStorm: 1, desertPercent: "25.00%" },
    { name: "andy murray", create: "21.11.2024", headquarter: 29, vsPoints: 10323216480, marshal: 11, marshalPercent: "73.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Il Predestinato 16", create: "21.11.2024", headquarter: 26, vsPoints: 8968130640, marshal: 5, marshalPercent: "33.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "socketeer", create: "21.11.2024", headquarter: 26, vsPoints: 8947329960, marshal: 13, marshalPercent: "86.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Ymir 71", create: "21.11.2024", headquarter: 27, vsPoints: 8936363100, marshal: 6, marshalPercent: "40.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Captain BlackBeard", create: "21.11.2024", headquarter: 29, vsPoints: 8714673780, marshal: 5, marshalPercent: "33.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "M3ssirv3", create: "21.11.2024", headquarter: 26, vsPoints: 8499875580, marshal: 7, marshalPercent: "46.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Power 1988", create: "21.11.2024", headquarter: 26, vsPoints: 8063872800, marshal: 7, marshalPercent: "46.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Jeroenos", create: "21.11.2024", headquarter: 26, vsPoints: 8025655500, marshal: 8, marshalPercent: "53.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Maxxlux", create: "21.11.2024", headquarter: 27, vsPoints: 7637583420, marshal: 7, marshalPercent: "46.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Bylli88", create: "21.11.2024", headquarter: 25, vsPoints: 7591034580, marshal: 12, marshalPercent: "80.00%", desertStorm: 2, desertPercent: "50.00%" },
    { name: "WilsOn", create: "21.11.2024", headquarter: 25, vsPoints: 6952689720, marshal: 3, marshalPercent: "20.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "dahäd", create: "21.11.2024", headquarter: 27, vsPoints: 6951159720, marshal: 10, marshalPercent: "66.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "OpalSpider", create: "21.11.2024", headquarter: 21, vsPoints: 6807717540, marshal: 2, marshalPercent: "13.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Commander KJ", create: "21.11.2024", headquarter: 25, vsPoints: 6692333460, marshal: 7, marshalPercent: "46.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Fooyi", create: "21.11.2024", headquarter: 24, vsPoints: 6641411160, marshal: 9, marshalPercent: "60.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Tom0593", create: "21.11.2024", headquarter: 22, vsPoints: 6590900160, marshal: 3, marshalPercent: "20.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Jors102", create: "21.11.2024", headquarter: 28, vsPoints: 6259893060, marshal: 7, marshalPercent: "46.67%", desertStorm: 2, desertPercent: "50.00%" },
    { name: "Lozzzzz", create: "21.11.2024", headquarter: 25, vsPoints: 6079900200, marshal: 4, marshalPercent: "26.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "60TUHOK", create: "21.11.2024", headquarter: 27, vsPoints: 6063414960, marshal: 8, marshalPercent: "53.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Viking king Micand", create: "21.11.2024", headquarter: 28, vsPoints: 5843822760, marshal: 9, marshalPercent: "60.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Chimakun", create: "21.11.2024", headquarter: 25, vsPoints: 5836088040, marshal: 10, marshalPercent: "66.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Kaliisi02", create: "21.11.2024", headquarter: 25, vsPoints: 5803095420, marshal: 6, marshalPercent: "40.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Gour the eternal", create: "21.11.2024", headquarter: 27, vsPoints: 5746491840, marshal: 5, marshalPercent: "33.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Backup Jack", create: "21.11.2024", headquarter: 25, vsPoints: 5649562860, marshal: 11, marshalPercent: "73.33%", desertStorm: 4, desertPercent: "100.00%" },
    { name: "Buuuuuuuuu", create: "21.11.2024", headquarter: 28, vsPoints: 5634212100, marshal: 9, marshalPercent: "60.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Ben123321", create: "21.11.2024", headquarter: 25, vsPoints: 5627433600, marshal: 2, marshalPercent: "13.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Lestat76", create: "21.11.2024", headquarter: 26, vsPoints: 5214032280, marshal: 5, marshalPercent: "33.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Oerwoud", create: "21.11.2024", headquarter: 24, vsPoints: 5107436460, marshal: 12, marshalPercent: "80.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Torulf", create: "21.11.2024", headquarter: 26, vsPoints: 4957555860, marshal: 12, marshalPercent: "80.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "眀仔", create: "21.11.2024", headquarter: 28, vsPoints: 4877940540, marshal: 4, marshalPercent: "26.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Jhonny08", create: "21.11.2024", headquarter: 27, vsPoints: 4853211780, marshal: 6, marshalPercent: "40.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Dissolver", create: "21.11.2024", headquarter: 26, vsPoints: 4833977940, marshal: 8, marshalPercent: "53.33%", desertStorm: 2, desertPercent: "50.00%" },
    { name: "breker", create: "21.11.2024", headquarter: 26, vsPoints: 4580447940, marshal: 12, marshalPercent: "80.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Ttiant", create: "21.11.2024", headquarter: 27, vsPoints: 4549235160, marshal: 7, marshalPercent: "46.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Mr Twister", create: "21.11.2024", headquarter: 23, vsPoints: 4500158580, marshal: 10, marshalPercent: "66.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "okok192", create: "21.11.2024", headquarter: 26, vsPoints: 4494322500, marshal: 4, marshalPercent: "26.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Nadav don", create: "21.11.2024", headquarter: 28, vsPoints: 4205405880, marshal: 3, marshalPercent: "20.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Atg0000", create: "21.11.2024", headquarter: 26, vsPoints: 4144099680, marshal: 11, marshalPercent: "73.33%", desertStorm: 3, desertPercent: "75.00%" },
    { name: "marsal", create: "21.11.2024", headquarter: 28, vsPoints: 4049812020, marshal: 6, marshalPercent: "40.00%", desertStorm: 1, desertPercent: "25.00%" },
    { name: "Jetlife Headquaters", create: "21.11.2024", headquarter: 23, vsPoints: 3812655840, marshal: 11, marshalPercent: "73.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Dommyynator", create: "21.11.2024", headquarter: 25, vsPoints: 3669410880, marshal: 3, marshalPercent: "20.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Flak Donbass", create: "21.11.2024", headquarter: 24, vsPoints: 3624381480, marshal: 6, marshalPercent: "40.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Calvin Lau", create: "21.11.2024", headquarter: 25, vsPoints: 3512671740, marshal: 8, marshalPercent: "53.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Arrow24", create: "21.11.2024", headquarter: 26, vsPoints: 3400454580, marshal: 3, marshalPercent: "20.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "ferdi29", create: "21.11.2024", headquarter: 24, vsPoints: 3254358720, marshal: 6, marshalPercent: "40.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "GK1212", create: "21.11.2024", headquarter: 25, vsPoints: 3194910720, marshal: 5, marshalPercent: "33.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Volokasay", create: "21.11.2024", headquarter: 27, vsPoints: 3021644580, marshal: 4, marshalPercent: "26.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "cloudlena", create: "21.11.2024", headquarter: 22, vsPoints: 2970686100, marshal: 8, marshalPercent: "53.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Janlovesoem", create: "21.11.2024", headquarter: 27, vsPoints: 2879612940, marshal: 6, marshalPercent: "40.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Ikiop", create: "21.11.2024", headquarter: 22, vsPoints: 2812605300, marshal: 5, marshalPercent: "33.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Rowan 88", create: "21.11.2024", headquarter: 25, vsPoints: 2686867020, marshal: 3, marshalPercent: "20.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "wolf 313", create: "21.11.2024", headquarter: 27, vsPoints: 2530883400, marshal: 3, marshalPercent: "20.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Cat7", create: "21.11.2024", headquarter: 24, vsPoints: 2419952640, marshal: 9, marshalPercent: "60.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Toyboy1", create: "21.11.2024", headquarter: 25, vsPoints: 2368053120, marshal: 9, marshalPercent: "60.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Digga1985", create: "21.11.2024", headquarter: 25, vsPoints: 2259853800, marshal: 5, marshalPercent: "33.33%", desertStorm: 1, desertPercent: "25.00%" },
    { name: "Mirac5561", create: "21.11.2024", headquarter: 25, vsPoints: 2087454300, marshal: 2, marshalPercent: "13.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Gruoninga", create: "21.11.2024", headquarter: 23, vsPoints: 1979677020, marshal: 6, marshalPercent: "40.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Reece95", create: "21.11.2024", headquarter: 28, vsPoints: 1908165600, marshal: 6, marshalPercent: "40.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "olimopa", create: "21.11.2024", headquarter: 25, vsPoints: 1906334160, marshal: 3, marshalPercent: "20.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "KeisariFIN", create: "21.11.2024", headquarter: 25, vsPoints: 1743796920, marshal: 4, marshalPercent: "26.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Mago26", create: "21.11.2024", headquarter: 24, vsPoints: 1706973360, marshal: 5, marshalPercent: "33.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "cuchlain", create: "21.11.2024", headquarter: 25, vsPoints: 1650327540, marshal: 3, marshalPercent: "20.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Kobus777", create: "21.11.2024", headquarter: 23, vsPoints: 1366529940, marshal: 4, marshalPercent: "26.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "theDash", create: "21.11.2024", headquarter: 24, vsPoints: 1322222400, marshal: 4, marshalPercent: "26.67%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Sug kuk", create: "21.11.2024", headquarter: 23, vsPoints: 1004031660, marshal: 5, marshalPercent: "33.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "bass1979", create: "26.11.2024", headquarter: 23, vsPoints: 137206116, marshal: 0, marshalPercent: "0.00%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Blosqui", create: "17.12.2024", headquarter: 30, vsPoints: 0, marshal: 2, marshalPercent: "13.33%", desertStorm: 0, desertPercent: "0.00%" },
    { name: "Saphty", create: "10.12.2024", headquarter: 27, vsPoints: 0, marshal: 0, marshalPercent: "0.00%", desertStorm: 0, desertPercent: "0.00%" }
  ]

  getRowClass(index: number): string {
    if (index < 20) {
      return 'custom-success';
    } else if (index < 39) {
      return 'custom-warning';
    } else {
      return 'custom-danger';
    }
  }
}
