import { Injectable } from "@angular/core";
import { Http } from '@angular/http';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';

import { IReportList, IReport, IReportCreate } from './report';
import { IResponseResult, IListFilter, INameValue, IChartDiscreteDataOptions, IChartSeriesDataOptions, ISeriesNameValue } from '../shared/shared-interfaces';
import { AuthHttp } from 'angular2-jwt';

@Injectable()
export class ReportService {
    private _listUrl = './api/reports/GetAll';
    private _addUrl = './api/reports/Create';
    private _updateUrl = './api/reports/Update/';
    private _getUrl = './api/reports/Get/';
    private _deleteUrl = './api/reports/Delete/';
    private _exportUrl = './api/reports/Export/';
    private _getDiscreteDataUrl = './api/reports/GetDiscreetRiportDiagram';
    private _getSeriesDataUrl = './api/reports/GetSeriesRiportDiagram';

    constructor(private _http: AuthHttp) { }

    getReports(filter: IListFilter): Observable<IReportList> {
        return this._http.post(this._listUrl, filter)
            .map(response => response.json() as IReportList)
            .catch(this.handleError);
    }

    addReport(report: IReportCreate): Observable<IResponseResult> {
        return this._http.post(this._addUrl, report)
            .catch(this.handleError);
    }

    updateReport(reportGUID: string, report: IReportCreate): Observable<IResponseResult> {
        let data = { "reportGUID": reportGUID, ...report };
        return this._http.put(this._updateUrl + reportGUID, data)
            .catch(this.handleError);
    }

    getReport(reportGUID: string): Observable<IReportCreate> {
        return this._http.get(this._getUrl + reportGUID)
            .map(response => response.json() as IReportCreate)
            .catch(this.handleError);
    }

    deleteReport(reportGUID: string): Observable<boolean> {
        return this._http.delete(this._deleteUrl + reportGUID)
            .catch(this.handleError);
    }

    exportReport(reportGUID: string): Observable<Response> {
        return this._http.get(this._exportUrl + reportGUID)
            .catch(this.handleError);
    }

    getDiscreteDiagramData(dataOptions: IChartDiscreteDataOptions): Observable<INameValue[]> {
        return this._http.post(this._getDiscreteDataUrl, dataOptions)
            .map(response => response.json() as INameValue[])
            .catch(this.handleError);
    }

    getSeriesDiagramData(dataOptions: IChartSeriesDataOptions): Observable<ISeriesNameValue[]> {
        return this._http.post(this._getSeriesDataUrl, dataOptions)
            .map(response => response.json() as ISeriesNameValue[])
            .catch(this.handleError);
    }

    private handleError(err: HttpErrorResponse) {
        console.log(err);
        return Observable.throw(err.statusText);
    }
}
