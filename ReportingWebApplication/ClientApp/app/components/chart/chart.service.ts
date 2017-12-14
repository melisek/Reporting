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
import { INameValue, ISeriesNameValue, IResponseResult, IChartDiscreteDataOptions, IChartSeriesDataOptions } from '../shared/shared-interfaces';
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

    types: Type<any>[] = [
        HorizontalBarChartComponent,
        VerticalBarChartComponent,
        PieChartComponent,
        LineChartComponent
    ];

    chartTypeOptions: IChartOption[][] = chartTypeOptions;

    arrdata: INameValue[] = [];

    seriesarrdata: ISeriesNameValue[] = [];

    getChart(selected: number) {
        return new ChartItem(this.types[selected], this.chartTypeOptions[selected], selected < 3 ? this.arrdata : this.seriesarrdata); 
    }

    getChartOptions(reportGUID: string): Observable<IChartStyle> {
        return this._http.get(this._getStyleUrl + reportGUID)
            .map(response => response.json() as IChartStyle)
            .do(data => console.log("get style: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    saveChart(chartItem: ChartItem, chartType: number, discreteDataOptions: IChartDiscreteDataOptions,
        seriesDataOptions: IChartSeriesDataOptions, reportGUID: string): Observable<boolean> {
        let opt = {
            chartType: chartType,
            dataOptions: discreteDataOptions || seriesDataOptions,
            displayOptions: chartItem.options
        };
        let style: IChartStyle = {
            reportGUID: discreteDataOptions.reportGUID || seriesDataOptions.reportGUID,
            style: JSON.stringify(opt)
        };
        return this._http.post(this._saveUrl, style)
            .catch(this.handleError);
    }

    private handleError(err: HttpErrorResponse) {
        console.log(err);
        return Observable.throw(err.statusText);
    }
}
