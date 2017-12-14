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
import { ChartEditComponent } from "./chart-edit.component";
import { ChartService } from "./chart.service";


class RouterStub {
    navigate(params: any) {

    }
}
class ActivatedRouteStub {
    params: Observable<any> = new Observable<any>();
    snapshot: ActivatedRouteSnapshot = new ActivatedRouteSnapshot();
}

describe('ChartEditComponent tests', () => {
    let comp: ChartEditComponent;
    let fixture: ComponentFixture<ChartEditComponent>;
    let de: DebugElement;
    let el: HTMLElement;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [MaterialModule, FormsModule, HttpModule, BrowserAnimationsModule, AuthModule, RouterModule, ReactiveFormsModule, ChartModule],
            declarations: [ChartEditComponent],
            providers: [ChartService, QueryService, FormControlDirective, FormGroupDirective,
                { provide: Router, useClass: RouterStub },
                { provide: APP_BASE_HREF, useValue: '/' },
                { provide: ActivatedRoute, useClass: ActivatedRouteStub }]
        });

        fixture = TestBed.createComponent(ChartEditComponent);

        comp = fixture.componentInstance;

        fixture.detectChanges();
    });

    it('save button should be disabled', () => {
        comp.discreteDataOptions = {
            reportGUID: '',
            nameColumn: '',
            valueColumn: '',
            aggregation: 0
        };
        fixture.detectChanges();

        de = fixture.debugElement.query(By.css('.saveButton'));
        expect(de.attributes['disabled']).toBeTruthy();
    });

    it('save button should not be disabled', () => {
        comp.selectedChartType = 1;
        comp.discreteDataOptions = {
            reportGUID: '',
            nameColumn: 'ProductName',
            valueColumn: 'NetValue',
            aggregation: 0
        };
        fixture.detectChanges();

        de = fixture.debugElement.query(By.css('.saveButton'));
        expect(de.attributes['disabled']).toBeTruthy();
    });

});


