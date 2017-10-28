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
    MatSelectModule,
    MatListModule,
    MatSnackBarModule,
    //MatSidenav,
    MatToolbarModule
} from '@angular/material';


import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { ShareDialogComponent } from './components/shared/share-dialog.component';
import { ReportComponent } from './components/report/report-list.component';
import { ReportEditComponent } from './components/report/report-edit.component';
//import { ReportModule } from './components/report/report.module';

import { TableHttpExample } from './components/table/table-http-example';

import { MATERIAL_COMPATIBILITY_MODE, MAT_PLACEHOLDER_GLOBAL_OPTIONS } from '@angular/material';




@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        TableHttpExample,
        ShareDialogComponent,
        ReportComponent,
        ReportEditComponent
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
            {
                path: 'report',
                component: ReportEditComponent
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
        MatSelectModule,
        MatListModule,
        MatSnackBarModule,
        MatToolbarModule
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
        MatDialogModule,
        MatSelectModule,
        MatListModule,
        MatSnackBarModule,
        MatToolbarModule
    ],
    providers: [
        { provide: MATERIAL_COMPATIBILITY_MODE, useValue: true },
        { provide: MAT_PLACEHOLDER_GLOBAL_OPTIONS, useValue: { float: 'always' } }
    ]
})
export class AppModuleShared {
}
