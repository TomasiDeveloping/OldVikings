import {Component, EventEmitter, inject, Input, OnInit, Output} from '@angular/core';
import {TrainConductorModel} from "../../../../../models/trainConductor.model";
import {TrainService} from "../../../../../services/train.service";
import Swal from "sweetalert2";

@Component({
  selector: 'app-conductor-edit',
  templateUrl: './conductor-edit.component.html',
  styleUrl: './conductor-edit.component.scss'
})
export class ConductorEditComponent implements OnInit {

  private readonly trainService: TrainService = inject(TrainService);

  @Input() conductor!: TrainConductorModel;
  @Output() saved: EventEmitter<any> = new EventEmitter();
  @Output() close: EventEmitter<any> = new EventEmitter();

  canForcePick = false;

  model = {
    forcePick: false,
    blockNextCycle: false,
  }


  ngOnInit() {

    this.canForcePick = !this.conductor.isAvailable;

    this.model = {
      forcePick: this.conductor.forcePick,
      blockNextCycle: this.conductor.blockNextCycle,
    }
  }

  save() {
    this.conductor.forcePick = this.model.forcePick;
    this.conductor.blockNextCycle = this.model.blockNextCycle;

    this.trainService.updateConductor(this.conductor.playerId, this.conductor).subscribe({
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
      this.model.forcePick !== this.conductor.forcePick ||
      this.model.blockNextCycle !== this.conductor.blockNextCycle
    );
  }
}
