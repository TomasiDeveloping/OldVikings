import {Component, EventEmitter, inject, Input, OnInit, Output} from '@angular/core';
import {PlayerModel} from "../../../../../models/player.model";
import {TrainService} from "../../../../../services/train.service";
import Swal from "sweetalert2";

@Component({
  selector: 'app-player-modal',
  templateUrl: './player-modal.component.html',
  styleUrl: './player-modal.component.scss'
})
export class PlayerModalComponent implements OnInit {

  private readonly _trainService: TrainService = inject(TrainService);

  @Input() player?: PlayerModel;
  @Output() close = new EventEmitter<void>();
  @Output() saved = new EventEmitter<void>();

  isEditMode = false;

  model = {
    displayName: '',
    registered: false,
    approved: false
  };

  ngOnInit() {
    if (this.player) {
      this.model = this.player;
      this.isEditMode = true;
    }
  }

  save() {
    if (!this.model.displayName.trim()) return;

    if (this.isEditMode) {
      const newPlayer: PlayerModel = {
        displayName: this.model.displayName.trim(),
        registered: this.model.registered,
        approved: this.model.approved,
        id: this.player!.id
      };
      this.updatePlayer(newPlayer);
    } else {
      this.createPlayer(this.model.displayName.trim());
    }
  }

  createPlayer(playerName: string) {
    this._trainService.createPlayer(playerName).subscribe({
      next: (response: PlayerModel) => {
        if (response) {
          this.saved.emit();
          Swal.fire({
            icon: 'success',
            title: 'Created!',
            timer: 1200,
            showConfirmButton: false,
          }).then()
        }
      },
      error: err => {
        console.log(err);
        Swal.fire({
          icon: 'error',
          title: 'Error creating player',
        }).then()
      }
    });
  }

  updatePlayer(player: PlayerModel) {
    this._trainService.updatePlayer(player.id, player).subscribe({
      next: (response: PlayerModel) => {
        if (response) {
          this.saved.emit();
          Swal.fire({
            icon: 'success',
            title: 'Updated!',
            timer: 1200,
            showConfirmButton: false,
          }).then()
        }
      },
      error: err => {
        console.log(err);
        Swal.fire({
          icon: 'error',
          title: 'Error updating player',
        }).then()
      }
    })
  }
}
