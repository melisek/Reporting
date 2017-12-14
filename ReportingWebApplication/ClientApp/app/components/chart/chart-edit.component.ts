import { Component, AfterViewInit, Input, ViewChild, ComponentFactoryResolver, OnInit, OnDestroy, ChangeDetectorRef, ElementRef, QueryList, ViewChildren, Output, EventEmitter } from '@angular/core';
import { ChartDirective } from './chart.directive';
import { ChartItem } from './chart-item';
import { IChart, IChartOption, IChartStyle } from './chart';
import { ChartService } from './chart.service';

import { MatSelectionList, MatSnackBar } from '@angular/material'; 
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/observable/fromEvent';

import { INameValue, IChartDiscreteDataOptions, IChartSeriesDataOptions } from '../shared/shared-interfaces';
import { IQueryColumns, IQueryColumn } from '../query/query';

import { chartTypes, aggregationTypes } from './chart-constants';
//import * as SvgSaver from 'svgsaver/svgsaver';
const SvgSaver = require('svgsaver');

@Component({
    selector: 'chart-editor',
    templateUrl: './chart-edit.component.html',
    styleUrls: ['./chart-edit.component.css', '../shared/shared-styles.css']
})
export class ChartEditComponent implements OnInit {
    @Input() chartItem: ChartItem;
    @Input()
    set chartData(chartData: any) {
        this._chartData = chartData;
        if (this._chartData) {
            this.chartTypeChange();
        }
    }
    private _queryColumns: IQueryColumn[];
    @Input()
    set queryColumns(queryColumns: IQueryColumn[]) {
        this._queryColumns = queryColumns;

        if (this._queryColumns) {
            this.queryStringColumns = this._queryColumns.filter(x => x.type === "string");
            this.queryNumberColumns = this._queryColumns.filter(x => x.type === "number");
            this.queryDateColumns = this._queryColumns.filter(x => x.type === "date");
        }

    }

    @Output() chartDiscreteDataOptionsChange:
    EventEmitter<IChartDiscreteDataOptions> = new EventEmitter<IChartDiscreteDataOptions>();

    @Output() chartSeriesDataOptionsChange:
    EventEmitter<IChartSeriesDataOptions> = new EventEmitter<IChartSeriesDataOptions>();

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

    seriesDataOptions: IChartSeriesDataOptions;

    options: IChartOption[];
    boolOptions: any[];
    stringOptions: any[];

    queryStringColumns: IQueryColumn[];
    queryNumberColumns: IQueryColumn[];
    queryDateColumns: IQueryColumn[];

    constructor(
        private componentFactoryResolver: ComponentFactoryResolver,
        private chartService: ChartService,
        private _snackbar: MatSnackBar,
        private _cdr: ChangeDetectorRef) {
    }

    svgSaver = SvgSaver;

    ngOnInit() {
        this.chartTypes = chartTypes;
        this.aggregationTypes = aggregationTypes;

        this.discreteDataOptions = {
            reportGUID: "",
            nameColumn: "",
            valueColumn: "",
            aggregation: 0
        };
        this.seriesDataOptions = {
            reportGUID: "",
            nameColumn: "",
            seriesNameColumn: "",
            valueColumn: ""
        };
        console.log(this.chartItem);
    }

    initChartOptions(reportGUID: string): Observable<IChartDiscreteDataOptions> {
        return this.chartService.getChartOptions(reportGUID).map(data => {
            let style = JSON.parse(data.style);
            if (style != null) {
                this.selectedChartType = style.chartType;
                this.discreteDataOptions = style.dataOptions;
                this.discreteDataOptions.reportGUID = reportGUID;
                this.chartItem = this.chartService.getChart(this.selectedChartType);
                this.chartItem.options = style.displayOptions;

            }
            return this.discreteDataOptions;
        });
    }

    chartTypeChange(): void {
        if (this.chartService != null && this.selectedChartType < 3 && this.discreteDataOptions.nameColumn != '' &&
            this.discreteDataOptions.valueColumn != '' && this._chartData) {
            this.chartItem = this.chartService.getChart(this.selectedChartType);

            this.stringOptions = this.chartItem.options.filter(x => x.type === "string");
            this.boolOptions = this.chartItem.options.filter(x => x.type === "boolean");
            this.loadChart();
        }
        else if (this.chartService != null && this.selectedChartType == 3 && this.seriesDataOptions.nameColumn != '' &&
            this.seriesDataOptions.seriesNameColumn != '' &&
            this.seriesDataOptions.valueColumn != '' && this._chartData) {
            this.chartItem = this.chartService.getChart(this.selectedChartType);
            this.loadChart();
        }
    }

    chartDataOptionChange(dataType: number) {
        if (dataType === 0 &&
            this.discreteDataOptions.nameColumn != '' &&
            this.discreteDataOptions.valueColumn != '' &&
            this.selectedChartType != null) {
            this.chartDiscreteDataOptionsChange.emit(this.discreteDataOptions);

            if (this.chartItem != null) {
                this.loadChart();
            }
            else {
                this.chartTypeChange();
            }
        }
        else if (dataType == 1 &&
            this.seriesDataOptions.nameColumn != '' &&
            this.seriesDataOptions.seriesNameColumn != '' &&
            this.seriesDataOptions.valueColumn != '' &&
            this.selectedChartType != null) {

            this.chartSeriesDataOptionsChange.emit(this.seriesDataOptions);

            if (this.chartItem != null) {
                this.loadChart();
            }
            else {
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
            this.chartService.saveChart(this.chartItem, this.selectedChartType,
                this.discreteDataOptions, this.seriesDataOptions, reportGUID).subscribe(res => {
                    this._snackbar.open(`Chart saved.`, 'x', {
                        duration: 5000
                    });
                }, err => {
                    this._snackbar.open(`Error: ${<any>err}`, 'x', {
                        duration: 5000
                    });
                });
        }
    }

}