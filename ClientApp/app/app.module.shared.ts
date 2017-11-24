
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ShareDialogComponent } from './components/shared/share-dialog.component';
import { MaterialModule } from './components/material/material-imports.module';
import { AuthGuard } from './components/user/auth-guard.service';


@NgModule({
    declarations: [
        ShareDialogComponent,
    ],
    entryComponents: [ShareDialogComponent],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        MaterialModule
    ],
    exports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        MaterialModule,
        ShareDialogComponent
    ],
    providers: [
        AuthGuard
    ]
})
export class AppModuleShared {
}
