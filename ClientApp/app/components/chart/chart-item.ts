import { Type } from '@angular/core';
import { IChartOption } from './chart';
import { INameValue } from '../shared/shared-interfaces';

export class ChartItem {
    constructor(public component: Type<any>, public options: IChartOption[], public data: INameValue[]) {
        
    }
} 
