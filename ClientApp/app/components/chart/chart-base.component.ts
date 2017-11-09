import { Component, Input } from '@angular/core';

import { IChart, IChartOption } from './chart';
import { INameValue } from '../shared/shared-interfaces';

@Component({})
export class ChartBaseComponent implements IChart {
    @Input()
    options: IChartOption[];
    @Input()
    data: INameValue[];
    
}
