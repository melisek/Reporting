import { Injectable } from "@angular/core";
import { Http } from '@angular/http';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';

import { IDashboard, IDashboardCreate, IDashboardList } from "./dashboard";
import { IResponseResult, IListFilter, INameValue, IChartDiscreteDataOptions } from "../shared/shared-interfaces";
import { AuthHttp } from "angular2-jwt";

@Injectable()
export class DashboardService {
    private _listUrl = './api/dashboards/GetAll';
    private _addUrl = './api/dashboards/Create';
    private _updateUrl = './api/dashboards/Update/';
    private _getUrl = './api/dashboards/GetDashboardReports/';
    private _deleteUrl = './api/dashboards/Delete/';

    constructor(private _http: AuthHttp) { }

    getDashboards(filter: IListFilter): Observable<IDashboardList> {
        console.log(filter);
        return this._http.post(this._listUrl, filter)
            .map(response => response.json() as IDashboardList)
            .do(data => console.log("Dashboards: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    addDashboard(dashboard: IDashboardCreate): Observable<IResponseResult> {
        console.log(dashboard);
        return this._http.post(this._addUrl, dashboard)
            //.map(response => response.json() as IResponseResult)
            //.do(data => console.log("Add report: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    updateDashboard(dashboardGUID: string, dashboard: IDashboardCreate): Observable<IResponseResult> {
        let data = { "dashboardGUID": dashboardGUID, ...dashboard };
        console.log('updatedash:' + JSON.stringify(data));
        return this._http.put(this._updateUrl + dashboardGUID, data)
            //.map(response => response.json() as IResponseResult)
            //.do(data => console.log("Add report: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    getDashboard(dashboardGUID: string): Observable<IDashboardCreate> {
        console.log(dashboardGUID);
        return this._http.get(this._getUrl + dashboardGUID)
            .map(response => response.json() as IDashboardCreate)
            .do(data => console.log("dashboard: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    deleteDashboard(dashboardGUID: string): Observable<boolean> {
        return this._http.delete(this._deleteUrl + dashboardGUID)
            //.map(response => response.ok)
            .do(data => console.log("Delete report: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    private handleError(err: HttpErrorResponse) {
        console.log(err);
        return Observable.throw(err.statusText);
    }
}
