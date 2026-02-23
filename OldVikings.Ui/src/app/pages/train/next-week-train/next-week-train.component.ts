import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {WeeklyScheduleModel} from "../../../models/weeklySchedule.model";
import {TrainService} from "../../../services/train.service";
import {TranslateService} from "@ngx-translate/core";
import {ToastrService} from "ngx-toastr";

type ZurichParts = {
  weekday: string;
  hour: number;
  minute: number;
  second: number;
  year: number;
  month: number;
  day: number;
};

@Component({
  selector: 'app-next-week-train',
  templateUrl: './next-week-train.component.html',
  styleUrl: './next-week-train.component.scss'
})


export class NextWeekTrainComponent implements OnInit, OnDestroy {

  state: 'HIDE' | 'COUNTDOWN' | 'SHOW' = 'HIDE';
  countdownText = '';
  loadCurrentWeekError: boolean = false;
  private timerId: any;

  private readonly TZ = 'Europe/Zurich';
  private readonly _trainService: TrainService = inject(TrainService);
  private readonly _toaster: ToastrService = inject(ToastrService);
  private readonly _translate: TranslateService = inject(TranslateService);

  weekPlan: WeeklyScheduleModel | null = null;

  ngOnInit() {
    this.tick();
  }

  ngOnDestroy() {
    this.stopTimer();
  }

  getNextWeek(dateString: string) {
    this._trainService.getNextWeek(dateString).subscribe({
      next: (response) => {
        if (response) {
          this.weekPlan = response;
        } else {
          this.weekPlan = null;
        }
      },
      error: (error) => {
        this.weekPlan = null;
        console.log(error);
        this.loadCurrentWeekError = true;
        const errorTitle = this._translate.instant('Train.Toaster.WeekPlan.Error.Title');
        const errorText = this._translate.instant('Train.Toaster.WeekPlan.Error.Description');
        this._toaster.error(errorText, errorTitle);
      }
    })
  }

  private startTimer(): void {
    if (this.timerId != null) return;
    this.timerId = setInterval(() => this.tick(), 1000);
  }

  private stopTimer(): void {
    if (this.timerId == null) return;
    clearInterval(this.timerId);
    this.timerId = null;
  }

  private tick(): void {
    const now = new Date();
    const parts = this.getZurichParts(now);

    const isSunday = parts.weekday === 'Sun';
    const minutesNow = parts.hour * 60 + parts.minute;
    const minutesTarget = 15 * 60 + 15; // 15:15

    if (!isSunday) {
      this.state = 'HIDE';
      this.countdownText = '';
      this.stopTimer();
      return;
    }

    if (minutesNow >= minutesTarget) {
      this.state = 'SHOW';
      this.countdownText = '';
      const dateString = `${parts.year}-${parts.month}-${parts.day}`;
      this.getNextWeek(dateString);
      this.stopTimer();
      return;
    }

    this.state = 'COUNTDOWN';
    const targetUtc = this.getUtcDateForZurichLocal(parts.year, parts.month, parts.day, 15, 15, 0);
    const diffMs = targetUtc.getTime() - now.getTime();

    if (diffMs <= 0) {
      this.state = 'SHOW';
      this.countdownText = '';
      this.stopTimer();
      return;
    }

    this.countdownText = this.formatDuration(diffMs);
    this.startTimer();
  }

  private getZurichParts(date: Date): ZurichParts {
    const dtf = new Intl.DateTimeFormat('en-US', {
      timeZone: this.TZ,
      weekday: 'short',
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      hour12: false
    });

    const map: any = {};
    for (const p of dtf.formatToParts(date)) {
      if (p.type !== 'literal') map[p.type] = p.value;
    }

    return {
      weekday: map.weekday,
      year: Number(map.year),
      month: Number(map.month),
      day: Number(map.day),
      hour: Number(map.hour),
      minute: Number(map.minute),
      second: Number(map.second),
    };
  }

  private getUtcDateForZurichLocal(y: number, m: number, d: number, hh: number, mm: number, ss: number): Date {
    const approxUtc = new Date(Date.UTC(y, m - 1, d, hh, mm, ss));

    const z = this.getZurichParts(approxUtc);

    const desiredMinutes = hh * 60 + mm;
    const gotMinutes = z.hour * 60 + z.minute;

    const desiredDate = `${y}-${m}-${d}`;
    const gotDate = `${z.year}-${z.month}-${z.day}`;

    let dayDiff = 0;
    if (gotDate !== desiredDate) {
      const desired = Date.UTC(y, m - 1, d);
      const got = Date.UTC(z.year, z.month - 1, z.day);
      dayDiff = Math.round((desired - got) / (24 * 60 * 60 * 1000));
    }

    const totalMinuteDiff = (desiredMinutes - gotMinutes) + dayDiff * 24 * 60;
    return new Date(approxUtc.getTime() + totalMinuteDiff * 60 * 1000);
  }

  private formatDuration(ms: number): string {
    const totalSeconds = Math.floor(ms / 1000);
    const h = Math.floor(totalSeconds / 3600);
    const m = Math.floor((totalSeconds % 3600) / 60);
    const s = totalSeconds % 60;

    const pad = (n: number) => String(n).padStart(2, '0');
    return `${pad(h)}:${pad(m)}:${pad(s)}`;
  }

}
