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

import { DashboardService } from './dashboard.service';
import { IDashboard } from "./dashboard";
import { ShareDialogComponent } from '../shared/share-dialog.component';
import { IResponseResult, IEntityWithIdName, IListFilter } from '../shared/shared-interfaces';



@Component({
    selector: 'dashboard',
    templateUrl: './dashboard-list.component.html',
    styleUrls: ['./dashboard-list.component.css', '../shared/shared-styles.css' ]
})
export class DashboardListComponent implements OnInit {
    displayedColumns = ['Name', 'Author', 'CreationDate', 'LastModifier', 'ModifyDate', 'Actions'];
    service: DashboardService | null;
    dataSource: DashboardDataSource | null;

    sharePermissions: IEntityWithIdName[];

    constructor(private http: Http,
        private dialog: MatDialog,
        private _snackbar: MatSnackBar) { }

    @ViewChild(MatSort) sort: MatSort;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild('filter') filter: ElementRef;


    ngOnInit() {
        this.service = new DashboardService(this.http);
        this.dataSource = new DashboardDataSource(this.service!, this.sort, this.paginator);
        Observable.fromEvent(this.filter.nativeElement, 'keydown')
            .debounceTime(150)
            .distinctUntilChanged()
            .subscribe((k: any) => {
                if (!this.dataSource) { return; }
                if (k.which === 13)
                    this.dataSource.filter = this.filter.nativeElement.value;
            });
    }

    deleteDashboard(guid: string): void {
        if (this.service != null) {
            this.service.deleteDashboard(guid)
                .subscribe(res => {
                    this._snackbar.open(`Dashboard deleted.`, 'x', {
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
}

export class DashboardDataSource extends DataSource<any> {
    totalCount = 0;
    isLoadingResults = false;

    _filterChange = new BehaviorSubject('');
    get filter(): string { return this._filterChange.value; }
    set filter(filter: string) { this._filterChange.next(filter); }

    constructor(private _dashboardService: DashboardService, private _sort: MatSort, private _paginator: MatPaginator) {
        super();
    }

    connect(): Observable<IDashboard[]> {
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
                return this._dashboardService.getDashboards(filterObject);
            })
            .map(data => {
                this.isLoadingResults = false;
                this.totalCount = data.totalCount;

                return data.dashboards;
            })
            .catch(() => {
                return Observable.of([]);
            });
    }

    disconnect() { }
}