
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ShareDialogComponent } from './components/shared/share-dialog.component';
import { MaterialModule } from './components/material/material-imports.module';


@NgModule({
    declarations: [
        ShareDialogComponent,
    ],
    entryComponents: [ShareDialogComponent],
    imports: [
        CommonModule,
        FormsModule,
        MaterialModule
    ],
    exports: [
        CommonModule,
        FormsModule,
        MaterialModule,
        ShareDialogComponent
    ],
    providers: [
    ]
})
export class AppModuleShared {
}
