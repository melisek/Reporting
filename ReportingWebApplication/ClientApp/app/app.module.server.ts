// imports
import { NgModule, NgModuleFactory, NgModuleFactoryLoader, RendererFactory2, NgZone } from '@angular/core';
import { ServerModule, ɵServerRendererFactory2 } from '@angular/platform-server';
import { ɵAnimationEngine } from '@angular/animations/browser';
import { NoopAnimationsModule, ɵAnimationRendererFactory } from '@angular/platform-browser/animations';

import { AppModuleShared } from './app.module.shared';
import { AppComponent } from './components/app/app.component';


import { MatSidenavContainer } from '@angular/material'
import { ReportModule } from './components/report/report.module';
import { DashboardModule } from './components/dashboard/dashboard.module';
import { RouterModule } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { UserModule } from './components/user/user.module';
import { AuthModule } from './components/user/auth.module';


@NgModule({
    bootstrap: [AppComponent],
    imports: [
        ServerModule,
        ReportModule,
        DashboardModule,
        UserModule,
    ],
    providers: [
        Title
    ]
})
export class AppModule {
}