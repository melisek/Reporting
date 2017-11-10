import { Injectable, Type } from '@angular/core';

import { HorizontalBarChartComponent } from './chart-horizontal-bar.component';
import { VerticalBarChartComponent } from './chart-vertical-bar.component';
import { PieChartComponent } from './chart-pie.component';

import { ChartItem } from './chart-item';
import { colorSets } from '@swimlane/ngx-charts/release/utils'
import { IChartOption } from './chart';
import { INameValue } from '../shared/shared-interfaces';


@Injectable()
export class ChartService {
    colorSets: any;
    types: Type<any>[] = [
        HorizontalBarChartComponent,
        VerticalBarChartComponent,
        PieChartComponent
    ];

    
    // view?
    baseOptions: IChartOption[] = [
        { name: "animations", text: "Enable animations", type: "boolean", value: true },
        { name: "showLegend", text: "Show legend", type: "boolean", value: true },
        { name: "legendTitle", text: "Legend title", type: "string", value: "Legend" },
        { name: "gradient", text: "Gradient", type: "boolean", value: true },
        { name: "tooltipDisabled", text: "Disable tooltip", type: "boolean", value: false }
        //scheme: { domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA'] }
    ];

    barOptions: IChartOption[] = [
        { name: "showXAxis", text: "Show X-axis", type: "boolean", value: true },
        { name: "showYAxis", text: "Show Y-axis", type: "boolean", value: true },
        { name: "showGridLines", text: "Show grid lines", type: "boolean", value: true },
        { name: "roundDomains", text: "Round domains", type: "boolean", value: true },
        { name: "showXAxisLabel", text: "Show X-axis label", type: "boolean", value: true },
        { name: "showYAxisLabel", text: "Show Y-axis label", type: "boolean", value: true },
        { name: "xAxisLabel", text: "X-axis label", type: "string", value: "x" },
        { name: "yAxisLabel", text: "Y-axis label", type: "string", value: "y" },
        { name: "schemeType", text: "Scheme type", type: "string", value: "ordinal" },
        { name: "axisMax", text: "Axis maximum", type: "number", value: 100 },
    ];

    pieOptions: IChartOption[] = [
        { name: "explodeSlices", text: "Explode slices", type: "boolean", value: false },
        { name: "doughnut", text: "Doughnut", type: "boolean", value: false },
        { name: "labels", text: "Show labels", type: "boolean", value: true }
    ];

    chartTypeOptions: IChartOption[][] = [
        [...this.barOptions, ...this.baseOptions],
        [...this.barOptions, ...this.baseOptions],
        [...this.pieOptions, ...this.baseOptions],
    ];

    arrdata: INameValue[] = [
        {
            "name": "Germany",
            "value": 8940000
        },
        {
            "name": "USA",
            "value": 5000000
        },
        {
            "name": "France",
            "value": 7200000
        }
    ];

    getChart(selected: number) {
        console.log(selected);
        //console.log(this.arrdata);
        return new ChartItem(this.types[selected], this.chartTypeOptions[selected], this.arrdata); 
    }

}
