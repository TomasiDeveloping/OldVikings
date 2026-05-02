import {Component, inject, OnInit} from '@angular/core';
import {TrainService} from "../../../../services/train.service";
import {TrainVipModel} from "../../../../models/trainVip.model";

@Component({
  selector: 'app-train-vip',
  templateUrl: './train-vip.component.html',
  styleUrl: './train-vip.component.scss'
})
export class TrainVipComponent implements OnInit {

  private readonly _trainService: TrainService = inject(TrainService);

  trainVips: TrainVipModel[] = [];
  filteredVips: TrainVipModel[] = [];

  searchTerm = '';

  filterPending: boolean | null = null;
  filterBlock: boolean | null = null;
  filterForce: boolean | null = null;

  page: number = 1;
  pageSize: number = 10;

  showModal: boolean = false;
  selectedVip!: TrainVipModel;

  get pendingCount(): number {
    return this.trainVips.filter(p => p.isAvailable).length
  }

  ngOnInit() {
    this.getVips();
  }

  applyFilter() {

    const search = this.searchTerm?.toLowerCase() ?? '';

    this.filteredVips = this.trainVips.filter(c => {

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

  getVips() {
    this._trainService.getVips().subscribe({
      next: (response) => {
        if (response) {
          this.trainVips = response;
          this.filteredVips = response;
        } else {
          this.trainVips = [];
          this.filteredVips = [];
        }
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  openEditModal(vip: TrainVipModel): void {
    this.selectedVip = vip;
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  onSaved(): void {
    this.getVips();
    this.closeModal();
  }
}
