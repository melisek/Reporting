import { NgModule } from '@angular/core';
import { AppModuleShared } from './app.module.shared';
import { AppComponent } from './components/app/app.component';
import { ReportModule } from './components/report/report.module';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { BrowserModule, Title } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { MatSidenavContainer, MatSidenavModule } from '@angular/material'
import { MaterialModule } from './components/material/material-imports.module';
import { ChartModule } from './components/chart/chart.module';
import { DashboardModule } from './components/dashboard/dashboard.module';
import { UserModule } from './components/user/user.module';
import { AuthModule } from './components/user/auth.module';
import { AuthGuard } from './components/user/auth-guard.service';
import { ChartDirective } from './components/chart/chart.directive';

@NgModule({
    bootstrap: [AppComponent],
    imports: [
        //BrowserModule,
        BrowserAnimationsModule,  
        //CommonModule,
        //FormsModule,
        HttpModule,
        //HttpClientModule,
        
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
            { path: '**', redirectTo: 'home' }
        ]),
        MaterialModule,
        ChartModule,
        ReportModule,
        DashboardModule,
        UserModule,
        AuthModule
    ],
    declarations: [AppComponent, HomeComponent, NavMenuComponent],
    providers: [
        { provide: 'BASE_URL', useFactory: getBaseUrl },
        Title
    ]
})
export class AppModule {
}

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}
