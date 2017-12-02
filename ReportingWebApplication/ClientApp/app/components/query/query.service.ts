import { Injectable } from "@angular/core";
import { Http } from '@angular/http';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';

import { IResponseResult, IEntityWithIdName } from '../shared/shared-interfaces';
import { IQueryColumns, IQuery, IQuerySourceDataFilter, IQuerySourceData, IQueryColumn } from "./query";
import { AuthHttp } from "angular2-jwt";

@Injectable()
export class QueryService {
    private _idNameUrl = './api/queries/GetAll';
    private _columnUrl = './api/queries/GetQueryColumns/';
    private _sourceDataUrl = './api/queries/GetQuerySource';
    

    constructor(private _http: AuthHttp) { }

    getQueriesIdName(): Observable<IQuery[]> {
        return this._http.get(this._idNameUrl)
            .map(response => response.json() as IQuery[])
            .do(data => console.log("Queries: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    getQueryColumns(guid: string): Observable<IQueryColumn[]> {
        console.log(guid);
        return this._http.get(this._columnUrl + guid)
            .map(response => {
                let queryColumns = response.json() as IQueryColumns;
                return queryColumns.columns;
            })
            .do(data => console.log(`Query: ${guid} ${JSON.stringify(data)}`))
            .catch(this.handleError);
    }

    getQuerySourceData(filter: IQuerySourceDataFilter): Observable<IQuerySourceData> {
        console.log(filter);
        return this._http.post(this._sourceDataUrl, filter)
            .map(response => response.json() as IQuerySourceData)
            .do(data => console.log(`Query source data: ${filter.queryGUID} ${JSON.stringify(data)}`))
            .catch(this.handleError);
    }

    private handleError(err: HttpErrorResponse) {
        console.log(err.statusText);
        return Observable.throw(err.statusText);
    }
}
