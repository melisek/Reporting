import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AppModuleShared } from '../../app.module.shared';
import { DashboardListComponent } from './dashboard-list.component';
import { DashboardService } from './dashboard.service';
import { AuthGuard } from '../user/auth-guard.service';
import { DashboardEditComponent } from './dashboard-edit.component';

import { NgxDnDModule } from '@swimlane/ngx-dnd'

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: 'dashboards',
                component: DashboardListComponent,
                canActivate: [AuthGuard]
            },
            {
                path: 'dashboards/create',
                component: DashboardEditComponent,
            },
            {
                path: 'dashboards/edit/:reportGUID',
                component: DashboardEditComponent,
            }
        ]),
        AppModuleShared,
        NgxDnDModule
    ],
    declarations: [
        DashboardListComponent,
        DashboardEditComponent
        
    ],
    providers: [
        DashboardService
    ]
})
export class DashboardModule { }