import {Component, inject} from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-season-three',
  templateUrl: './season-three.component.html',
  styleUrl: './season-three.component.scss'
})
export class SeasonThreeComponent {

  private readonly _router: Router = inject(Router)

  goToLeaderboard() {
    this._router.navigate(['season-three-leaderboard']).then();
  }
}
