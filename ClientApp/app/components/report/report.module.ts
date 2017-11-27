import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AppModuleShared } from '../../app.module.shared';
import { ChartModule } from '../chart/chart.module';

import { ReportListComponent } from './report-list.component';
import { ReportEditComponent } from './report-edit.component';
import { ReportService } from './report.service';
import { AuthGuard } from '../user/auth-guard.service';
import { QueryService } from '../query/query.service';


@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: 'reports',
                component: ReportListComponent,
                canActivate: [AuthGuard]
            },/*
            {
                path: ':id',
                component: ProductDetailComponent,
                resolve: { product: ProductResolver }
            },*/
            {
                path: 'reports/create',
                component: ReportEditComponent,
                canActivate: [AuthGuard]
                //canDeactivate: [ProductEditGuard],
                //resolve: { product: ProductResolver },
            },
            {
                path: 'reports/edit/:reportGUID',
                component: ReportEditComponent,
                canActivate: [AuthGuard]
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
        ReportService,
        QueryService
    ]
})
export class ReportModule { }