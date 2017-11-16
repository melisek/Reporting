import { Component, AfterViewInit, Input, ViewChild, ComponentFactoryResolver, OnInit, OnDestroy, ChangeDetectorRef, ElementRef, QueryList, ViewChildren } from '@angular/core';
import { ChartDirective } from './chart.directive';
import { ChartItem } from './chart-item';
import { IChart, IChartOption } from './chart';
import { ChartService } from './chart.service';

import { MatSelectionList } from '@angular/material'; 
import { Observable } from 'rxjs/Observable';

import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/observable/fromEvent';


@Component({
    selector: 'chart-editor',
    templateUrl: './chart-edit.component.html',
    styleUrls: [ './chart-edit.component.css','../shared/shared-styles.css' ]
})
export class ChartEditComponent implements AfterViewInit {
    @Input() chartItem: ChartItem;
    @ViewChild(ChartDirective) chartHost: ChartDirective;
    @ViewChildren('stringOpts') stringOpts: QueryList<ElementRef>;

    chartTypes: any[];
    selectedChartType: number = 0;

    options: IChartOption[];
    boolOptions: any[];
    stringOptions: any[];

    
   

    constructor(
        private componentFactoryResolver: ComponentFactoryResolver,
        private chartService: ChartService,
        private _cdr: ChangeDetectorRef) {
    }

    ngOnInit() {
        this.chartTypes =
            [{
                name: "Horizontal Bar Chart",
                value: 0,
            },
            {
                name: "Vertical Bar Chart",
                value: 1
            },
            {
                name: "Pie Chart",
                value: 2
            },
            {
                name: "Line Chart",
                value: 3
            }];
        this.chartItem = this.chartService.getChart(this.selectedChartType);

        //this.stringOptions = this.chartItem.options.filter(x => x.type === "string");
        
        //this.boolOptions = this.chartItem.options.filter(x => x.type === "boolean");
        console.log(this.chartItem);
    }

    ngAfterViewInit() {
        this.loadComponent();
        //this.stringOpts.forEach(option => {
        //    Observable.fromEvent(option.nativeElement, 'keyup')
        //        .debounceTime(150)
        //        .distinctUntilChanged()
        //        .subscribe(() => {
        //            //this.chartItem.options.find(x => x.name === option.nativeElement.name).value = option.nativeElement.value;
        //            //this.chartItem.options[option.nativeElement.name] = option.nativeElement.value;
        //        });
        //}); 
    }

    chartTypeChange(): void {
        if (this.chartService != null) {
            this.chartItem = this.chartService.getChart(this.selectedChartType);
            this.stringOptions = this.chartItem.options.filter(x => x.type === "string");

            this.boolOptions = this.chartItem.options.filter(x => x.type === "boolean");
            this.loadComponent();
        }
    }

    loadComponent() {

        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.chartItem.component);

        let viewContainerRef = this.chartHost.viewContainerRef;
        viewContainerRef.clear();

        let componentRef = viewContainerRef.createComponent(componentFactory);
        (<IChart>componentRef.instance).options = this.chartItem.options;
        (<IChart>componentRef.instance).data = this.chartItem.data;
        this._cdr.detectChanges();
    }

}