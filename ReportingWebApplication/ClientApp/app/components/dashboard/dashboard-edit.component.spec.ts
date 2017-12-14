import { TestBed, ComponentFixture } from "@angular/core/testing";
import { FormsModule, FormControl, ReactiveFormsModule, Validators, AbstractControl, FormBuilder, FormGroup, FormGroupDirective, FormControlDirective, NgControl } from '@angular/forms';

import { By } from "@angular/platform-browser";
import { DebugElement } from "@angular/core";
import { MaterialModule } from "../material/material-imports.module";
import { HttpModule } from "@angular/http";
import { AuthHttp } from "angular2-jwt";
import { Router, RouterModule, ActivatedRoute, ActivatedRouteSnapshot } from "@angular/router";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { ChartModule } from "../chart/chart.module";
import { AuthModule } from "../user/auth.module";
import { QueryService } from "../query/query.service";
import { APP_BASE_HREF } from "@angular/common";
import { Observable } from "rxjs/Observable";
import { DashboardEditComponent } from "./dashboard-edit.component";
import { DashboardService } from "./dashboard.service";
import { ChartService } from "../chart/chart.service";
import { ReportService } from "../report/report.service";
import { NgxDnDModule } from "@swimlane/ngx-dnd";


class RouterStub {
    navigate(params: any) {

    }
}
class ActivatedRouteStub {
    params: Observable<any> = new Observable<any>();
    snapshot: ActivatedRouteSnapshot = new ActivatedRouteSnapshot();
}

describe('DashboardEditComponent tests', () => {
    let comp: DashboardEditComponent;
    let fixture: ComponentFixture<DashboardEditComponent>;
    let de: DebugElement;
    let el: HTMLElement;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [MaterialModule, FormsModule, HttpModule, BrowserAnimationsModule, AuthModule, RouterModule, ReactiveFormsModule, ChartModule, NgxDnDModule],
            declarations: [DashboardEditComponent],
            providers: [DashboardService, ReportService, ChartService, FormControlDirective, FormGroupDirective,
                { provide: Router, useClass: RouterStub },
                { provide: APP_BASE_HREF, useValue: '/' },
                { provide: ActivatedRoute, useClass: ActivatedRouteStub }]
        });

        fixture = TestBed.createComponent(DashboardEditComponent);

        comp = fixture.componentInstance;

        fixture.detectChanges();
    });

    it('name control should be invalid on the form', () => {
        let control = comp.form.get('name');
        control!.setValue('');
        fixture.detectChanges();
        expect(control!.valid).toBeFalsy();
    });

    it('name control should be valid on the form', () => {
        let control = comp.form.get('name');
        control!.setValue('Jelentés 1');
        fixture.detectChanges();
        expect(control!.valid).toBeTruthy();
    });

    it('save button should be disabled', () => {
        let control = comp.form.get('name');
        control!.setValue('');
        comp.targetItems = [];
        fixture.detectChanges();

        de = fixture.debugElement.query(By.css('.saveButton'));
        expect(de.attributes['disabled']).toBeTruthy();
    });

    it('save button should not be disabled', () => {
        let control = comp.form.get('name');
        control!.setValue('Dashboard 1');
        comp.targetItems[0] = ['report1'];

        fixture.detectChanges();

        de = fixture.debugElement.query(By.css('.saveButton'));
        expect(de.attributes['disabled']).toBeTruthy();
    });

});


