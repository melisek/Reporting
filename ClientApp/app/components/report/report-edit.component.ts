import { Component, OnInit, ElementRef, ViewChild, ChangeDetectorRef } from '@angular/core';
import { Http } from '@angular/http';
import { DataSource } from '@angular/cdk/collections';
import { MatSort, MatPaginator, MatDialog, MatSelectionList } from '@angular/material';
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
import { IResponseResult, IEntityWithIdName, IListFilter } from '../shared/shared-interfaces';
import { ReportService } from './report.service';
import { IQueryColumns, IQuery } from '../query/query';


@Component({
    templateUrl: './report-edit.component.html',
    styleUrls: [ './report-edit.component.css', '../shared/shared-styles.css' ]
})
export class ReportEditComponent implements OnInit {

    report: IReportCreate;


    columnNames = [
        { columnDef: 'id', header: 'ID', cell: (row: IReport) => `${row.query}` },
        { columnDef: 'name', header: 'Name', cell: (row: IReport) => `${row.modifyDate}` }
    ];

    /** Column definitions in order */
    displayedColumns = this.columnNames.map(x => x.columnDef);
    //displayedColumns = ['id', 'name', 'query', 'createdBy', 'createdAt', 'modifiedBy', 'modifiedAt', 'actions'];
    queryService: QueryService | null;
    reportService: ReportService | null;
    dataSource: ExampleDataSource | null;
    selectedValue: string;
    showDataTable: boolean;

    queries: IQuery[];
    queryColumns: IQueryColumns;


    constructor(private http: Http, private dialog: MatDialog, private _cdr: ChangeDetectorRef) { }

    @ViewChild(MatSort) sort: MatSort;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild('filter') filter: ElementRef;
    @ViewChild('columns') columns: MatSelectionList;


    ngOnInit() {
        this.report = {
            name: "",
            queryGUID: "",
            columns: [],
            filter: "",
            rows: 10,
            sort: "abc"
            /*Sort: {
                Column: "", Direction: ""
            }*/
        };

        this.queryService = new QueryService(this.http);
        this.queryService.getQueriesIdName()
            .subscribe(queries => this.queries = queries);

        this.showDataTable = false;
        this.reportService = new ReportService(this.http);
        this.dataSource = new ExampleDataSource(this.reportService!, this.sort, this.paginator);
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
    }

    queryChange(): void {
        if (this.queryService != null)
            this.queryService.getQueryColumns(this.selectedValue)
                .subscribe(data => this.queryColumns = data);
        
    }

    onUpdateClick(): void {

        this.columnNames = [];
        this.columns.selectedOptions.selected.forEach(x => {
            this.columnNames.push({ columnDef: x.value, header: x.value, cell: (row: IReport) => `${(<any>row)[x.value]}` });
        });

        this.displayedColumns = this.columnNames.map(x => x.columnDef);
        this.showDataTable = true;
        
    }

    onSaveClick(): void {

        this.report.queryGUID = "3066e94b-ff9e-454c-ab58-6a88436e4b52";
        this.report.sort = "abc";//{ Column: "abc", Direction: "asc" };
        this.report.rows = 20;

        if (this.reportService != null)
            this.reportService.addReport(this.report)
                .subscribe();

        //if (this.reportService != null)
        //    this.reportService.getStyle()
        //        .subscribe();
    }

    deleteReport(id: number): boolean {
        console.log("okker:"+ id);
        return false;
        //return this.service!.deleteReport(id)
        //    .map(data => {
        //        return data.result;
        //    })
        //    .catch(() => {
        //        return Observable.of([]);
        //    });
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
            //this.animal = result;
            }
        });
    }
}

/**
 * Data source to provide what data should be rendered in the table. Note that the data source
 * can retrieve its data in any way. In this case, the data source is provided a reference
 * to a common data base, ExampleDatabase. It is not the data source's responsibility to manage
 * the underlying data. Instead, it only needs to take the data and send the table exactly what
 * should be rendered.
 */
export class ExampleDataSource extends DataSource<any> {
    totalCount = 0;
    isLoadingResults = false;

    _filterChange = new BehaviorSubject('');
    get filter(): string { return this._filterChange.value; }
    set filter(filter: string) { this._filterChange.next(filter); }

    constructor(private _reportService: ReportService,
        private _sort: MatSort, private _paginator: MatPaginator) {
        super();
    }

    /** Connect function called by the table to retrieve one stream containing the data to render. */
    connect(): Observable<IReport[]> {
        const displayDataChanges = [
            this._sort.sortChange,
            this._filterChange,
            this._paginator.page,
        ];

        this._sort.sortChange.subscribe(() => this._paginator.pageIndex = 0);

        return Observable.merge(...displayDataChanges)
            .startWith(null)
            .switchMap(() => {
                this.isLoadingResults = true;
                let filterObject: IListFilter = {
                    filter: this.filter,
                    page: this._paginator.pageIndex,
                    sort: { columnName: this._sort.active, direction: this._sort.direction },
                    rows: 10,
                }
                return this._reportService.getReports(filterObject);
            })
            .map(data => {
                this.isLoadingResults = false;
                this.totalCount = data.totalCount;

                return data.reports;
            })
            .catch(() => {
                return Observable.of([]);
            });
    }

    disconnect() { }
}