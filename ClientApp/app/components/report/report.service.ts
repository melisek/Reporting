import { Injectable } from "@angular/core";
import { Http } from '@angular/http';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';

import { IReportList, IReport, IReportCreate } from "./report";
import { IResponseResult, IListFilter, INameValue, IChartDiscreteDataOptions } from "../shared/shared-interfaces";

@Injectable()
export class ReportService {
    private _listUrl = './api/reports/GetAll';
    private _addUrl = './api/reports/Create';
    private _getUrl = './api/reports/Get/';
    private _deleteUrl = './api/reports/Delete/';
    private _getStyleUrl = './api/reports/GetStyle';
    private _getDiscreteDataUrl = './api/reports/GetDiscreetRiportDiagram';

    constructor(private _http: Http) { }

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

    getReport(reportGUID: string): Observable<IReportCreate> {
        console.log(reportGUID);
        return this._http.get(this._getUrl + reportGUID)
            .map(response => response.json() as IReportCreate)
            .do(data => console.log("Report: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    getStyle(): Observable<IResponseResult> {
        return this._http.get(this._getStyleUrl + "/a4f53c6a-21f8-4fe6-a45c-fb0ceec919e6")
            .map(response => response.json() as IResponseResult)
            .do(data => console.log("get style: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    deleteReport(reportGUID: string): Observable<boolean> {
        return this._http.delete(this._deleteUrl + reportGUID)
            //.map(response => response.ok)
            .do(data => console.log("Delete report: " + JSON.stringify(data)))
            .catch(this.handleError);
    }


    getDiscreteDiagramData(dataOptions: IChartDiscreteDataOptions): Observable<INameValue[]> {
        /*let param = {
            reportGUID: "d75dbdb7-498c-46c2-a18a-9a90519e3a31",
            nameColumn: "Table_95_Field_67",
            valueColumn: "Table_95_Field_119",
            aggregation: 4
        };*/
        console.log(dataOptions);
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
