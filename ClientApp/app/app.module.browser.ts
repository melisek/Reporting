import { NgModule } from '@angular/core';
import { AppModuleShared } from './app.module.shared';
import { AppComponent } from './components/app/app.component';
import { HorizontalBarChartComponent } from './components/chart/chart-horizontal-bar.component';
import { ChartDirective } from './components/chart/chart.directive';

@NgModule({
    bootstrap: [AppComponent],
    imports: [
        AppModuleShared
    ],
    //declarations: [HorizontalBarChartComponent, ChartDirective],
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
