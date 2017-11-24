import { Component } from '@angular/core';
import { FormControl, Validators, AbstractControl, FormBuilder, FormGroup } from '@angular/forms';
import { UserService } from './user.service';

import { PasswordValidator } from './password-validator';
import { ILoginCredential } from './user';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';

@Component({
    templateUrl: './user-login.component.html',
    styleUrls: ['../shared/shared-styles.css']
})
export class UserLoginComponent {
    loginform: FormGroup;
    registerform: FormGroup;

    constructor(private _userService: UserService, private _formBuilder: FormBuilder, private _router: Router, private titleService: Title) {
        this.loginform = _formBuilder.group({
            email: ['', Validators.required],
            password: ['', Validators.required],
        });

        this.registerform = _formBuilder.group({
            email: ['', Validators.required],
            password: ['', Validators.required],
            confirmPassword: ['', Validators.required]
        }, {
                validator: PasswordValidator.MatchPassword
            });

        this.titleService.setTitle("Login - Register");
    }

    onLoginClick() {
        let credential: ILoginCredential = {
            emailAddress: this.loginform.controls.email.value,
            password: this.loginform.controls.password.value
        };
        this._userService.login(credential).subscribe(success => {
            if (success)
                this._router.navigate(['./']);
        });
    }

}