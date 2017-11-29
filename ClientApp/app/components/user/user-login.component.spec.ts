import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { UserLoginComponent } from './user-login.component';
import { DebugElement } from '@angular/core';
import { By, Title } from '@angular/platform-browser';
import { UserService } from './user.service';

describe('UserLoginComponent', () => {

    var component: UserLoginComponent;
    let service: UserService; 

    // synchronous beforeEach
    //beforeEach(() => {
    //    service = new UserService(null, null);
    //    component = new UserLoginComponent(service,)
    //});

    //it('should display original title', () => {

    //    expect(el.attributes.getNamedItem('disabled').value).toBe('disabled');
    //});

    //it('should display original title', () => {
    //    fixture.detectChanges();
    //    expect(el.attributes.getNamedItem('disabled').value).toBe('disabled');
    //});

});


