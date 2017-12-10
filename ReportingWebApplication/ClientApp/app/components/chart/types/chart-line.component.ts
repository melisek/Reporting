import { Component, Input } from '@angular/core';

import { ChartBaseComponent } from '../chart-base.component';

@Component({
    template: `
     <ngx-charts-line-chart
        [results]="data"
        [view]=[800,400]
        [xAxis] = "options[0].value"
        [yAxis] = "options[1].value"
        [showGridLines] = "options[2].value"
        [roundDomains] = "options[3].value"
        [showXAxisLabel] = "options[4].value"
        [showYAxisLabel] = "options[5].value"
        [xAxisLabel] = "options[6].value"
        [yAxisLabel] = "options[7].value"
        [schemeType] = "options[8].value"
       
        [timeline] = "true"
        [autoScale] = "options[9].value"
        [animations] = "options[10].value"
        [legend] = "options[11].value"
        [legendTitle] = "options[12].value"
        [gradient] = "options[13].value"
        [tooltipDisabled] = "options[14].value">
    </ngx-charts-line-chart>
  `
}) // [yScaleMax] = "options[9].value"
export class LineChartComponent extends ChartBaseComponent { }