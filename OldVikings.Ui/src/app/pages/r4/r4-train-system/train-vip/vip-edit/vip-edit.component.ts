import {Component, EventEmitter, inject, Input, OnInit, Output} from '@angular/core';
import {TrainVipModel} from "../../../../../models/trainVip.model";
import {TrainService} from "../../../../../services/train.service";
import Swal from "sweetalert2";

@Component({
  selector: 'app-vip-edit',
  templateUrl: './vip-edit.component.html',
  styleUrl: './vip-edit.component.scss'
})
export class VipEditComponent implements OnInit {

  private readonly trainService: TrainService = inject(TrainService);

  @Input() vip!: TrainVipModel;
  @Output() saved: EventEmitter<any> = new EventEmitter();
  @Output() close: EventEmitter<any> = new EventEmitter();

  canForcePick = false;

  model = {
    forcePick: false,
    blockNextCycle: false,
  }


  ngOnInit() {

    this.canForcePick = !this.vip.isAvailable;

    this.model = {
      forcePick: this.vip.forcePick,
      blockNextCycle: this.vip.blockNextCycle,
    }
  }

  save() {
    this.vip.forcePick = this.model.forcePick;
    this.vip.blockNextCycle = this.model.blockNextCycle;

    this.trainService.updateVip(this.vip.playerId, this.vip).subscribe({
      next: (response) => {
        if (response) {
          Swal.fire({
            icon: 'success',
            title: 'Updated!',
            timer: 1200,
            showConfirmButton: false,
          }).then()
          this.saved.emit();
        }
      },
      error: (error) => {
        console.log(error);
        Swal.fire({
          icon: 'error',
          title: 'Error deleting player',
        }).then()
      }
    });
  }

  isDirty(): boolean {
    return (
      this.model.forcePick !== this.vip.forcePick ||
      this.model.blockNextCycle !== this.vip.blockNextCycle
    );
  }
}
