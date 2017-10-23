//import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
//import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import {
    MatButtonModule,
    MatCheckboxModule,
    MatInputModule,
    MatCardModule,
    MatButtonToggleModule,
    MatMenuModule,
    MatSidenavModule,
    MatSortModule,
    MatTableModule,
    MatPaginatorModule,
    MatTooltipModule,
    MatDialogModule,
} from '@angular/material';


import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { ShareDialogComponent } from './components/shared/share-dialog.component';

//import { ReportModule } from './components/report/report.module';

import { TableHttpExample } from './components/table/table-http-example';

import { MATERIAL_COMPATIBILITY_MODE } from '@angular/material';
import { ReportComponent } from './components/report/report-list.component';



@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        TableHttpExample,
        ShareDialogComponent,
        ReportComponent,
        
    ],
    entryComponents: [ShareDialogComponent],
    imports: [
        //BrowserModule,
        BrowserAnimationsModule,    
        CommonModule,
        HttpModule,
        HttpClientModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            {
                path: 'reports',
                component: ReportComponent
            },
            { path: 'table', component: TableHttpExample },
            { path: '**', redirectTo: 'home' }
        ]),

        //ReportModule,

        MatButtonModule,
        MatCheckboxModule,
        MatInputModule,
        MatCardModule,
        MatButtonToggleModule,
        MatMenuModule,
        MatSidenavModule,
        MatSortModule,
        MatTableModule,
        MatPaginatorModule,
        MatTooltipModule,
        MatDialogModule,
        
    ],
    exports: [
        MatButtonModule,
        MatCheckboxModule,
        MatInputModule,
        MatCardModule,
        MatButtonToggleModule,
        MatMenuModule,
        MatSidenavModule,
        MatSortModule,
        MatTableModule,
        MatPaginatorModule,
        MatTooltipModule,
        MatDialogModule
    ],
    providers: [
        { provide: MATERIAL_COMPATIBILITY_MODE, useValue: true },
    ]
})
export class AppModuleShared {
}
