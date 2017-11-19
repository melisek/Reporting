import { Injectable, Type } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Http } from '@angular/http';

import { HorizontalBarChartComponent } from './types/chart-horizontal-bar.component';
import { VerticalBarChartComponent } from './types/chart-vertical-bar.component';
import { PieChartComponent } from './types/chart-pie.component';
import { LineChartComponent } from './types/chart-line.component';

import { ChartItem } from './chart-item';
import { colorSets } from '@swimlane/ngx-charts/release/utils'
import { IChartOption } from './chart';
import { INameValue, ISeriesNameValue } from '../shared/shared-interfaces';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';



@Injectable()
export class ChartService {

    private _saveUrl = './api/reports/UpdateStyle';

    constructor(private _http: Http) { }

    colorSets: any;
    types: Type<any>[] = [
        HorizontalBarChartComponent,
        VerticalBarChartComponent,
        PieChartComponent,
        LineChartComponent
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
        { name: "axisMax", text: "Axis maximum", type: "number", value: 10 },
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
        [...this.barOptions, ...this.baseOptions],
    ];

    arrdata: INameValue[] = [
        /*{
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
        }*/
    ];

    seriesarrdata: ISeriesNameValue[] = [
        {
            name: "Brunei Darussalam",
            series: [
                {
                    value: 2743,
                    name: "2016-09-21T00:30:01.431Z"
                },
                {
                    value: 4184,
                    name: "2016-09-22T01:53:25.763Z"
                },
                {
                    value: 6839,
                    name: "2016-09-23T18:09:28.859Z"
                },
                {
                    value: 2767,
                    name: "2016-09-15T13:48:57.935Z"
                },
                {
                    value: 2777,
                    name: "2016-09-23T08:40:00.582Z"
                }
            ]
        },
        {
            name: "Iran",
            series: [
                {
                    value: 3780,
                    name: "2016-09-21T00:30:01.431Z"
                },
                {
                    value: 5523,
                    name: "2016-09-22T01:53:25.763Z"
                },
                {
                    value: 3737,
                    name: "2016-09-23T18:09:28.859Z"
                },
                {
                    value: 4398,
                    name: "2016-09-15T13:48:57.935Z"
                },
                {
                    value: 2862,
                    name: "2016-09-23T08:40:00.582Z"
                }
            ]
        },
        {
            name: "Cambodia",
            series: [
                {
                    value: 3073,
                    name: "2016-09-21T00:30:01.431Z"
                },
                {
                    value: 6583,
                    name: "2016-09-22T01:53:25.763Z"
                },
                {
                    value: 2340,
                    name: "2016-09-23T18:09:28.859Z"
                },
                {
                    value: 5742,
                    name: "2016-09-15T13:48:57.935Z"
                },
                {
                    value: 3696,
                    name: "2016-09-23T08:40:00.582Z"
                }
            ]
        },
        {
            name: "Mauritania",
            series: [
                {
                    value: 2054,
                    name: "2016-09-21T00:30:01.431Z"
                },
                {
                    value: 3892,
                    name: "2016-09-22T01:53:25.763Z"
                },
                {
                    value: 5643,
                    name: "2016-09-23T18:09:28.859Z"
                },
                {
                    value: 2861,
                    name: "2016-09-15T13:48:57.935Z"
                },
                {
                    value: 5372,
                    name: "2016-09-23T08:40:00.582Z"
                }
            ]
        },
        {
            name: "India",
            series: [
                {
                    value: 4485,
                    name: "2016-09-21T00:30:01.431Z"
                },
                {
                    value: 3630,
                    name: "2016-09-22T01:53:25.763Z"
                },
                {
                    value: 2786,
                    name: "2016-09-23T18:09:28.859Z"
                },
                {
                    value: 2996,
                    name: "2016-09-15T13:48:57.935Z"
                },
                {
                    value: 4148,
                    name: "2016-09-23T08:40:00.582Z"
                }
            ]
        }
    ];

    getChart(selected: number) {
        console.log(selected);
        //console.log(this.arrdata);
        return new ChartItem(this.types[selected], this.chartTypeOptions[selected], selected < 3 ? this.arrdata : this.seriesarrdata); 
    }

    //saveChart(chartItem: ChartItem): Observable<INameValue[]> {
    //    let style = {
    //        reportGUID: "abc",
    //        style: {
    //            component: chartItem.component,
    //            ...chartItem.options
    //            }
    //    };
    //    return this._http.post(this._saveUrl, style)
    //        .map(response => response.json() as INameValue[])
    //        .do(data => console.log("get diagram data: " + JSON.stringify(data)))
    //        .catch(this.handleError);
    //}

    private handleError(err: HttpErrorResponse) {
        console.log(err);
        return Observable.throw(err.statusText);
    }
}
