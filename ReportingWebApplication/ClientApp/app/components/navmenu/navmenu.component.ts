import { Component } from '@angular/core';
import { UserService } from '../user/user.service';
import { Router } from '@angular/router';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {
    constructor(private _userService: UserService, private _router: Router) { }

    logout() {
        this._userService.logout();
        this._router.navigate(['./user/login/']);
    }
}
