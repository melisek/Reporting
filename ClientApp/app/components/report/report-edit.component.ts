import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Http } from '@angular/http';
import { DataSource } from '@angular/cdk/collections';
import { MatSort, MatPaginator, MatDialog } from '@angular/material';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/observable/merge';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/observable/fromEvent';

import { QueryService } from "../query/query.service";
import { IReport } from "./report";
import { ShareDialogComponent } from '../shared/share-dialog.component';
import { IResponseResult, IEntityWithIdName } from '../shared/shared-interfaces';
import { ReportService } from './report.service';
import { IQueryColumns } from '../query/query';


@Component({
    templateUrl: './report-edit.component.html',
    //styleUrls: [ './report-edit.component.css' ]
})
export class ReportEditComponent implements OnInit {
    displayedColumns = ['id', 'name', 'query', 'createdBy', 'createdAt', 'modifiedBy', 'modifiedAt', 'actions'];
    queryService: QueryService | null;
    dataSource: ExampleDataSource | null;
    selectedValue: string;

    queries: IEntityWithIdName[];
    queryColumns: IQueryColumns;

    constructor(private http: Http, private dialog: MatDialog) { }

    @ViewChild(MatSort) sort: MatSort;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild('filter') filter: ElementRef;


    ngOnInit() {
        this.queryService = new QueryService(this.http);
        this.queryService.getQueriesIdName()
            .subscribe(queries => this.queries = queries);

        /*this.queryService.getQueryColumns(Number(this.selectedValue))
            .subscribe(data => this.queryColumns = data);*/
            /*.map(data => {
                return data;
            })
            .catch(() => {
                return Observable.of([]);
            });*/

        //this.dataSource = new ExampleDataSource(this.service!, this.sort, this.paginator);
        /*Observable.fromEvent(this.querySelect.nativeElement, 'change')
            .distinctUntilChanged()
            .subscribe(() => {
                this.dataSource.filter = this.querySelect.nativeElement.value;
            });*/
    }

    queryChange(): void {
        if (this.queryService != null)
            this.queryService.getQueryColumns(Number(this.selectedValue))
                .subscribe(data => this.queryColumns = data);
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

    constructor(private _reportService: ReportService, private _sort: MatSort, private _paginator: MatPaginator) {
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
                return this._reportService.getReports(this._sort.active, this._sort.direction, this._paginator.pageIndex);
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