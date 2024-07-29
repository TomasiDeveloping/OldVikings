import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {HomeComponent} from "./pages/home/home.component";
import {VsDuellComponent} from "./pages/vs-duell/vs-duell.component";
import {RulesComponent} from "./pages/rules/rules.component";

const routes: Routes = [
  {path: 'home', component: HomeComponent},
  {path: 'vs', component: VsDuellComponent},
  {path: 'rules', component: RulesComponent},
  {path: '', redirectTo: '/home', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
