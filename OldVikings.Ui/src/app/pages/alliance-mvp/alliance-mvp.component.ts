import {Component, inject, OnInit} from '@angular/core';
import {PlayerService} from "../../services/player.service";
import {PlayerMvp} from "../../models/playerMvp.model";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-alliance-mvp',
  templateUrl: './alliance-mvp.component.html',
  styleUrl: './alliance-mvp.component.scss'
})
export class AllianceMvpComponent implements OnInit {

  private readonly _playerService: PlayerService = inject(PlayerService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  public mvpPlayers: PlayerMvp[] = [];

  ngOnInit() {
    this.getMvpPlayers();
  }

  getMvpPlayers() {
    this._playerService.getMvpPlayers().subscribe({
      next: ((response) => {
        if (response) {
          this.mvpPlayers = response;
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Could not load data', 'Load data');
      })
    });
  }

}
