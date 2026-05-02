import { NgModule} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';
import { HomeComponent } from './pages/home/home.component';
import { VsDuellComponent } from './pages/vs-duell/vs-duell.component';
import {TranslateLoader, TranslateModule} from "@ngx-translate/core";
import {TranslateHttpLoader} from "@ngx-translate/http-loader";
import { HttpClient, withInterceptors, provideHttpClient, withInterceptorsFromDi } from "@angular/common/http";
import {ToastrModule} from "ngx-toastr";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import { RulesComponent } from './pages/rules/rules.component';
import { DesertStormComponent } from './pages/desert-storm/desert-storm.component';
import { AlliancesComponent } from './pages/alliances/alliances.component';
import { BasicsComponent } from './pages/basics/basics.component';
import { TipsAndTricksComponent } from './pages/tips-and-tricks/tips-and-tricks.component';
import { BuildingsComponent } from './pages/buildings/buildings.component';
import { HeadqartersTableComponent } from './pages/buildings/headqarters-table/headqarters-table.component';
import { NgxScrollTopModule } from 'ngx-scrolltop';
import { GreetingModalComponent } from './pages/guestbook/greeting-modal/greeting-modal.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {NgxSpinnerModule} from "ngx-spinner";
import {spinnerInterceptor} from "./interceptors/spinner.interceptor";
import { MemberLoginComponent } from './auth/member-login/member-login.component';
import { SquadComponent } from './pages/squad/squad.component';
import { SeasonOneComponent } from './pages/season-one/season-one.component';
import { CounterCharactersPipe } from './pipes/counter-characters.pipe';
import { GuestbookComponent } from './pages/guestbook/guestbook.component';
import {NgxPaginationModule} from "ngx-pagination";
import { MapComponent } from './pages/map/map.component';
import { MarshallComponent } from './pages/marshall/marshall.component';
import { SeasonTwoComponent } from './pages/season-two/season-two.component';
import { VideosComponent } from './pages/videos/videos.component';
import { R4Component } from './pages/r4/r4.component';
import { AllianceMvpComponent } from './pages/alliance-mvp/alliance-mvp.component';
import { SeasonTwoCelebrationComponent } from './pages/season-two/season-two-celebration/season-two-celebration.component';
import { CapitolComponent } from './pages/capitol/capitol.component';
import { WeekDayPipe } from './pipes/week-day.pipe';
import { PlayerManagerComponent } from './pages/player-manager/player-manager.component';
import { SeasonThreeComponent } from './pages/season-three/season-three.component';
import { SeasonThreeAllianceLeaderboardComponent } from './pages/season-three/season-three-alliance-leaderboard/season-three-alliance-leaderboard.component';
import { R4RolesComponent } from './pages/r4-roles/r4-roles.component';
import { SeasonsComponent } from './pages/seasons/seasons.component';
import { S4CalculatorComponent } from './pages/s4-calculator/s4-calculator.component';
import { SeasonLeaderBordComponent } from './pages/seasons/season-leader-bord/season-leader-bord.component';
import { MemorialComponent } from './pages/memorial/memorial.component';
import { TrainComponent } from './pages/train/train.component';
import { DayNamePipe } from './pipes/day-name.pipe';
import { NextWeekTrainComponent } from './pages/train/next-week-train/next-week-train.component';
import { TrainWeekPlanComponent } from './pages/train/train-week-plan/train-week-plan.component';
import { FeedbackComponent } from './pages/feedback/feedback.component';
import { FeedbackDetailComponent } from './pages/feedback/feedback-detail/feedback-detail.component';
import { TrainHistoryComponent } from './pages/train/train-history/train-history.component';
import { R4LoginComponent } from './pages/r4/r4-login/r4-login.component';
import { R4TrainSystemComponent } from './pages/r4/r4-train-system/r4-train-system.component';
import { TrainConductorComponent } from './pages/r4/r4-train-system/train-conductor/train-conductor.component';
import { TrainVipComponent } from './pages/r4/r4-train-system/train-vip/train-vip.component';
import { R4MemberComponent } from './pages/r4/r4-member/r4-member.component';
import { R4SettingsComponent } from './pages/r4/r4-settings/r4-settings.component';
import { TrainPlayerComponent } from './pages/r4/r4-train-system/train-player/train-player.component';
import { PlayerModalComponent } from './pages/r4/r4-train-system/train-player/player-modal/player-modal.component';
import { ConductorEditComponent } from './pages/r4/r4-train-system/train-conductor/conductor-edit/conductor-edit.component';
import { VipEditComponent } from './pages/r4/r4-train-system/train-vip/vip-edit/vip-edit.component';
import {jwtInterceptor} from "./interceptors/jwt.interceptor";

// AoT requires an exported function for factories
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({ declarations: [
        AppComponent,
        NavigationComponent,
        HomeComponent,
        VsDuellComponent,
        RulesComponent,
        DesertStormComponent,
        AlliancesComponent,
        BasicsComponent,
        TipsAndTricksComponent,
        BuildingsComponent,
        HeadqartersTableComponent,
        GreetingModalComponent,
        MemberLoginComponent,
        SquadComponent,
        SeasonOneComponent,
        CounterCharactersPipe,
        GuestbookComponent,
        MapComponent,
        MarshallComponent,
        SeasonTwoComponent,
        VideosComponent,
        R4Component,
        AllianceMvpComponent,
        SeasonTwoCelebrationComponent,
        CapitolComponent,
        WeekDayPipe,
        PlayerManagerComponent,
        SeasonThreeComponent,
        SeasonThreeAllianceLeaderboardComponent,
        R4RolesComponent,
        SeasonsComponent,
        S4CalculatorComponent,
        SeasonLeaderBordComponent,
        MemorialComponent,
        TrainComponent,
        DayNamePipe,
        NextWeekTrainComponent,
        TrainWeekPlanComponent,
        FeedbackComponent,
        FeedbackDetailComponent,
        TrainHistoryComponent,
        R4LoginComponent,
        R4TrainSystemComponent,
        TrainConductorComponent,
        TrainVipComponent,
        R4MemberComponent,
        R4SettingsComponent,
        TrainPlayerComponent,
        PlayerModalComponent,
        ConductorEditComponent,
        VipEditComponent
    ],
    bootstrap: [AppComponent], imports: [BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    NgxScrollTopModule,
    NgxSpinnerModule,
    NgxPaginationModule, FormsModule], providers: [
        provideHttpClient(withInterceptors([spinnerInterceptor, jwtInterceptor])),
        provideHttpClient(withInterceptorsFromDi())
    ] })
export class AppModule { }
