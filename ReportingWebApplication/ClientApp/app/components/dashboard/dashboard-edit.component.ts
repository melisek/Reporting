import { Component, OnInit, ViewChild, ViewEncapsulation, ComponentFactoryResolver, ChangeDetectorRef, AfterViewInit, ViewChildren, QueryList } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSort, MatPaginator, MatDialog, MatSelectionList, MatSnackBar, MatList } from '@angular/material';


import { ReportService } from '../report/report.service';
import { IDashboardCreate } from './dashboard';
import { IListFilter } from '../shared/shared-interfaces';
import { IReportList } from '../report/report';
import { ChartService } from '../chart/chart.service';
import { ChartItem } from '../chart/chart-item';
import { ChartDirective } from '../chart/chart.directive';
import { IChart } from '../chart/chart';

@Component({
    styleUrls: ['./dashboard-edit.component.css', '../shared/shared-styles.css'],
    templateUrl: './dashboard-edit.component.html',
    encapsulation: ViewEncapsulation.None
})
export class DashboardEditComponent implements OnInit {

    @ViewChild('reports') columns: MatList;
    @ViewChildren(ChartDirective) chartHost: QueryList<ChartDirective>;

    chartItem: ChartItem;

    dashboard: IDashboardCreate;
    //reportList: IReportList;

    gridCount: number = 4;
    numbers = Array(4);
    dropzones: string[] = [];

    sourceItems: any[] = [
    ];
    targetItems: any[] = [];
    targetItemsA: any[][] = [];
    targetItemsB: any[] = [];

    constructor(
        private _reportService: ReportService,
        //private _reportService: ReportService,
        private _chartService: ChartService,
        private componentFactoryResolver: ComponentFactoryResolver,
        private dialog: MatDialog,
        private _snackbar: MatSnackBar,
        private _router: Router,
        private _route: ActivatedRoute,
        private _cdr: ChangeDetectorRef) {
        this.numbers = Array(this.gridCount).fill(0);
    }


    ngOnInit() {
        this.dashboard = {
            name: "",
            dashboardGUID: "",
            reports: []
        };

        let filter: IListFilter = {
            filter: "",
            page: 0,
            rows: 99999,
            sort: {
                columnName: "Name", direction: "Asc"
            }
        };
        this._reportService.getReports(filter).subscribe(
            result => {
                result.reports.filter(x => x.hasStyle).forEach(x => {
                    this.sourceItems.push({ "label": x.name, "reportGUID": x.reportGUID })
                })
            }
        );

        let i = 0;
        this.numbers.forEach(x => {
            this.dropzones.push('target-' + i);
            i++;
        });
        
        //this.getChart("");
    }

    chartData: any;

    onDrop(e: any, id: number) {
        console.log(e.type, e);
        console.log('dropid' + id);
        console.log('target'+this.targetItemsA);

        this.getChart(e.value.reportGUID, id);

    }

    sliderChange() {
        console.log('slider onchange');
        this.numbers = Array(this.gridCount).fill(0);
        let i = 0;
        this.numbers.forEach(x => {
            this.dropzones.push('target-' + i);
            i++;
        });
    }
    onRemove(e: any, id: number) {
        console.log(e.type, e);
        this.removeChart(id);
    }

    getChart(reportGUID: string, id: number) {
        this._reportService.getReport(reportGUID).subscribe(report => {
            this._chartService.getChartOptions(reportGUID).subscribe(data => {
                let style = JSON.parse(data.style);
                if (style != null) {
                    console.log(style);
                    this.chartItem = this._chartService.getChart(<number>style.chartType);
                    this.chartItem.options = style.displayOptions;
                    console.log(this.chartItem);
                    style.dataOptions.reportGUID = reportGUID;
                    this._reportService.getDiscreteDiagramData(style.dataOptions)
                        .subscribe(data2 => {
                            this.chartData = data2;
                            this.loadChart(data2,id);
                        },
                        err => console.log(err));
                }

            });
        });
    }

    

    loadChart(chartData: any, id: number) {

        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.chartItem.component);
        let viewContainerRef = id == 0 ? this.chartHost.first.viewContainerRef : this.chartHost.last.viewContainerRef;
        viewContainerRef.clear();

        let componentRef = viewContainerRef.createComponent(componentFactory);
        (<IChart>componentRef.instance).options = this.chartItem.options;
        console.log('loadcomp:' + chartData);
        (<IChart>componentRef.instance).data = chartData;
        this._cdr.detectChanges();

    }

    removeChart(id: number) {
        let viewContainerRef = id == 0 ? this.chartHost.first.viewContainerRef : this.chartHost.last.viewContainerRef;
        viewContainerRef.clear();
    }

    queryChange(): void {
        
    }

    onSaveClick() {

    }
    
}
