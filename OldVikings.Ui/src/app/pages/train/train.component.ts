import {Component, HostListener, inject, OnInit} from '@angular/core';
import Swal from 'sweetalert2'
import {TrainService} from "../../services/train.service";
import {PlayerModel} from "../../models/player.model";
import {WeeklyScheduleModel} from "../../models/weeklySchedule.model";
import {ToastrService} from "ngx-toastr";
import {TranslateService} from "@ngx-translate/core";


@Component({
  selector: 'app-train',
  templateUrl: './train.component.html',
  styleUrl: './train.component.scss'
})
export class TrainComponent implements OnInit {

  open: boolean = false;
  query: string = '';
  selectedPlayer: PlayerModel | null = null;
  loadPlayerError: boolean = false;
  loadCurrentWeekError: boolean = false;
  players: PlayerModel[] = [];
  weekPlan: WeeklyScheduleModel | null = null;

  private readonly _trainService: TrainService = inject(TrainService);
  private readonly _toaster: ToastrService = inject(ToastrService);
  private readonly _translate: TranslateService = inject(TranslateService);

  get filteredPlayers(): PlayerModel[] {
    const q = this.query.toLowerCase().trim();
    if (!q) return this.players;
    return this.players.filter(player => player.displayName.toLowerCase().includes(q));
  }

  ngOnInit() {
    this.getPlayers();
    this.getCurrentWeek();
  }

  getPlayers() {
    this._trainService.getPlayers().subscribe({
      next: (response => {
        if (response) {
          this.players = response;
        } else {
          this.players = [];
        }
      }),
      error: (error) => {
        this.players = [];
        console.log(error);
        this.loadPlayerError = true;
        const errorTitle = this._translate.instant('Train.Toaster.Player.Error.Title');
        const errorText = this._translate.instant('Train.Toaster.Player.Error.Description');
        this._toaster.error(errorText, errorTitle);
      }
    })
  }

  getCurrentWeek() {
    this._trainService.getCurrentWeek().subscribe({
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

  toggle(): void {
    this.open = !this.open;
  }

  selectPlayer(player: PlayerModel): void {
    this.selectedPlayer = player;
    this.open = false;
    this.query = '';
  }

  submitApplication(): void {
    if (!this.selectedPlayer) return;

    if (this.selectedPlayer.registered) {
      const title = this._translate.instant('Train.SweetAlert.AlreadyRegister.Title');
      const html = this._translate.instant('Train.SweetAlert.AlreadyRegister.Description', {name: this.selectedPlayer.displayName});
      const button = this._translate.instant('Train.SweetAlert.AlreadyRegister.Button');
      Swal.fire({
        icon: 'info',
        title: title,
        html: html,
        confirmButtonText: button,
        confirmButtonColor: '#0dcaf0'
      }).then(_ => this.selectedPlayer = null);
      return;
    }

    const userRegisteredPlayer = sessionStorage.getItem('playerName');

    if (userRegisteredPlayer) {
      if (this.selectedPlayer.displayName !== userRegisteredPlayer) {
        const title = this._translate.instant('Train.SweetAlert.NoOthers.Title');
        const html = this._translate.instant('Train.SweetAlert.NoOthers.Description', {name: userRegisteredPlayer});
        const button = this._translate.instant('Train.SweetAlert.NoOthers.Button');
        Swal.fire({
          icon: 'warning',
          title: title,
          html: html,
          confirmButtonText: button,
          confirmButtonColor: '#0dcaf0'
        }).then(_ => this.selectedPlayer = null);
        return;
      }
    }


    this._trainService.registerPlayer(this.selectedPlayer.id).subscribe({
      next: (response) => {
        if (response) {
          sessionStorage.setItem('playerName', this.selectedPlayer!.displayName);
          const title = this._translate.instant('Train.SweetAlert.Register.Title');
          const html = this._translate.instant('Train.SweetAlert.Register.Description', {name: this.selectedPlayer!.displayName});
          const button = this._translate.instant('Train.SweetAlert.Register.Button');
          Swal.fire({
            icon: 'success',
            title: title,
            html: html,
            confirmButtonText: button,
            confirmButtonColor: '#198754'
          }).then(_ => {
            this.selectedPlayer = null;
            this.getPlayers();
          });
        }
      },
      error: (error) => {
        console.log(error);
        const errorTitle = this._translate.instant('Train.Toaster.Register.Error.Title');
        const errorText = this._translate.instant('Train.Toaster.Register.Error.Description');
        this._toaster.error(errorText, errorTitle);
        this.selectedPlayer = null;
      }
    });


  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const target = event.target as HTMLElement | null;
    if (!target) return;

    if (!target.closest('.dropdown')) {
      this.open = false;
    }
  }

}
