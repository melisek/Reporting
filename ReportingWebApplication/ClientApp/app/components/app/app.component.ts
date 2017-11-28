import { Component } from '@angular/core';
import { ReportService } from '../report/report.service';
import { MatSidenavContainer } from '@angular/material'
import { Title } from '@angular/platform-browser';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css', '../shared/shared-styles.css'],
    providers: [ ReportService ]
})
export class AppComponent {
    public constructor(private titleService: Title) { }

    public setTitle(newTitle: string) {
        this.titleService.setTitle(newTitle);
    }
}
