import { Component, inject, OnInit } from '@angular/core';
import { WeeklyScheduleModel } from "../../../models/weeklySchedule.model";
import { TrainService } from "../../../services/train.service";
import { debounceTime, distinctUntilChanged, Subject } from "rxjs";

@Component({
  selector: 'app-train-history',
  templateUrl: './train-history.component.html',
  styleUrls: ['./train-history.component.scss']
})
export class TrainHistoryComponent implements OnInit {

  weeks: WeeklyScheduleModel[] = [];
  searchPlayer: string = '';
  searchPlayerChanged: Subject<string> = new Subject<string>();
  selectedYear?: number;
  years: number[] = [];
  openWeeks = new Set<string>();

  page = 1;
  pageSize = 10;
  hasMore = true;
  isLoading = false;

  private readonly _trainService: TrainService = inject(TrainService);

  ngOnInit() {
    const currentYear = new Date().getFullYear();
    this.years = Array.from({ length: 2 }, (_, i) => currentYear - i);
    this.selectedYear = currentYear;

    this.loadWeeks(true);

    this.searchPlayerChanged.pipe(
      debounceTime(700),
      distinctUntilChanged()
    ).subscribe(value => {
      if ((value?.length ?? 0) < 2 && value.length !== 0) return;
      this.loadWeeks(true);
    });
  }

  loadWeeks(reset: boolean = false) {
    if (this.isLoading) return;

    if (reset) {
      this.weeks = [];
      this.page = 1;
      this.hasMore = true;
      this.openWeeks.clear();
    }

    this.isLoading = true;
    this._trainService.getWeeks(this.page, this.pageSize, this.searchPlayer, this.selectedYear).subscribe({
      next: (data) => {
        if (data.length < this.pageSize) this.hasMore = false;
        this.weeks = [...this.weeks, ...data];

        if (this.searchPlayer?.trim()) {
          data.forEach(week => {
            const found = week.days?.some(day =>
              (day.leaderPlayer ?? '').toLowerCase().includes(this.searchPlayer.toLowerCase()) ||
              (day.vipPlayer ?? '').toLowerCase().includes(this.searchPlayer.toLowerCase())
            ) ?? false;

            if (found) {
              this.openWeeks.add(week.id);

              setTimeout(() => {
                const element = document.getElementById('week-' + week.id);
                element?.scrollIntoView({behavior: 'smooth', block: 'start'});

                const pills = element?.querySelectorAll('.pill.highlight') ?? [];
                pills.forEach(p => {
                  p.classList.add('animate');
                  setTimeout(() => p.classList.remove('animate'), 500);
                });
              }, 150);
            }
          });
        }

        this.page++;
        this.isLoading = false;
      }, error: err => {
        console.log(err);
        this.isLoading = false;
      }
    });
  }

  toggleWeek(week: WeeklyScheduleModel) {
    if (this.openWeeks.has(week.id)) this.openWeeks.delete(week.id);
    else this.openWeeks.add(week.id);
  }

  isOpen(week: WeeklyScheduleModel): boolean {
    return this.openWeeks.has(week.id);
  }

  onSearchChange() {
    this.searchPlayerChanged.next(this.searchPlayer);
  }

  onYearChange() {
    this.loadWeeks(true);
  }
}
