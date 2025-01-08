import { NgModule } from '@angular/core';
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
import {ReactiveFormsModule} from "@angular/forms";
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
        SeasonTwoCelebrationComponent
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
        NgxPaginationModule], providers: [
        provideHttpClient(withInterceptors([spinnerInterceptor])),
        provideHttpClient(withInterceptorsFromDi())
    ] })
export class AppModule { }
