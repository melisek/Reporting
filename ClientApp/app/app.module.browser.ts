import { NgModule } from '@angular/core';
import { AppModuleShared } from './app.module.shared';
import { AppComponent } from './components/app/app.component';
import { ReportModule } from './components/report/report.module';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { MatSidenavContainer, MatSidenavModule } from '@angular/material'
import { MaterialModule } from './components/material/material-imports.module';
import { ChartModule } from './components/chart/chart.module';

@NgModule({
    bootstrap: [AppComponent],
    imports: [
        //BrowserModule,
        BrowserAnimationsModule,  
        //CommonModule,
        //FormsModule,
        HttpModule,
        HttpClientModule,
        
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            //{
            //    path: 'reports',
            //    component: ReportListComponent
            //},
            //{
            //    path: 'report',
            //    component: ReportEditComponent
            //},
            //{
            //    path: 'chart',
            //    component: ChartEditComponent
            //},
            //{ path: 'table', component: TableHttpExample },
            { path: '**', redirectTo: 'home' }
        ]),
        MaterialModule,
        ChartModule,
        ReportModule,
        
    ],
    declarations: [AppComponent, HomeComponent, NavMenuComponent],
   // entryComponents: [HorizontalBarChartComponent],
    providers: [
        { provide: 'BASE_URL', useFactory: getBaseUrl }
    ]
})
export class AppModule {
}

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}
