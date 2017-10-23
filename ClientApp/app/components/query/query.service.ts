import { Injectable } from "@angular/core";
import { Http } from '@angular/http';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';

import { IResponseResult, IEntityWithIdName } from '../shared/shared-interfaces';
import { IQueryColumns } from "./query";

@Injectable()
export class QueryService {
    private _idNameUrl = './api/queries-idname.json';
    private _columnUrl = './api/query-columns.json';

    constructor(private _http: Http) { }

    getQueriesIdName(): Observable<IEntityWithIdName[]> {
        return this._http.get(this._idNameUrl)
            .map(response => response.json().queries as IEntityWithIdName[])
            .do(data => console.log("Queries: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    getQueryColumns(id: number): Observable<IQueryColumns> {
        console.log(id);
        return this._http.get(this._columnUrl)
            .map(response => response.json() as IQueryColumns)
            .do(data => console.log(`Query: ${id} ${JSON.stringify(data)}`))
            .catch(this.handleError);
    }

    private handleError(err: HttpErrorResponse) {
        console.log(err.message);
        return Observable.throw(err.message);
    }
}
