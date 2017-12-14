import { Component, Input } from '@angular/core';

import { IChart, IChartOption } from './chart';

@Component({})
export class ChartBaseComponent implements IChart {
    @Input()
    options: IChartOption[];
    @Input()
    data: any[];
}
