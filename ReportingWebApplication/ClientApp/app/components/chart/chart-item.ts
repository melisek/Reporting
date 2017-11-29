import { Type } from '@angular/core';
import { IChartOption } from './chart';

export class ChartItem {
    constructor(public component: Type<any>, public options: IChartOption[], public data: any) {
        console.log(this.data);
    }
} 
