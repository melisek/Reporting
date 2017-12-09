import { NgModule } from '@angular/core';

import { NgxChartsModule } from '@swimlane/ngx-charts'

import { AppModuleShared } from '../../app.module.shared';

import { ChartEditComponent } from './chart-edit.component';
import { ChartService } from './chart.service';
import { ChartDirective } from './chart.directive';

import { HorizontalBarChartComponent } from './types/chart-horizontal-bar.component';
import { VerticalBarChartComponent } from './types/chart-vertical-bar.component';
import { PieChartComponent } from './types/chart-pie.component';
import { LineChartComponent } from './types/chart-line.component';

@NgModule({
    declarations: [
        ChartEditComponent,
        HorizontalBarChartComponent,
        VerticalBarChartComponent,
        PieChartComponent,
        LineChartComponent,
    ],
    entryComponents: [HorizontalBarChartComponent, VerticalBarChartComponent, PieChartComponent, LineChartComponent],
    imports: [
        NgxChartsModule,
        AppModuleShared
    ],
    exports: [
        NgxChartsModule,
        ChartEditComponent
    ],
    providers: [
        ChartService,
    ]
})
export class ChartModule {
}
