import { Component, OnInit, ViewChild, ViewEncapsulation, ComponentFactoryResolver, ChangeDetectorRef, AfterViewInit, ViewChildren, QueryList, ViewContainerRef } from '@angular/core';
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
import { DashboardService } from './dashboard.service';

@Component({
    styleUrls: ['./dashboard-edit.component.css', '../shared/shared-styles.css'],
    templateUrl: './dashboard-edit.component.html',
    encapsulation: ViewEncapsulation.None
})
export class DashboardEditComponent implements OnInit {

    @ViewChild('reports') columns: MatList;
    @ViewChildren(ChartDirective) chartHost: QueryList<ChartDirective>;

    chartItems: ChartItem[] = [];

    dashboard: IDashboardCreate;
    dashboardGUID: string | null;

    gridCount: number = 4;
    numbers = Array(4);
    dropzones: string[] = [];

    sourceItems: any[] = [];
    get targetItems(): any[][] { return this._targetItems; }
    set targetItems(value: any[][]) { console.log('lefutott'); this._targetItems = value; }
    _targetItems: any[][] = [];

    constructor(
        private _reportService: ReportService,
        private _chartService: ChartService,
        private _dashboardService: DashboardService,
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
        this.dashboardGUID = this._route.snapshot.paramMap.get('dashboardGUID');
        console.log('dashguid:' + this.dashboardGUID);
        if (this.dashboardGUID != null) {
            this._dashboardService.getDashboard(this.dashboardGUID)
                .subscribe(res => {
                    this.dashboard = res;
                    console.log('dashrepo'+JSON.stringify(res));
                    this.dashboard.reports.sort(x => x.position).forEach((x, i) => {
                        this.targetItems[x.position] = [{ label: x.name, reportGUID: x.reportGUID }];
                        

                    });
                    this.gridCount = this.dashboard.reports.length;
                    this.sliderChange();
                    this.initCharts();
                    console.log('target1' + JSON.stringify(this.targetItems));

                });
        }
        else {
            this.numbers.forEach((x, i) => {
                this.targetItems.push([]);
            });
        }

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

        this.numbers.forEach((x,i) => {
            this.dropzones.push('target-' + i);
        });
        
        //this.getChart("");
    }

    initCharts() {
        console.log('target'+JSON.stringify(this.targetItems));
        this.targetItems.forEach((x, i) => {
            console.log('x=' + JSON.stringify(x) + '/i=' + i);
            if (x.length > 0)
                this.getChart(x[0].reportGUID, i);

        });
    }

    chartData: any;

    onDrop(e: any, id: number) {
        console.log(e.type, e);
        console.log('dropid' + id);
        console.log('dropz' + this.dropzones);
        console.log('target' + JSON.stringify(this.targetItems));
        
        //this.targetItemsA[id];
        this.getChart(e.value.reportGUID, id);

    }

    onOver() {

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
                    this.chartItems[id] = this._chartService.getChart(<number>style.chartType);
                    this.chartItems[id].options = style.displayOptions;
                    console.log(this.chartItems[id]);
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

        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.chartItems[id].component);
        let viewContainerRef = this.getViewContainerRef(id)!;
        viewContainerRef.clear();

        let componentRef = viewContainerRef.createComponent(componentFactory);
        (<IChart>componentRef.instance).options = this.chartItems[id].options;
        console.log('loadcomp:' + chartData);
        (<IChart>componentRef.instance).data = chartData;
        this._cdr.detectChanges();

    }

    removeChart(id: number) {
        let viewContainerRef = this.getViewContainerRef(id)!;
        viewContainerRef.clear();
    }

    getViewContainerRef(id: number): ViewContainerRef | null {
        let chartDirective = this.chartHost.find((x, i) => i === id);

        if (chartDirective != null)
            return chartDirective.viewContainerRef;
        else
            return null;
    }

    queryChange(): void {
        
    }

    onSaveClick() {
        this.dashboard.reports = [];
        this.targetItems.filter(x => x.length > 0).forEach((x, i) => {
            this.dashboard.reports.push({ reportGUID: x[0].reportGUID, position: i, name: x[0].label });
        });
        console.log('dash'+this.dashboard);
        this._dashboardService.addDashboard(this.dashboard)
            .subscribe(res => {
                this._snackbar.open(`Dashboard created.`, 'x', {
                    duration: 5000
                });
            }, err => {
                this._snackbar.open(`Error: ${<any>err}`, 'x', {
                    duration: 5000
                });
            });
    }
    
}
