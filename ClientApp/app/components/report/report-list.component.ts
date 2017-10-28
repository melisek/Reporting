﻿import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Http } from '@angular/http';
import { DataSource } from '@angular/cdk/collections';
import { MatSort, MatPaginator, MatDialog, MatSnackBar } from '@angular/material';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/observable/merge';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
/*import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/observable/fromEvent';*/

import { ReportService } from "./report.service";
import { IReport } from "./report";
import { ShareDialogComponent } from '../shared/share-dialog.component';
import { IResponseResult, IEntityWithIdName } from '../shared/shared-interfaces';


@Component({
    selector: 'report',
    templateUrl: './report-list.component.html',
    styleUrls: [ './report-list.component.css' ]
})
export class ReportComponent implements OnInit {
    displayedColumns = ['id', 'name', 'query', 'createdBy', 'createdAt', 'modifiedBy', 'modifiedAt', 'actions'];
    service: ReportService | null;
    dataSource: ExampleDataSource | null;

    sharePermissions: IEntityWithIdName[];

    constructor(private http: Http, private dialog: MatDialog, private snackbar: MatSnackBar) { }

    @ViewChild(MatSort) sort: MatSort;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild('filter') filter: ElementRef;


    ngOnInit() {
        this.service = new ReportService(this.http);
        this.dataSource = new ExampleDataSource(this.service!, this.sort, this.paginator);
        /*Observable.fromEvent(this.filter.nativeElement, 'keyup')
            .debounceTime(150)
            .distinctUntilChanged()
            .subscribe(() => {
                if (!this.dataSource) { return; }
                this.dataSource.filter = this.filter.nativeElement.value;
            });*/
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
        this.sharePermissions = [
            {
                id: 1,
                name: "Szerkesztés és megosztás"
            },
            {
                id: 2,
                name: "Szerkesztés"
            }];

        let dialogRef = this.dialog.open(ShareDialogComponent, {
            width: '400px',
            data: { reportId: id, name: name, email: null, permissions: this.sharePermissions }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');
            if (result != undefined) {
                console.log(`email:${result.email};permission:${result.permission}`);
                this.snackbar.open(`${result.reportName} shared with ${result.email}.`, 'OK', {
                    duration: 5000
                });
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

    /** Returns a sorted copy of the database data. */
    /*getSortedData(): IReport[] {
        const data = this._reportService.getReports.slice().filter((item: IReport) => {
            let searchStr = item.name.toLowerCase();
            return searchStr.indexOf(this.filter.toLowerCase()) != -1;
        });

        if (!this._sort.active || this._sort.direction == '') { return data; }

        return data.sort((a, b) => {
            let propertyA: number | string = '';
            let propertyB: number | string = '';

            switch (this._sort.active) {
                case 'id': [propertyA, propertyB] = [a.id, b.id]; break;
                case 'name': [propertyA, propertyB] = [a.name, b.name]; break;
               // case 'progress': [propertyA, propertyB] = [a.progress, b.progress]; break;
               // case 'color': [propertyA, propertyB] = [a.color, b.color]; break;
            }

            let valueA = isNaN(+propertyA) ? propertyA : +propertyA;
            let valueB = isNaN(+propertyB) ? propertyB : +propertyB;

            return (valueA < valueB ? -1 : 1) * (this._sort.direction == 'asc' ? 1 : -1);
        });
    }*/
}