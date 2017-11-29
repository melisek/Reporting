﻿import { Component, OnInit, ElementRef, ViewChild, ChangeDetectorRef } from '@angular/core';
import { Http } from '@angular/http';
import { DataSource } from '@angular/cdk/collections';
import { MatSort, MatPaginator, MatDialog, MatSelectionList, MatSnackBar } from '@angular/material';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/observable/merge';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/observable/fromEvent';

import { QueryService } from "../query/query.service";
import { IReport, IReportCreate } from "./report";
import { ShareDialogComponent } from '../shared/share-dialog.component';
import { ChartEditComponent } from '../chart/chart-edit.component';
import { IResponseResult, IEntityWithIdName, IListFilter, IChartDiscreteDataOptions } from '../shared/shared-interfaces';
import { ReportService } from './report.service';
import { IQueryColumns, IQuery, IQuerySourceData, IQueryColumn } from '../query/query';
import { ActivatedRoute, Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { AuthHttp } from 'angular2-jwt';


@Component({
    templateUrl: './report-edit.component.html',
    styleUrls: [ './report-edit.component.css', '../shared/shared-styles.css' ]
})
export class ReportEditComponent implements OnInit {

    report: IReportCreate;
    reportGUID: string | null;

    columnNames = [
        { columnDef: 'id', header: 'ID', cell: (row: IReport) => `${row.query}` },
        { columnDef: 'name', header: 'Name', cell: (row: IReport) => `${row.modifyDate}` }
    ];

    /** Column definitions in order */
    displayedColumns = this.columnNames.map(x => x.columnDef);
    //displayedColumns = ['id', 'name', 'query', 'createdBy', 'createdAt', 'modifiedBy', 'modifiedAt', 'actions'];
    //queryService: QueryService | null;
    //reportService: ReportService | null;
    dataSource: QueryDataSource | null;
    selectedValue: string;
    showDataTable: boolean;
    showPaginator: boolean = false;
    disableChartTab: boolean = true;

    queries: IQuery[];
    queryColumns: IQueryColumns;

    chartData: any;

    constructor(//private http: AuthHttp,
        private reportService: ReportService,
        private queryService: QueryService,
        private dialog: MatDialog,
        private _snackbar: MatSnackBar,
        private _cdr: ChangeDetectorRef,
        private _router: Router,
        private _route: ActivatedRoute,
        private titleService: Title) { }

    @ViewChild(MatSort) sort: MatSort;
    @ViewChild('paginator') paginator: MatPaginator;
    @ViewChild('filter') filter: ElementRef;
    @ViewChild('columns') columns: MatSelectionList;
    @ViewChild(ChartEditComponent)
    chartComponent: ChartEditComponent;


    ngOnInit() {
        //this.reportService = new ReportService(this.http);
        this.report = {
            name: "",
            queryGUID: "",
            columns: [],
            filter: "",
            rows: 10,
            sort: {
                columnName: "", direction: "asc"
            }
        };
        this.reportGUID = this._route.snapshot.paramMap.get('reportGUID');
        console.log('reportguid:' + this.reportGUID);
        if (this.reportGUID != null) {
            this.reportService.getReport(this.reportGUID)
                .subscribe(report => {
                    this.report = report;
                    this.queryChange();
                    this.disableChartTab = false;
                    this.titleService.setTitle(report.name + " - Edit Report");
                },
                err => console.log(err));

            this.chartComponent.initChartOptions(this.reportGUID).subscribe(result => {
                if (result.reportGUID != '') {
                    let discreteDataOptions: IChartDiscreteDataOptions = {
                        reportGUID: this.reportGUID!,
                        nameColumn: result.nameColumn,
                        valueColumn: result.valueColumn,
                        aggregation: result.aggregation
                    };
                    console.log('repot-edit.oninit-initchart-result:' + discreteDataOptions);
                    this.reportService!.getDiscreteDiagramData(discreteDataOptions)
                        .subscribe(data => {
                            this.chartData = data;
                        },
                        err => console.log(err));
                }
            });
        }
        else {
            this.report = {
                name: "",
                queryGUID: "",
                columns: [],
                filter: "",
                rows: 10,
                sort: {
                    columnName: "", direction: "asc"
                }
            };
            this.titleService.setTitle("Create Report");
        }
        console.log(this.reportGUID);
        

        //this.queryService = new QueryService(this.http);
        this.queryService.getQueriesIdName()
            .subscribe(queries => this.queries = queries);

        this.showDataTable = false;
        
        //this.dataSource = new ExampleDataSource(this.queryService!, this.sort, this.paginator);
        this._cdr.detectChanges();
        /*this.queryService.getQueryColumns(Number(this.selectedValue))
            .subscribe(data => this.queryColumns = data);*/
            /*.map(data => {
                return data;
            })
            .catch(() => {
                return Observable.of([]);
            });*/
        
        /*Observable.fromEvent(this.querySelect.nativeElement, 'change')
            .distinctUntilChanged()
            .subscribe(() => {
                this.dataSource.filter = this.querySelect.nativeElement.value;
            });*/

        //this.columns.options.subscribe(
        //    res => {
        //        this.columns.options.find(x => x.value == "Table_95_Field_3")!.toggle();
        //    });


        
    }


    queryChange(): void {
        this.queryService.getQueryColumns(this.report.queryGUID)
            .subscribe(data => {
                this.queryColumns = data;
                if (this.columns != null)
                    this.columns.selectedOptions.clear();
                this._cdr.detectChanges();
                //if (this.dataSource)
                //    this.dataSource.clearSort();
            });
    }

    clearFilter(): void {
        this.filter.nativeElement.value = '';
        this.dataSource!.filter = '';
    }

    getColumnText(name: string): string {
        let col = this.queryColumns.columns.find(x => x.name === name);
        if (col != undefined)
            return col.text;
        else
            return name;
    }
    onUpdateClick(): void {
                   // this.columns.options.find(x => x.value == "Table_95_Field_3")!.toggle();

        //this.columns.options.first.;
        console.log(this.sort);
        this.dataSource = new QueryDataSource(this.queryService!, this.sort, this.paginator);
        this.dataSource.queryGUID = this.report.queryGUID;

        console.log('colbefore '+this.columnNames);
        this.columnNames = [];
        console.log('colafter ' + this.columnNames);
        console.log('displaybeforeinit:' + this.displayedColumns);

        
        this.columns.selectedOptions.selected.forEach(x => {
            console.log(x);
            this.columnNames.push({ columnDef: x.value, header: "", cell: (row: any) => `${(<any>row)[x.value]}` });
        });
        this.columnNames.forEach(x => {
            x.header = this.getColumnText(x.columnDef);
        });

        console.log('displayafterinit: ' + this.displayedColumns);
        this.displayedColumns = this.columnNames.map(x => x.columnDef);
        this.dataSource.selectedColumns = this.displayedColumns;

        console.log('display:'+this.displayedColumns);
        console.log('paginator' + this.paginator);

        Observable.fromEvent(this.filter.nativeElement, 'keydown')
            .debounceTime(150)
            .distinctUntilChanged()
            .subscribe((k: any) => {
                if (!this.dataSource) { return; }
                if (k.which === 13)
                    this.dataSource.filter = this.filter.nativeElement.value;
            });

        this.showDataTable = true;
        this._cdr.detectChanges();
    }

    onChartSave(): void {
        this.chartComponent.onChartSave(this.reportGUID!);
    }

    onSaveClick(): void {

        this.report.queryGUID = this.report.queryGUID;
        this.report.sort = { columnName: this.sort.active, direction: this.sort.direction };
        this.report.rows = this.paginator.pageSize;
        this.report.filter = this.filter.nativeElement.value;
        this.report.columns = this.columns.selectedOptions.selected.map(x => x.value);

        this.reportService.addReport(this.report)
            .subscribe(res => {
                console.log('res:' + res._body);
                this._snackbar.open(`Report created.`, 'x', {
                    duration: 5000
                });
                this._router.navigate(['./reports/edit/' + res._body,]);
            }, err => {
                this._snackbar.open(`Error: ${<any>err}`, 'x', {
                    duration: 5000
                });
            });



        //if (this.reportService != null)
        //    this.reportService.getStyle()
        //        .subscribe();
    }

    onChartDataOptionChange(chartDataOptions: IChartDiscreteDataOptions): void {
        chartDataOptions.reportGUID = this.reportGUID!;
        this.reportService.getDiscreteDiagramData(chartDataOptions)
            .subscribe(data => {
                this.chartData = data;
            },
            err => console.log(err));
    }

    openShareDialog(id: number, name: string): void {
        let dialogRef = this.dialog.open(ShareDialogComponent, {
            width: '250px',
            data: { reportId: id, name: name, email: null }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');
            if (result != undefined) {
                console.log('result:' + result);
            }
        });
    }
}

export class QueryDataSource extends DataSource<any[]> {
    totalCount = 0;
    isLoadingResults = false;

    _filterChange = new BehaviorSubject('');
    get filter(): string { return this._filterChange.value; }
    set filter(filter: string) { this._filterChange.next(filter); }

    defaultColumns: string[] = [];
    _selectedColumnsChange = new BehaviorSubject(this.defaultColumns);
    get selectedColumns(): string[] { return this._selectedColumnsChange.value; }
    set selectedColumns(selectedColumns: string[]) { this._selectedColumnsChange.next(selectedColumns); }

    _queryChange = new BehaviorSubject('');
    get queryGUID(): string { return this._queryChange.value; }
    set queryGUID(queryGUID: string) { this._queryChange.next(queryGUID); }

    constructor(private _queryService: QueryService, private _sort: MatSort, private _paginator: MatPaginator) {
        super();
    }

    clearSort(): void {
        this._sort.active = '';
        this.selectedColumns = [];
    }

    /** Connect function called by the table to retrieve one stream containing the data to render. */
    connect(): Observable<any[]> {
        const displayDataChanges = [
            this._sort.sortChange,
            this._filterChange,
            this._selectedColumnsChange,
            this._queryChange,
            this._paginator.page,
        ];

        this._sort.sortChange.subscribe(() => this._paginator.pageIndex = 0);

        return Observable.merge(...displayDataChanges)
            .startWith(null)
            .switchMap(() => {
                this.isLoadingResults = true;

                console.log('selcol ' + this.selectedColumns);
                console.log('selact ' + this._sort.active);

                if (this._sort.active == null || this._sort.active == '')
                    this._sort.active = this.selectedColumns[0];

                console.log('selact2 ' + this._sort.active);

                return this._queryService.getQuerySourceData(
                    {
                        queryGUID: this.queryGUID,
                        page: this._paginator.pageIndex + 1,
                        rows: this._paginator.pageSize,
                        filter: this.filter,
                        sort: { columnName: this._sort.active, direction: this._sort.direction },
                        columns: this.selectedColumns
                    });
            })
            .map(result => {
                this.isLoadingResults = false;
                this.totalCount = result.TotalCount;
                console.log('ds res: ' + result.Data);
                return result.Data;
            })
            .catch(() => {
                return Observable.of([]);
            });
    }

    disconnect() { }
}