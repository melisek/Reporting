//import { Component, ElementRef, ViewChild } from '@angular/core';
//import { DataSource } from '@angular/cdk/collections';
//import { MdSort, MdPaginator } from '@angular/material';
//import { BehaviorSubject } from 'rxjs/BehaviorSubject';
//import { Observable } from 'rxjs/Observable';
//import 'rxjs/add/operator/startWith';
//import 'rxjs/add/observable/merge';
//import 'rxjs/add/operator/map';
//import 'rxjs/add/operator/debounceTime';
//import 'rxjs/add/operator/distinctUntilChanged';
//import 'rxjs/add/observable/fromEvent';

import { Component, OnInit, ViewChild } from '@angular/core';
import { Http } from '@angular/http';
import { DataSource } from '@angular/cdk/collections';
import { MatPaginator, MatSort } from '@angular/material';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/merge';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/operator/switchMap';

import { NgxChartsModule } from '@swimlane/ngx-charts';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
    single: any[];
    multi: any[];

    view: any[] = [700, 400];

    // options
    showXAxis = true;
    showYAxis = true;
    gradient = false;
    showLegend = true;
    showXAxisLabel = true;
    xAxisLabel = 'Country';
    showYAxisLabel = true;
    yAxisLabel = 'Population';

    colorScheme = {
        domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA']
    };




    displayedColumns = ['created_at', 'state', 'number', 'title'];
    exampleDatabase: ExampleDatabase | null;
    dataSource: ExampleDataSource | null;

    @ViewChild(MatSort) sort: MatSort;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    //@ViewChild('filter') filter: ElementRef;

    constructor(private http: Http) {
        Object.assign(this, { single }) 
    }

    /*onSelect(event) {
        console.log(event);
    }*/

    ngOnInit() {
        this.exampleDatabase = new ExampleDatabase(this.http);
        this.dataSource = new ExampleDataSource(
            this.exampleDatabase!, this.paginator, this.sort);
        /*this.dataSource = new ExampleDataSource(this.exampleDatabase, this.sort, this.paginator);
        Observable.fromEvent(this.filter.nativeElement, 'keyup')
            .debounceTime(150)
            .distinctUntilChanged()
            .subscribe(() => {
                if (!this.dataSource) { return; }
                this.dataSource.filter = this.filter.nativeElement.value;
            });*/
    }
}

export var single = [
    {
        "name": "Germany",
        "value": 8940000
    },
    {
        "name": "USA",
        "value": 5000000
    },
    {
        "name": "France",
        "value": 7200000
    }
];

/** Constants used to fill up our data base. */
//const COLORS = ['maroon', 'red', 'orange', 'yellow', 'olive', 'green', 'purple',
//    'fuchsia', 'lime', 'teal', 'aqua', 'blue', 'navy', 'black', 'gray'];
//const NAMES = ['Maia', 'Asher', 'Olivia', 'Atticus', 'Amelia', 'Jack',
//    'Charlotte', 'Theodore', 'Isla', 'Oliver', 'Isabella', 'Jasper',
//    'Cora', 'Levi', 'Violet', 'Arthur', 'Mia', 'Thomas', 'Elizabeth'];

export interface UserData {
    id: string;
    name: string;
    progress: string;
    color: string;
}

export interface GithubApi {
    items: GithubIssue[];
    total_count: number;
}

export interface GithubIssue {
    created_at: string;
    number: string;
    state: string;
    title: string;
}

/** An example database that the data source uses to retrieve data for the table. */
export class ExampleDatabase {
    /** Stream that emits whenever the data has been modified. */
    constructor(private http: Http) { }

    getRepoIssues(sort: string, order: string, page: number): Observable<GithubApi> {
        const href = 'https://api.github.com/search/issues';
        const requestUrl =
            `${href}?q=repo:angular/material2&sort=${sort}&order=${order}&page=${page + 1}`;

        return this.http.get(requestUrl)
            .map(response => response.json() as GithubApi);
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
    resultsLength = 0;
    isLoadingResults = false;
    isRateLimitReached = false;
    //_filterChange = new BehaviorSubject('');
    //get filter(): string { return this._filterChange.value; }
    //set filter(filter: string) { this._filterChange.next(filter); }

    constructor(private exampleDatabase: ExampleDatabase, private paginator: MatPaginator, private sort: MatSort ) {
        super();
    }
   
    /** Connect function called by the table to retrieve one stream containing the data to render. */
    connect(): Observable<GithubIssue[]> {
        const displayDataChanges = [
            //this._exampleDatabase.dataChange,
            this.sort.sortChange,
            //this._filterChange,
            this.paginator.page,
        ];


        this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

        return Observable.merge(...displayDataChanges)
            .startWith(null)
            .switchMap(() => {
                this.isLoadingResults = true;
                return this.exampleDatabase.getRepoIssues(
                    this.sort.active, this.sort.direction, this.paginator.pageIndex);
            })
            .map(data => {
                // Flip flag to show that loading has finished.
                this.isLoadingResults = false;
                this.isRateLimitReached = false;
                this.resultsLength = data.total_count;

                return data.items;
            })
            .catch(() => {
                this.isLoadingResults = false;
                // Catch if the GitHub API has reached its rate limit. Return empty data.
                this.isRateLimitReached = true;
                return Observable.of([]);
            });

        /*return Observable.merge(...displayDataChanges).map(() => {
            return ;
        });*/

        /*return Observable.merge(...displayDataChanges).map(() => {

            const data = this.getSortedData();

            // Grab the page's slice of data.
            const startIndex = this._paginator.pageIndex * this._paginator.pageSize;
            return data.splice(startIndex, this._paginator.pageSize);
        });*/
    }

    disconnect() { }

    /** Returns a sorted copy of the database data. */
    /*getSortedData(): UserData[] {
        const data = this._exampleDatabase.data.slice().filter((item: UserData) => {
            let searchStr = (item.name + item.color).toLowerCase();
            return searchStr.indexOf(this.filter.toLowerCase()) != -1;
        });

        if (!this._sort.active || this._sort.direction == '') { return data; }

        return data.sort((a, b) => {
            let propertyA: number | string = '';
            let propertyB: number | string = '';

            switch (this._sort.active) {
                case 'userId': [propertyA, propertyB] = [a.id, b.id]; break;
                case 'userName': [propertyA, propertyB] = [a.name, b.name]; break;
                case 'progress': [propertyA, propertyB] = [a.progress, b.progress]; break;
                case 'color': [propertyA, propertyB] = [a.color, b.color]; break;
            }

            let valueA = isNaN(+propertyA) ? propertyA : +propertyA;
            let valueB = isNaN(+propertyB) ? propertyB : +propertyB;

            return (valueA < valueB ? -1 : 1) * (this._sort.direction == 'asc' ? 1 : -1);
        });
    }*/
}