import { NgModule } from '@angular/core';
import {ExtraOptions, RouterModule, Routes} from '@angular/router';
import {HomeComponent} from "./pages/home/home.component";
import {VsDuellComponent} from "./pages/vs-duell/vs-duell.component";
import {RulesComponent} from "./pages/rules/rules.component";
import {DesertStormComponent} from "./pages/desert-storm/desert-storm.component";
import {AlliancesComponent} from "./pages/alliances/alliances.component";

const routerOptions: ExtraOptions = {
  anchorScrolling: 'enabled',
  scrollPositionRestoration: 'enabled'
}
const routes: Routes = [
  {path: 'home', component: HomeComponent},
  {path: 'vs', component: VsDuellComponent},
  {path: 'rules', component: RulesComponent},
  {path: 'desert-storm', component: DesertStormComponent},
  {path: 'alliances', component: AlliancesComponent},
  {path: '', redirectTo: '/home', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, routerOptions)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
