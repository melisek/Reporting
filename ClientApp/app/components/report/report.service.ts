import { Injectable } from "@angular/core";
import { Http } from '@angular/http';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';

import { IReportList } from "./report";
import { IResponseResult } from "../shared/shared-interfaces";

@Injectable()
export class ReportService {
    private _reportsUrl = './api/reports.json';
    private _deleteUrl = './api/delete/';


    constructor(private _http: Http) { }

    getReports(sort: string, order: string, page: number): Observable<IReportList> {
        return this._http.get(this._reportsUrl)
            .map(response => response.json() as IReportList)
            .do(data => console.log("Reports: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    deleteReport(id: number): Observable<IResponseResult> {
        return this._http.get(this._deleteUrl + id)
            .map(response => response.json() as IResponseResult)
            .do(data => console.log("Delete report: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    private handleError(err: HttpErrorResponse) {
        console.log(err.message);
        return Observable.throw(err.message);
    }
}
