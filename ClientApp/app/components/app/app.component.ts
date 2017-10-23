import { Component } from '@angular/core';
import { ReportService } from '../report/report.service';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    providers: [ ReportService ]
})
export class AppComponent {
}
