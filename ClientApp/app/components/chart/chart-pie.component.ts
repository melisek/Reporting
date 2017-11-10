import { Component, Input } from '@angular/core';

import { ChartBaseComponent } from './chart-base.component';

@Component({
    template: `
     <ngx-charts-pie-chart
        [results]="data"
        [explodeSlices]="options[0].value"
        [doughnut]="options[1].value"
        [labels]="options[2].value"
        [animations] = "options[3].value"
        [legend] = "options[4].value"
        [legendTitle] = "options[5].value"
        [gradient] = "options[6].value"
        [tooltipDisabled] = "options[7].value">
    </ngx-charts-pie-chart>
  `
})
export class PieChartComponent extends ChartBaseComponent { }