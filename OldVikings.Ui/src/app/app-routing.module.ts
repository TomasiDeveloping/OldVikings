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

const routerOptions: ExtraOptions = {
  anchorScrolling: 'enabled',
  scrollPositionRestoration: 'enabled'
}
const routes: Routes = [
  {path: 'home', component: HomeComponent},
  {path: 'vs', component: VsDuellComponent, canActivate: [memberGuard]},
  {path: 'rules', component: RulesComponent},
  {path: 'desert-storm', component: DesertStormComponent, canActivate: [memberGuard]},
  {path: 'alliances', component: AlliancesComponent},
  {path: 'basics', component: BasicsComponent},
  {path: 'tips-and-tricks', component: TipsAndTricksComponent, canActivate: [memberGuard]},
  {path: 'buildings', component: BuildingsComponent, canActivate: [memberGuard]},
  {path: 'squad', component: SquadComponent},
  {path: 'member-login', component: MemberLoginComponent},
  {path: '', redirectTo: '/home', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, routerOptions)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
