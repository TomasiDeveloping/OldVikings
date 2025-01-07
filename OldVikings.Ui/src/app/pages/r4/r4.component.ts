import {Component, inject, OnInit} from '@angular/core';
import {PlayerService} from "../../services/player.service";
import {ToastrService} from "ngx-toastr";
import {PlayerMvp} from "../../models/playerMvp.model";

@Component({
  selector: 'app-r4',
  templateUrl: './r4.component.html',
  styleUrl: './r4.component.scss'
})
export class R4Component implements OnInit {

  private readonly _playerService: PlayerService = inject(PlayerService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  public mvpPlayers: PlayerMvp[] = [];

  ngOnInit() {
    this.getMvpPlayers();
  }

  getMvpPlayers() {
    this._playerService.getLeadershipMvp().subscribe({
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
