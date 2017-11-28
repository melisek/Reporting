import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CanActivate } from '@angular/router';
import { UserService } from './user.service';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private _userService: UserService, private router: Router) { }

    canActivate() {
        if (this._userService.loggedIn()) {
            return true;
        } else {
            this.router.navigate(['./user/login']);
            return false;
        }
    }
}