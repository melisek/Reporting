﻿import { NgModule } from '@angular/core';

import {
    MatProgressSpinnerModule,
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
    MatToolbarModule,
    MatTabsModule,
    MatIconModule,
    MatExpansionModule
} from '@angular/material';

import { MATERIAL_COMPATIBILITY_MODE, MAT_PLACEHOLDER_GLOBAL_OPTIONS } from '@angular/material';


@NgModule({
    declarations: [
    ],
    imports: [
        MatProgressSpinnerModule,
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
        MatToolbarModule,
        MatExpansionModule,
        MatTabsModule,
        MatIconModule
    ],
    exports: [
        MatProgressSpinnerModule,
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
        MatToolbarModule,
        MatExpansionModule,
        MatTabsModule,
        MatIconModule
    ],
    providers: [
        { provide: MATERIAL_COMPATIBILITY_MODE, useValue: true },
        { provide: MAT_PLACEHOLDER_GLOBAL_OPTIONS, useValue: { float: 'always' } }
    ]
})
export class MaterialModule {
}