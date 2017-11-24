import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AppModuleShared } from '../../app.module.shared';
import { DashboardListComponent } from './dashboard-list.component';
import { DashboardService } from './dashboard.service';
import { AuthGuard } from '../user/auth-guard.service';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: 'dashboards',
                component: DashboardListComponent,
                canActivate: [AuthGuard]
            },
            //{
            //    path: 'reports/create',
            //    component: ReportEditComponent,
            //},
            //{
            //    path: 'reports/edit/:reportGUID',
            //    component: ReportEditComponent,
            //}
        ]),
        AppModuleShared
    ],
    declarations: [
        DashboardListComponent,
        //ReportEditComponent,
        
    ],
    providers: [
        DashboardService
    ]
})
export class DashboardModule { }