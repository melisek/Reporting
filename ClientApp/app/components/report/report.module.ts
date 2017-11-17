import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AppModuleShared } from '../../app.module.shared';
import { ChartModule } from '../chart/chart.module';

import { ReportListComponent } from './report-list.component';
import { ReportEditComponent } from './report-edit.component';
import { ReportService } from './report.service';


@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: 'reports',
                component: ReportListComponent
            },/*
            {
                path: ':id',
                component: ProductDetailComponent,
                resolve: { product: ProductResolver }
            },*/
            {
                path: 'reports/create',
                component: ReportEditComponent,
                //canDeactivate: [ProductEditGuard],
                //resolve: { product: ProductResolver },
            },
            {
                path: 'reports/edit/:reportGUID',
                component: ReportEditComponent,
                //canDeactivate: [ProductEditGuard],
                //resolve: { product: ProductResolver },
            }
        ]),
        AppModuleShared,
        ChartModule
    ],
    declarations: [
        ReportListComponent,
        ReportEditComponent,
        
    ],
    providers: [
        ReportService
    ]
})
export class ReportModule { }