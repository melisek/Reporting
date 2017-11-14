import { Injectable } from "@angular/core";
import { Http } from '@angular/http';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';

import { IResponseResult, IEntityWithIdName } from '../shared/shared-interfaces';
import { IQueryColumns, IQuery } from "./query";

@Injectable()
export class QueryService {
    private _idNameUrl = './api/queries/GetAll';
    private _columnUrl = './api/queries/GetQueryColumns/';

    constructor(private _http: Http) { }

    getQueriesIdName(): Observable<IQuery[]> {
        return this._http.get(this._idNameUrl)
            .map(response => response.json() as IQuery[])
            .do(data => console.log("Queries: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    getQueryColumns(guid: string): Observable<IQueryColumns> {
        console.log(guid);
        return this._http.get(this._columnUrl + guid)
            .map(response => response.json() as IQueryColumns)
            .do(data => console.log(`Query: ${guid} ${JSON.stringify(data)}`))
            .catch(this.handleError);
    }

    private handleError(err: HttpErrorResponse) {
        console.log(err.message);
        return Observable.throw(err.message);
    }
}
