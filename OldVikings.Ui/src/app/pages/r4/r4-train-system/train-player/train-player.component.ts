import {Component, inject, OnInit} from '@angular/core';
import {TrainService} from "../../../../services/train.service";
import {PlayerModel} from "../../../../models/player.model";
import Swal from "sweetalert2";

@Component({
  selector: 'app-train-player',
  templateUrl: './train-player.component.html',
  styleUrl: './train-player.component.scss'
})
export class TrainPlayerComponent implements OnInit {

  private readonly _trainService: TrainService = inject(TrainService);

  players: PlayerModel[] = [];
  filteredPlayers: PlayerModel[] = [];

  searchTerm = '';

  filterRegistered: boolean | null = null;
  filterApproved: boolean | null = null;

  registeredPlayers: number = 0;
  totalPlayers: number = 0;
  notRegisteredPlayers: number = 0;

  page: number = 1;
  pageSize: number = 10;

  showModal = false;
  selectedPlayer?: PlayerModel = undefined;

  ngOnInit() {
    this.getPlayers();
  }

  getPlayers() {
    this._trainService.getPlayers().subscribe({
      next: (response) => {
        if (response) {
          this.players = response;
          this.filteredPlayers = response;
          this.totalPlayers = response.length;
          this.registeredPlayers = response.filter(p => p.registered).length;
          this.notRegisteredPlayers = this.totalPlayers - this.registeredPlayers;
        } else {
          this.players = [];
          this.filteredPlayers = [];
        }
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  applyFilter() {

    const search = this.searchTerm?.toLowerCase() ?? '';

    this.filteredPlayers = this.players.filter(c => {

      const matchesSearch =
        !search || c.displayName.toLowerCase().includes(search);

      const matchesRegistered =
        this.filterRegistered === null || c.registered === this.filterRegistered;

      const matchesApproved =
        this.filterApproved === null || c.approved === this.filterApproved;


      return matchesSearch && matchesRegistered && matchesApproved;
    });

    this.page = 1; // wichtig: reset pagination
  }

  openAddModal() {
    this.selectedPlayer = undefined;
    this.showModal = true;
  }

  openEditModal(player: PlayerModel) {
    this.selectedPlayer = player;
    this.showModal = true;
  }

  closeAddModal() {
    this.showModal = false;
  }

  onSaved() {
    this.getPlayers();
    this.closeAddModal();
  }

  confirmDelete(player: PlayerModel) {
    Swal.fire({
      title: 'Delete Player?',
      text: `Are you sure you want to delete "${player.displayName}"?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#dc3545',
      cancelButtonColor: '#6c757d',
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'Cancel'
    }).then((result) => {

      if (result.isConfirmed) {
        this.deletePlayer(player.id);
      }
    });
  }

  deletePlayer(id: string) {
    this._trainService.deletePlayer(id).subscribe({
      next: () => {
        this.getPlayers();
        Swal.fire({
          icon: 'success',
          title: 'Deleted!',
          timer: 1200,
          showConfirmButton: false,
        }).then()
      }, error: (error) => {
        console.error(error);
        Swal.fire({
          icon: 'error',
          title: 'Error deleting player',
        }).then()
      }
    })
  }

}
