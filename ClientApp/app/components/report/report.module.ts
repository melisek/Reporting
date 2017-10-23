import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { ReportComponent } from './report-list.component';
import { ReportService } from './report.service';

import { AppModuleShared } from '../../app.module.shared';

@NgModule({
    imports: [
        AppModuleShared,
        RouterModule.forChild([
            {
                path: 'reports',
                component: ReportComponent
            },/*
            {
                path: ':id',
                component: ProductDetailComponent,
                resolve: { product: ProductResolver }
            },
            {
                path: ':id/edit',
                component: ProductEditComponent,
                canDeactivate: [ProductEditGuard],
                resolve: { product: ProductResolver },
                children: [
                    { path: '', redirectTo: 'info', pathMatch: 'full' },
                    { path: 'info', component: ProductEditInfoComponent },
                    { path: 'tags', component: ProductEditTagsComponent }
                ]
            }*/
        ])
    ],
    declarations: [
        ReportComponent,

    ],
    providers: [
        ReportService
    ]
})
export class ReportModule { }