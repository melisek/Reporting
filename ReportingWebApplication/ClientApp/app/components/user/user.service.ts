import { Injectable } from "@angular/core";
import { Http } from '@angular/http';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import { ILoginResponse, ILoginCredential, IRegisterCredential } from './user';
import { AuthHttp, tokenNotExpired } from 'angular2-jwt';
import { IResponseResult } from "../shared/shared-interfaces";

@Injectable()
export class UserService {
    private _loginUrl = './api/auth/login';
    private _registerUrl = './api/auth/register';
    private _usernameUrl = './api/auth/GetUserName';

    public username: string;

    constructor(private _http: Http, private _authHttp: AuthHttp) { }

    login(credential: ILoginCredential): Observable<boolean> {
        console.log(credential);
        return this._http.post(this._loginUrl, credential)
            .map(response => {
                let token = response.json() && response.json().value.jwt;
                if (token) {
                    localStorage.setItem('token', token);
                    this._authHttp.get(this._usernameUrl)
                        .map(response => {
                            this.username = response.text();
                        }).subscribe();
                    return true;
                } else {
                    return false;
                }
            })
            .catch(this.handleError);
    }

    register(credential: IRegisterCredential): Observable<boolean> {
        console.log(credential);
        return this._http.post(this._registerUrl, credential)
            .map(response => {
                let token = response.json() && response.json().value.jwt;
                if (token) {
                    localStorage.setItem('token', token);
                    this._authHttp.get(this._usernameUrl)
                        .map(response => {
                            this.username = response.text();
                        }).subscribe();
                    return true;
                } else {
                    return false;
                }
            })
            .catch(this.handleError);
    }

    logout(): void {
        localStorage.removeItem('token');
    }

    loggedIn() {
        return tokenNotExpired();
    }

    private handleError(err: HttpErrorResponse) {
        //console.log(err.statusText);
        return Observable.throw(err.statusText);
    }
}
