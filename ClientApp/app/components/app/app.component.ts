import { Component } from '@angular/core';
import { ReportService } from '../report/report.service';
import { MatSidenavContainer } from '@angular/material'

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    providers: [ ReportService ]
})
export class AppComponent {
}
