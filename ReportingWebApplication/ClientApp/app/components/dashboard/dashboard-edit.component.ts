import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSort, MatPaginator, MatDialog, MatSelectionList, MatSnackBar, MatList } from '@angular/material';


import { ReportService } from '../report/report.service';
import { IDashboardCreate } from './dashboard';
import { IListFilter } from '../shared/shared-interfaces';
import { IReportList } from '../report/report';

@Component({
    styleUrls: ['./dashboard-edit.component.css', '../shared/shared-styles.css'],
    templateUrl: './dashboard-edit.component.html',
    encapsulation: ViewEncapsulation.None
})
export class DashboardEditComponent implements OnInit {

    @ViewChild('reports') columns: MatList;

    dashboard: IDashboardCreate;
    //reportList: IReportList;

    sourceItems: any[] = [
    ];
    targetItems: any[] = [];
    targetItemsA: any[] = [];
    targetItemsB: any[] = [];

    constructor(
        private _reportService: ReportService,
        //private _reportService: ReportService,

        private dialog: MatDialog,
        private _snackbar: MatSnackBar,
        private _router: Router,
        private _route: ActivatedRoute) { }


    ngOnInit() {
        this.dashboard = {
            name: "",
            dashboardGUID: "",
            reports: []
        };

        let filter: IListFilter = {
            filter: "",
            page: 0,
            rows: 99,
            sort: {
                columnName: "ModifyDate", direction: "Asc"
            }
        };
        this._reportService.getReports(filter).subscribe(
            result => {
                result.reports.forEach(x => {
                    this.sourceItems.push({ "label": x.name })
                })
            }
        );

        //this.report = {
        //    name: "",
        //    queryGUID: "",
        //    columns: [],
        //    filter: "",
        //    rows: 10,
        //    sort: {
        //        columnName: "", direction: "asc"
        //    }
        //};
        //let reportGUID = this._route.snapshot.paramMap.get('reportGUID');
        //if (reportGUID != null) {
        //    if (this.reportService != null) {
        //        this.reportService.getReport(reportGUID)
        //            .subscribe(report => {
        //                this.report = report;
        //                this.queryChange();
        //                this.disableChartTab = false;
        //            },
        //            err => console.log(err));

        //        let discreteDataOptions: IChartDiscreteDataOptions = {
        //            reportGUID: reportGUID,
        //            nameColumn: "Table_95_Field_67",
        //            valueColumn: "Table_95_Field_41",
        //            aggregation: 0
        //        };
        //        this.reportService.getDiscreteDiagramData(discreteDataOptions)
        //            .subscribe(data => {
        //                this.chartData = data;
        //            },
        //            err => console.log(err));
        //    }
                
             
        //}
        //else {
        //    this.report = {
        //        name: "",
        //        queryGUID: "",
        //        columns: [],
        //        filter: "",
        //        rows: 10,
        //        sort: {
        //            columnName: "", direction: "asc"
        //        }
        //    };
        //}
        //console.log(reportGUID);

    }

    log(e: any) {
        console.log(e.type, e);
    }

    queryChange(): void {
        
    }

    onSaveClick() {

    }
    
}
