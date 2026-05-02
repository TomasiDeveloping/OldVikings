import {NgModule} from '@angular/core';
import {ExtraOptions, RouterModule, Routes} from '@angular/router';
import {HomeComponent} from "./pages/home/home.component";
import {VsDuellComponent} from "./pages/vs-duell/vs-duell.component";
import {RulesComponent} from "./pages/rules/rules.component";
import {DesertStormComponent} from "./pages/desert-storm/desert-storm.component";
import {AlliancesComponent} from "./pages/alliances/alliances.component";
import {BasicsComponent} from "./pages/basics/basics.component";
import {TipsAndTricksComponent} from "./pages/tips-and-tricks/tips-and-tricks.component";
import {BuildingsComponent} from "./pages/buildings/buildings.component";
import {MemberLoginComponent} from "./auth/member-login/member-login.component";
import {memberGuard} from "./guards/member.guard";
import {SquadComponent} from "./pages/squad/squad.component";
import {SeasonOneComponent} from "./pages/season-one/season-one.component";
import {GuestbookComponent} from "./pages/guestbook/guestbook.component";
import {MapComponent} from "./pages/map/map.component";
import {MarshallComponent} from "./pages/marshall/marshall.component";
import {SeasonTwoComponent} from "./pages/season-two/season-two.component";
import {VideosComponent} from "./pages/videos/videos.component";
import {R4Component} from "./pages/r4/r4.component";
import {AllianceMvpComponent} from "./pages/alliance-mvp/alliance-mvp.component";
import {
  SeasonTwoCelebrationComponent
} from "./pages/season-two/season-two-celebration/season-two-celebration.component";
import {CapitolComponent} from "./pages/capitol/capitol.component";
import {PlayerManagerComponent} from "./pages/player-manager/player-manager.component";
import {SeasonThreeComponent} from "./pages/season-three/season-three.component";
import {
  SeasonThreeAllianceLeaderboardComponent
} from "./pages/season-three/season-three-alliance-leaderboard/season-three-alliance-leaderboard.component";
import {R4RolesComponent} from "./pages/r4-roles/r4-roles.component";
import {SeasonsComponent} from "./pages/seasons/seasons.component";
import {SeasonLeaderBordComponent} from "./pages/seasons/season-leader-bord/season-leader-bord.component";
import {MemorialComponent} from "./pages/memorial/memorial.component";
import {TrainComponent} from "./pages/train/train.component";
import {FeedbackComponent} from "./pages/feedback/feedback.component";
import {FeedbackDetailComponent} from "./pages/feedback/feedback-detail/feedback-detail.component";
import {TrainHistoryComponent} from "./pages/train/train-history/train-history.component";
import {r4Guard} from "./guards/r4.guard";
import {R4LoginComponent} from "./pages/r4/r4-login/r4-login.component";
import {TrainConductorComponent} from "./pages/r4/r4-train-system/train-conductor/train-conductor.component";
import {R4TrainSystemComponent} from "./pages/r4/r4-train-system/r4-train-system.component";
import {TrainVipComponent} from "./pages/r4/r4-train-system/train-vip/train-vip.component";
import {R4MemberComponent} from "./pages/r4/r4-member/r4-member.component";
import {R4SettingsComponent} from "./pages/r4/r4-settings/r4-settings.component";
import {TrainPlayerComponent} from "./pages/r4/r4-train-system/train-player/train-player.component";

const routerOptions: ExtraOptions = {
  anchorScrolling: 'enabled',
  scrollPositionRestoration: 'enabled'
}
const routes: Routes = [
  {path: 'home', component: HomeComponent},
  {path: 'vs', component: VsDuellComponent, canActivate: [memberGuard]},
  {path: 'rules', component: RulesComponent},
  {path: 'desert-storm', component: DesertStormComponent, canActivate: [memberGuard]},
  {path: 'marshall', component: MarshallComponent, canActivate: [memberGuard]},
  {path: 'alliances', component: AlliancesComponent, canActivate: [memberGuard]},
  {path: 'train', component: TrainComponent, canActivate: [memberGuard]},
  {path: 'train-history', component: TrainHistoryComponent, canActivate: [memberGuard]},
  {path: 'basics', component: BasicsComponent},
  {path: 'tips-and-tricks', component: TipsAndTricksComponent, canActivate: [memberGuard]},
  {path: 'buildings', component: BuildingsComponent, canActivate: [memberGuard]},
  {path: 'squad', component: SquadComponent},
  {path: 'member-login', component: MemberLoginComponent},
  {path: 'seasons', component: SeasonsComponent},
  {path: 'season-one', component: SeasonOneComponent},
  {path: 'season-two', component: SeasonTwoComponent},
  {path: 'season-two-celebration', component: SeasonTwoCelebrationComponent},
  {path: 'season-three', component: SeasonThreeComponent},
  {path: 'season-leaderboard', component: SeasonLeaderBordComponent, canActivate: [memberGuard]},
  {path: 'map', component: MapComponent, canActivate: [memberGuard]},
  {path: 'alliance-mvp', component: AllianceMvpComponent, canActivate: [memberGuard]},
  {
    path: 'r4',
    component: R4Component,
    canActivate: [r4Guard],
    canDeactivate: [r4Guard],
    children: [
      { path: '', redirectTo: 'train-system', pathMatch: 'full' },
      {
        path: 'train-system',
        component: R4TrainSystemComponent,
        children: [
          { path: '', redirectTo: 'conductor', pathMatch: 'full' },
          { path: 'conductor', component: TrainConductorComponent },
          {path: 'vip', component: TrainVipComponent},
          {path: 'player', component: TrainPlayerComponent },
        ]
      },
      {path: 'members', component: R4MemberComponent},
      {path: 'settings', component: R4SettingsComponent}
    ]
  },
  {path: 'r4/train-conductor', component: TrainConductorComponent},
  {path: 'r4-login', component: R4LoginComponent},
  {path: 'r4-roles', component: R4RolesComponent, canActivate: [memberGuard]},
  {path: 'capitol', component: CapitolComponent},
  {path: 'player-manager', component: PlayerManagerComponent},
  {path: 'guestbook', component: GuestbookComponent},
  {path: 'memorial', component: MemorialComponent},
  {path: 'videos', component: VideosComponent, canActivate: [memberGuard]},
  {path: 'feedback', component: FeedbackComponent, canActivate: [memberGuard]},
  {path: 'feedback/:id', component: FeedbackDetailComponent, canActivate: [memberGuard]},
  {path: '', redirectTo: '/home', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, routerOptions)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
