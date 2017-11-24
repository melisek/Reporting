import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
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

import { ReportService } from "./report.service";
import { IReport } from "./report";
import { ShareDialogComponent } from '../shared/share-dialog.component';
import { IResponseResult, IEntityWithIdName, IListFilter } from '../shared/shared-interfaces';
import { Title } from '@angular/platform-browser';
import { AuthHttp } from 'angular2-jwt';


@Component({
    selector: 'report',
    templateUrl: './report-list.component.html',
    styleUrls: ['./report-list.component.css', '../shared/shared-styles.css' ]
})
export class ReportListComponent implements OnInit {
    displayedColumns = ['Name', 'Query', 'Author', 'CreationDate', 'LastModifier', 'ModifyDate', 'Actions'];
    service: ReportService | null;
    dataSource: ExampleDataSource | null;

    sharePermissions: IEntityWithIdName[];

    constructor(private http: AuthHttp,
        private dialog: MatDialog,
        private _snackbar: MatSnackBar,
        private titleService: Title) { }

    @ViewChild(MatSort) sort: MatSort;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild('filter') filter: ElementRef;


    ngOnInit() {
        this.titleService.setTitle("Reports");
        this.service = new ReportService(this.http);
        this.dataSource = new ExampleDataSource(this.service!, this.sort, this.paginator);
        Observable.fromEvent(this.filter.nativeElement, 'keydown')
            .debounceTime(150)
            .distinctUntilChanged()
            .subscribe((k: any) => {
                if (!this.dataSource) { return; }
                if (k.which === 13)
                    this.dataSource.filter = this.filter.nativeElement.value;
            });
    }

    deleteReport(guid: string): void {
        if (this.service != null) {
            this.service.deleteReport(guid)
                .subscribe(res => {
                    this._snackbar.open(`Report deleted.`, 'x', {
                        duration: 5000
                    });
                }, err => {
                    this._snackbar.open(`Error: ${<any>err}`, 'x', {
                        duration: 5000
                    });
                });
        }
    }

    openShareDialog(id: number, name: string): void {
        this.sharePermissions = [
            {
                id: "1",
                name: "Szerkesztés és megosztás"
            },
            {
                id: "2",
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
                this._snackbar.open(`${result.reportName} shared with ${result.email}.`, 'OK', {
                    duration: 5000
                });
            }
        });
    }

    clearFilter(): void {
        this.filter.nativeElement.value = '';
        this.dataSource!.filter = '';
    }
}

export class ExampleDataSource extends DataSource<any> {
    totalCount = 0;
    isLoadingResults = false;

    _filterChange = new BehaviorSubject('');
    get filter(): string { return this._filterChange.value; }
    set filter(filter: string) { this._filterChange.next(filter); }

    constructor(private _reportService: ReportService, private _sort: MatSort, private _paginator: MatPaginator) {
        super();
    }

    connect(): Observable<IReport[]> {
        const displayDataChanges = [
            this._sort.sortChange,
            this._filterChange,
            this._paginator.page,
            this._paginator.pageSize
        ];

        this._sort.sortChange.subscribe(() => this._paginator.pageIndex = 0);

        return Observable.merge(...displayDataChanges)
            .startWith(null)
            .switchMap(() => {
                this.isLoadingResults = true;
                let filterObject: IListFilter = {
                    filter: this.filter,
                    page: this._paginator.pageIndex + 1,
                    sort: { columnName: this._sort.active, direction: this._sort.direction },
                    rows: this._paginator.pageSize,
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