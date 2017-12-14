import { Component } from '@angular/core';
import { FormControl, Validators, AbstractControl, FormBuilder, FormGroup } from '@angular/forms';
import { UserService } from './user.service';

import { PasswordValidator } from './password-validator';
import { ILoginCredential, IRegisterCredential } from './user';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';

@Component({
    templateUrl: './user-login.component.html',
    styleUrls: ['../shared/shared-styles.css']
})
export class UserLoginComponent {
    loginform: FormGroup;
    registerform: FormGroup;

    loginError: boolean;
    registerError: boolean;

    constructor(private _userService: UserService, private _formBuilder: FormBuilder, private _router: Router, private titleService: Title) {
        this.loginform = _formBuilder.group({
            email: ['', Validators.required],
            password: ['', Validators.required],
        });

        this.registerform = _formBuilder.group({
            name: ['', Validators.required],
            email: ['', Validators.required],
            password: ['', Validators.required],
            confirmPassword: ['', Validators.required]
        }, {
                validator: PasswordValidator.MatchPassword
            });

        this.titleService.setTitle("Login - Register");
    }

    loginFormHasErrors(): boolean {
        return this.loginform.controls.email.hasError('required') ||
            this.loginform.controls.password.hasError('required') ||
            this.loginform.controls.email.hasError('pattern');
    }

    registerFormHasErrors(): boolean {
        return this.registerform.controls.email.hasError('required') ||
            this.registerform.controls.name.hasError('required') ||
            this.registerform.controls.password.hasError('required') ||
            this.registerform.controls.confirmPassword.hasError('required') ||
            this.registerform.controls.confirmPassword.hasError('MatchPassword') ||
            this.registerform.controls.email.hasError('pattern');
    }

    onLoginClick() {
        let credential: ILoginCredential = {
            emailAddress: this.loginform.controls.email.value,
            password: this.loginform.controls.password.value
        };
        this._userService.login(credential).subscribe(success => {
            if (success)
                this._router.navigate(['./']);
        },
            err => this.loginError = true);
    }

    onRegisterClick() {
        let credential: IRegisterCredential = {
            name: this.registerform.controls.name.value,
            emailAddress: this.registerform.controls.email.value,
            password: this.registerform.controls.password.value
        };
        this._userService.register(credential).subscribe(success => {
            if (success)
                this._router.navigate(['./']);
        },
            err => this.registerError = true);
    }

}