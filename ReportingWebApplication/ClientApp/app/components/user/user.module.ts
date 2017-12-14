import { NgModule } from '@angular/core';

import { AppModuleShared } from '../../app.module.shared';
import { UserLoginComponent } from './user-login.component';
import { RouterModule } from '@angular/router';
import { UserService } from './user.service';

@NgModule({
    declarations: [
        UserLoginComponent
    ],
    imports: [
        RouterModule.forChild([
            {
                path: 'user/login',
                component: UserLoginComponent
            },
            {
                path: 'user/logout',
                component: UserLoginComponent
            }
        ]),
        AppModuleShared
    ],
    providers: [
        UserService
    ]
})
export class UserModule {
}
