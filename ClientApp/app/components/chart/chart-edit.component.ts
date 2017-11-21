import { Component, AfterViewInit, Input, ViewChild, ComponentFactoryResolver, OnInit, OnDestroy, ChangeDetectorRef, ElementRef, QueryList, ViewChildren, Output, EventEmitter } from '@angular/core';
import { ChartDirective } from './chart.directive';
import { ChartItem } from './chart-item';
import { IChart, IChartOption, IChartStyle } from './chart';
import { ChartService } from './chart.service';

import { MatSelectionList } from '@angular/material'; 
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/observable/fromEvent';

import { INameValue, IChartDiscreteDataOptions } from '../shared/shared-interfaces';
import { IQueryColumns, IQueryColumn } from '../query/query';

import { chartTypes, aggregationTypes } from './chart-constants'

@Component({
    selector: 'chart-editor',
    templateUrl: './chart-edit.component.html',
    styleUrls: [ './chart-edit.component.css','../shared/shared-styles.css' ]
})
export class ChartEditComponent implements AfterViewInit {
    @Input() chartItem: ChartItem;
    @Input()
    set chartData(chartData: any) {
        this._chartData = chartData;

        if (this._chartData && this.selectedChartType != null)
            this.loadComponent();
    }
    private _queryColumns: IQueryColumns;
    @Input() 
    set queryColumns(queryColumns: IQueryColumns) {
        this._queryColumns = queryColumns;

        if (this._queryColumns) {
            this.queryStringColumns = this._queryColumns.columns.filter(x => x.type === "string");
            console.log(this.queryStringColumns);
            this.queryNumberColumns = this._queryColumns.columns.filter(x => x.type === "number");
            console.log(this.queryNumberColumns);
        }
            
    }

    @Output() chartDataOptionsChange:
        EventEmitter<IChartDiscreteDataOptions> = new EventEmitter<IChartDiscreteDataOptions>();

    @ViewChild(ChartDirective) chartHost: ChartDirective;
    @ViewChildren('stringOpts') stringOpts: QueryList<ElementRef>;

    private _chartData: any;
    get chartData(): any {
        return this._chartData;
    }

    chartTypes: INameValue[];
    selectedChartType: number;

    discreteDataOptions: IChartDiscreteDataOptions;
    aggregationTypes: INameValue[];

    options: IChartOption[];
    boolOptions: any[];
    stringOptions: any[];

    queryStringColumns: IQueryColumn[];
    queryNumberColumns: IQueryColumn[];
    
    constructor(
        private componentFactoryResolver: ComponentFactoryResolver,
        private chartService: ChartService,
        private _cdr: ChangeDetectorRef) {
    }

    ngOnInit() {
        this.chartTypes = chartTypes;
        this.aggregationTypes = aggregationTypes;

        this.discreteDataOptions = {
            reportGUID: "",
            nameColumn: "Table_95_Field_67",
            valueColumn: "Table_95_Field_45",
            aggregation: 0
        };
        //this.chartItem = this.chartService.getChart(this.selectedChartType);

        //this.stringOptions = this.chartItem.options.filter(x => x.type === "string");
        
        //this.boolOptions = this.chartItem.options.filter(x => x.type === "boolean");
        console.log(this.chartItem);
    }

    ngAfterViewInit() {
        //this.loadComponent();
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

    initChartOptions(reportGUID: string): Observable<IChartDiscreteDataOptions> {

        return this.chartService.getChartOptions(reportGUID).map(data => {
            let style = JSON.parse(data.style);
            if (style != null) {
                console.log('style' + style.chartType);
                this.selectedChartType = style.chartType;
                this.discreteDataOptions = style.dataOptions;
                this.chartItem = this.chartService.getChart(this.selectedChartType);
                this.chartItem.options = style.displayOptions;
            }
            
            //this.chartDataOptionChange();
            //this.loadComponent();
            return this.discreteDataOptions;
        });

    }

    chartTypeChange(): void {
        if (this.chartService != null) {
            this.chartItem = this.chartService.getChart(this.selectedChartType);
            
            this.stringOptions = this.chartItem.options.filter(x => x.type === "string");
            
            this.boolOptions = this.chartItem.options.filter(x => x.type === "boolean");
            this.loadComponent();
        }
    }

    chartDataOptionChange() {
        this.chartDataOptionsChange.emit(this.discreteDataOptions);
        this.loadComponent();
    }

    loadComponent() {

        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.chartItem.component);

        let viewContainerRef = this.chartHost.viewContainerRef;
        viewContainerRef.clear();

        let componentRef = viewContainerRef.createComponent(componentFactory);
        (<IChart>componentRef.instance).options = this.chartItem.options;
        console.log('loadcomp:' + this.chartData);
        (<IChart>componentRef.instance).data = this.chartData;
        this._cdr.detectChanges();
    }

    onChartSave(reportGUID: string) {
        if (this.chartService != null) {
            this.chartService.saveChart(this.chartItem, this.selectedChartType, this.discreteDataOptions, reportGUID).subscribe();  
        }
    }

}