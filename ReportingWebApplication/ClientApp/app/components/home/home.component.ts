import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { DashboardService } from '../dashboard/dashboard.service';
import { ReportService } from '../report/report.service';
import { IListFilter } from '../shared/shared-interfaces';
import { IReport } from '../report/report';
import { IDashboard } from '../dashboard/dashboard';
import { UserService } from '../user/user.service';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styles: ['.mat-progress-spinner{margin: 0 auto; width: 50px;}']
})
export class HomeComponent implements OnInit {

    reports: IReport[];
    dashboards: IDashboard[];

    isLoadingReports: boolean = false;
    isLoadingDashboards: boolean = false;

    constructor(
        private _cdr: ChangeDetectorRef,
        private _reportService: ReportService,
        private _dashboardService: DashboardService,
        private _userService: UserService,
        private _titleService: Title) {
            this._titleService.setTitle("Home");
    }

    ngOnInit() {
        let filter: IListFilter = {
            filter: "",
            page: 0,
            rows: 10,
            sort: {
                columnName: "ModifyDate", direction: "Desc"
            }
        };

        this.isLoadingReports = true;
        this._reportService.getReports(filter).subscribe(
            result => {
                this.reports = result.reports;
                this.isLoadingReports = false;
            }
        );

        this.isLoadingDashboards = true;
        this._dashboardService.getDashboards(filter).subscribe(
            result => {
                this.dashboards = result.dashboards;
                this.isLoadingDashboards = false;
            }
        );

        this._cdr.detectChanges();
    }
}

