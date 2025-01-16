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
  {path: 'alliances', component: AlliancesComponent},
  {path: 'basics', component: BasicsComponent},
  {path: 'tips-and-tricks', component: TipsAndTricksComponent, canActivate: [memberGuard]},
  {path: 'buildings', component: BuildingsComponent, canActivate: [memberGuard]},
  {path: 'squad', component: SquadComponent},
  {path: 'member-login', component: MemberLoginComponent},
  {path: 'season-one', component: SeasonOneComponent},
  {path: 'season-two', component: SeasonTwoComponent},
  {path: 'season-two-celebration', component: SeasonTwoCelebrationComponent},
  {path: 'map', component: MapComponent, canActivate: [memberGuard]},
  {path: 'alliance-mvp', component: AllianceMvpComponent, canActivate: [memberGuard]},
  {path: 'r4', component: R4Component, canActivate: [memberGuard]},
  {path: 'capitol', component: CapitolComponent},
  {path: 'guestbook', component: GuestbookComponent},
  {path: 'videos', component: VideosComponent, canActivate: [memberGuard]},
  {path: '', redirectTo: '/home', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, routerOptions)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
