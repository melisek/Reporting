import { Injectable } from "@angular/core";
import { Http } from '@angular/http';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';

import { IReportList, IReport, IReportCreate } from './report';
import { IResponseResult, IListFilter, INameValue, IChartDiscreteDataOptions } from '../shared/shared-interfaces';
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

    constructor(private _http: AuthHttp) { }

    getReports(filter: IListFilter): Observable<IReportList> {
        console.log(filter);
        return this._http.post(this._listUrl, filter)
            .map(response => response.json() as IReportList)
            .do(data => console.log("Reports: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    addReport(report: IReportCreate): Observable<IResponseResult> {
        console.log(report);
        return this._http.post(this._addUrl, report)
            //.map(response => response.json() as IResponseResult)
            //.do(data => console.log("Add report: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    updateReport(reportGUID: string, report: IReportCreate): Observable<IResponseResult> {
        
        let data = { "reportGUID": reportGUID, ...report };
        console.log('updatereport:' + JSON.stringify(data));
        return this._http.put(this._updateUrl + reportGUID, data)
            //.map(response => response.json() as IResponseResult)
            //.do(data => console.log("Add report: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    getReport(reportGUID: string): Observable<IReportCreate> {
        console.log(reportGUID);
        return this._http.get(this._getUrl + reportGUID)
            .map(response => response.json() as IReportCreate)
            .do(data => console.log("Report: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    

    deleteReport(reportGUID: string): Observable<boolean> {
        return this._http.delete(this._deleteUrl + reportGUID)
            //.map(response => response.ok)
            .do(data => console.log("Delete report: " + JSON.stringify(data)))
            .catch(this.handleError);
    }


    exportReport(reportGUID: string): Observable<Response> {
        return this._http.get(this._exportUrl + reportGUID)
            //.map(response => response)
            .do(data => console.log("Export report: " + JSON.stringify(data)))
            .catch(this.handleError);
    }


    getDiscreteDiagramData(dataOptions: IChartDiscreteDataOptions): Observable<INameValue[]> {
        console.log('disrete' + dataOptions.reportGUID);
        return this._http.post(this._getDiscreteDataUrl, dataOptions)
            .map(response => response.json() as INameValue[])
            .do(data => console.log("get diagram data: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    private handleError(err: HttpErrorResponse) {
        console.log(err);
        return Observable.throw(err.statusText);
    }
}
