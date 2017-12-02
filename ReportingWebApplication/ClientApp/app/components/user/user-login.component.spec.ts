import { TestBed, ComponentFixture } from "@angular/core/testing";
import { UserLoginComponent } from "./user-login.component";
import { FormsModule, FormControl, ReactiveFormsModule, Validators, AbstractControl, FormBuilder, FormGroup, FormGroupDirective, FormControlDirective, NgControl } from '@angular/forms';

import { By } from "@angular/platform-browser";
import { DebugElement } from "@angular/core";
import { MaterialModule } from "../material/material-imports.module";
import { UserService } from "./user.service";
import { HttpModule } from "@angular/http";
import { AuthHttp } from "angular2-jwt";
import { AuthModule } from "./auth.module";
import { Router, RouterModule } from "@angular/router";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";


class RouterStub {
    navigate(params: any) {

    }
}

describe('1st tests', () => {
    let comp: UserLoginComponent;
    let fixture: ComponentFixture<UserLoginComponent>;
    let de: DebugElement;
    let el: HTMLElement;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [MaterialModule, FormsModule, HttpModule, AuthModule, BrowserAnimationsModule, RouterModule, ReactiveFormsModule],
            declarations: [UserLoginComponent],
            providers: [UserService, FormControlDirective, FormGroupDirective,
                { provide: Router, useClass: RouterStub }]
        });

        fixture = TestBed.createComponent(UserLoginComponent);

        comp = fixture.componentInstance; 

        fixture.detectChanges();
    });

    // testing that controls are present
    it('login form should contain an email control', () => {

        //de = fixture.debugElement.query(By.css('.ac'));

        //el = de.nativeElement;
        expect(comp.loginform.contains('email')).toBeTruthy();
    });

    it('login form should contain a password control', () => {
        expect(comp.loginform.contains('password')).toBeTruthy();
    });

    it('register form should contain a name control', () => {
        expect(comp.registerform.contains('name')).toBeTruthy();
    });

    it('register form should contain an email control', () => {
        expect(comp.registerform.contains('email')).toBeTruthy();
    });

    it('register form should contain a password confirm control', () => {
        expect(comp.registerform.contains('confirmPassword')).toBeTruthy();
    });

    it('register form should contain a password control', () => {
        expect(comp.registerform.contains('password')).toBeTruthy();
    });

    // login form control invalid tests
    it('email control should be invalid on login form', () => {
        let control = comp.loginform.get('email');
        control!.setValue('');
        fixture.detectChanges();
        expect(control!.valid).toBeFalsy();
    });

    it('password control should be invalid on login form', () => {
        let control = comp.loginform.get('password');
        control!.setValue('');
        fixture.detectChanges();
        expect(control!.valid).toBeFalsy();
    });

    // login form control valid tests
    it('email control should be valid on login form', () => {
        let control = comp.loginform.get('email');
        control!.setValue('admin@admin.com');
        fixture.detectChanges();
        expect(control!.valid).toBeTruthy();
    });

    it('password control should be valid on login form', () => {
        let control = comp.loginform.get('password');
        control!.setValue('jelszo123');
        fixture.detectChanges();
        expect(control!.valid).toBeTruthy();
    });

    // register form control invalid tests
    it('name control should be invalid on register form', () => {
        let control = comp.registerform.get('name');
        control!.setValue('');
        fixture.detectChanges();
        expect(control!.valid).toBeFalsy();
    });

    it('email control should be invalid on register form', () => {
        let control = comp.registerform.get('email');
        control!.setValue('');
        fixture.detectChanges();
        expect(control!.valid).toBeFalsy();
    });

    it('password control should be invalid on register form', () => {
        let control = comp.registerform.get('password');
        control!.setValue('');
        fixture.detectChanges();
        expect(control!.valid).toBeFalsy();
    });

    it('password confirm control should be invalid on register form', () => {
        let control = comp.registerform.get('confirmPassword');
        control!.setValue('');
        fixture.detectChanges();
        expect(control!.valid).toBeFalsy();
    });

    // register form control valid tests

    it('name control should be valid on register form', () => {
        let control = comp.registerform.get('name');
        control!.setValue('Tóth Máté');

        fixture.detectChanges();

        expect(control!.valid).toBeTruthy();
    });

    it('email control should be valid on register form', () => {
        let control = comp.registerform.get('email');
        control!.setValue('admin@admin.com');

        fixture.detectChanges();

        expect(control!.valid).toBeTruthy();
    });

    it('password control should be valid on register form', () => {
        let control = comp.registerform.get('password');
        control!.setValue('jelszo123');

        fixture.detectChanges();

        expect(control!.valid).toBeTruthy();
    });

    // testing password and password confirm control values match
    it('password confirm should be valid', () => {
        let pwdControl = comp.registerform.get('password');
        pwdControl!.setValue('jelszo123');

        let confirmPwdControl = comp.registerform.get('confirmPassword');
        confirmPwdControl!.setValue('jelszo123');

        fixture.detectChanges();

        expect(confirmPwdControl!.valid).toBeTruthy();
    });

    it('password confirm should be invalid', () => {
        let pwdControl = comp.registerform.get('password');
        pwdControl!.setValue('masjelszo123');

        let confirmPwdControl = comp.registerform.get('confirmPassword');
        confirmPwdControl!.setValue('jelszo123');

        fixture.detectChanges();

        expect(confirmPwdControl!.valid).toBeFalsy();
    });

    // register form error tests
    it('register form should have errors', () => {
        let registerFormHasErrors = comp.registerFormHasErrors();

        expect(registerFormHasErrors).toBeTruthy();
    });

    it('register form should have no errors', () => {
        comp.registerform.get('name')!.setValue('Tóth Máté');
        comp.registerform.get('email')!.setValue('admin@admin.com');
        comp.registerform.get('password')!.setValue('jelszo123');
        comp.registerform.get('confirmPassword')!.setValue('jelszo123');

        let registerFormHasErrors = comp.registerFormHasErrors();

        fixture.detectChanges();

        expect(registerFormHasErrors).toBeFalsy();
    });

    // login form error tests
    it('login form should have errors', () => {
        let loginFormHasErrors = comp.loginFormHasErrors();

        expect(loginFormHasErrors).toBeTruthy();
    });

    it('login form should have no errors', () => {
        comp.loginform.get('email')!.setValue('admin@admin.com');
        comp.loginform.get('password')!.setValue('jelszo123');

        let loginFormHasErrors = comp.loginFormHasErrors();

        fixture.detectChanges();

        expect(loginFormHasErrors).toBeFalsy();
    });

    //it('register form should contain a required password confirm control', () => {
    //    let control = comp.registerform.get('confirmPassword');
    //    control!.setValue('');
    //    expect(control!.valid).toBeFalsy();
    //});
    
});


