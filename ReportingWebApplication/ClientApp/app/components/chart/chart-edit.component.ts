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
        console.log('chartdataset.start');
        if (this._chartData)//&& this.selectedChartType != null) {
        //this.loadComponent();
        {
            console.log('chartdataset.if');
            this.chartTypeChange();
        }
    }
    private _queryColumns: IQueryColumn[];
    @Input() 
    set queryColumns(queryColumns: IQueryColumn[]) {
        this._queryColumns = queryColumns;

        if (this._queryColumns) {
            this.queryStringColumns = this._queryColumns.filter(x => x.type === "string");
            console.log(this.queryStringColumns);
            this.queryNumberColumns = this._queryColumns.filter(x => x.type === "number");
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
    selectedChartType: number = 0;

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
            nameColumn: "",
            valueColumn: "",
            aggregation: 0
        };
        //this.chartItem = this.chartService.getChart(this.selectedChartType);

        //this.stringOptions = this.chartItem.options.filter(x => x.type === "string");
        
        //this.boolOptions = this.chartItem.options.filter(x => x.type === "boolean");
        console.log(this.chartItem);
    }

    //xAxisLabel: ElementRef;
    //yAxisLabel: ElementRef;

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
        console.log('reportguid:chart-edit' + reportGUID);
        return this.chartService.getChartOptions(reportGUID).map(data => {
            let style = JSON.parse(data.style);
            if (style != null) {
                console.log('style' + style.chartType);
                this.selectedChartType = style.chartType;
                this.discreteDataOptions = style.dataOptions;
                this.discreteDataOptions.reportGUID = reportGUID;
                this.chartItem = this.chartService.getChart(this.selectedChartType);
                this.chartItem.options = style.displayOptions;
                
            }
            
            //this.chartDataOptionChange();
            //this.loadComponent();
            return this.discreteDataOptions;
        });

    }

    chartTypeChange(): void {
        console.log('charttypechange.start');
        if (this.chartService != null && this.discreteDataOptions.nameColumn != '' &&
            this.discreteDataOptions.valueColumn != '' && this._chartData /*&& this.selectedChartType != null*/) {
            this.chartItem = this.chartService.getChart(this.selectedChartType);
            console.log('charttypechange.if');
            this.stringOptions = this.chartItem.options.filter(x => x.type === "string");
            
            this.boolOptions = this.chartItem.options.filter(x => x.type === "boolean");
            this.loadChart();
        }
    }

    chartDataOptionChange() {

        

        console.log('chartdataoptionchange.start');
        if (this.discreteDataOptions.nameColumn != '' &&
            this.discreteDataOptions.valueColumn != '' &&
            this.selectedChartType != null
        ) {
            console.log('chartdataoptionchange.emit');
            this.chartDataOptionsChange.emit(this.discreteDataOptions);
            
            if (this.chartItem != null) {
                console.log('chartdataoptionchange.if');
                //this.xAxisLabel = this.stringOpts.find(x => x.nativeElement.name == 'xAxisLabel')!;
                //this.yAxisLabel = this.stringOpts.find(x => x.nativeElement.name == 'yAxisLabel')!;

                //this.stringOpts.forEach(x => console.log(x));
                //console.log('xAxisLabel' + this.xAxisLabel);

                //this.discreteDataOptions.nameColumn != '' ? this.xAxisLabel.nativeElement.value = this.queryStringColumns.find(x => x.name === this.discreteDataOptions.nameColumn)!.text : '';
                //this.discreteDataOptions.valueColumn != '' ? this.yAxisLabel.nativeElement.value = this.queryStringColumns.find(x => x.name === this.discreteDataOptions.valueColumn)!.text : '';
                this.loadChart();
            }

            else {
                console.log('chartdataoptionchange.else');
                this.chartTypeChange();
            }
                
        }
    }

    loadChart() {

        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.chartItem.component);

        let viewContainerRef = this.chartHost.viewContainerRef;
        viewContainerRef.clear();

        let componentRef = viewContainerRef.createComponent(componentFactory);
        (<IChart>componentRef.instance).options = this.chartItem.options;
        (<IChart>componentRef.instance).data = this.chartData;
        this._cdr.detectChanges();
    }

    onChartSave(reportGUID: string) {
        if (this.chartService != null) {
            this.chartService.saveChart(this.chartItem, this.selectedChartType, this.discreteDataOptions, reportGUID).subscribe();  
        }
    }

}