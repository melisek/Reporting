import { Injectable, Type } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Http } from '@angular/http';

import { HorizontalBarChartComponent } from './types/chart-horizontal-bar.component';
import { VerticalBarChartComponent } from './types/chart-vertical-bar.component';
import { PieChartComponent } from './types/chart-pie.component';
import { LineChartComponent } from './types/chart-line.component';

import { ChartItem } from './chart-item';
import { colorSets } from '@swimlane/ngx-charts/release/utils'
import { chartTypeOptions } from './chart-constants'

import { IChartOption, IChartStyle } from './chart';
import { INameValue, ISeriesNameValue, IResponseResult, IChartDiscreteDataOptions } from '../shared/shared-interfaces';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import { AuthHttp } from 'angular2-jwt';

@Injectable()
export class ChartService {

    private _saveUrl = './api/reports/UpdateStyle';
    private _getStyleUrl = './api/reports/GetStyle/';

    constructor(private _http: AuthHttp) { }

    colorSets: any;
    types: Type<any>[] = [
        HorizontalBarChartComponent,
        VerticalBarChartComponent,
        PieChartComponent,
        LineChartComponent
    ];

    chartTypeOptions: IChartOption[][] = chartTypeOptions;

    arrdata: INameValue[] = [ ];

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

    getChartOptions(reportGUID: string): Observable<IChartStyle> {
        return this._http.get(this._getStyleUrl + reportGUID)
            .map(response => response.json() as IChartStyle)
            .do(data => console.log("get style: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    saveChart(chartItem: ChartItem, chartType: number, discreteDataOptions: IChartDiscreteDataOptions, reportGUID: string): Observable<boolean> {
        let opt = {
            chartType: chartType,
            dataOptions: discreteDataOptions,
            displayOptions: chartItem.options
        };
        let style: IChartStyle = {
            reportGUID: discreteDataOptions.reportGUID,
            style: JSON.stringify(opt)
        };
        console.log('save chart init ' + JSON.stringify(style));
        return this._http.post(this._saveUrl, style)
            //.map(response => response.json() as IResponseResult)
            .do(data => console.log("save chart: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    private handleError(err: HttpErrorResponse) {
        console.log(err);
        return Observable.throw(err.statusText);
    }
}
