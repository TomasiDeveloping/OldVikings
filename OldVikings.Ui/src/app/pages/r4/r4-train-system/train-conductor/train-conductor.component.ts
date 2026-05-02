import {Component, inject, OnInit} from '@angular/core';
import {TrainService} from "../../../../services/train.service";
import {TrainConductorModel} from "../../../../models/trainConductor.model";

@Component({
  selector: 'app-train-conductor',
  templateUrl: './train-conductor.component.html',
  styleUrl: './train-conductor.component.scss'
})
export class TrainConductorComponent implements OnInit {

  private readonly _trainService: TrainService = inject(TrainService);

  trainsConductors: TrainConductorModel[] = [];
  filteredConductors: TrainConductorModel[] = [];

  searchTerm = '';

  filterPending: boolean | null = null;
  filterBlock: boolean | null = null;
  filterForce: boolean | null = null;

  page: number = 1;
  pageSize: number = 10;

  showModal: boolean = false;
  selectedConductor!: TrainConductorModel;


  get pendingCount(): number {
    return this.trainsConductors.filter(p => p.isAvailable).length
  }

  ngOnInit() {
    this.getConductors();
  }

  applyFilter() {

    const search = this.searchTerm?.toLowerCase() ?? '';

    this.filteredConductors = this.trainsConductors.filter(c => {

      const matchesSearch =
        !search || c.playerName.toLowerCase().includes(search);

      const matchesPending =
        this.filterPending === null || c.isAvailable === this.filterPending;

      const matchesBlock =
        this.filterBlock === null || c.blockNextCycle === this.filterBlock;

      const matchesForce =
        this.filterForce === null || c.forcePick === this.filterForce;

      return matchesSearch && matchesPending && matchesBlock && matchesForce;
    });

    this.page = 1; // wichtig: reset pagination
  }

  getConductors() {
    this._trainService.getConductors().subscribe({
      next: (response) => {
        if (response) {
          this.trainsConductors = response;
          this.filteredConductors = response;
        } else {
          this.trainsConductors = [];
          this.filteredConductors = [];
        }
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  openEditModal(conductor: TrainConductorModel): void {
    this.selectedConductor = conductor;
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  onSaved(): void {
    this.getConductors();
    this.closeModal();
  }
}
