import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'share-dialog',
    templateUrl: 'share-dialog.component.html',
})
export class ShareDialogComponent {
    email: string;
    selectedPermission: number;

    constructor(
        public dialogRef: MatDialogRef<ShareDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any) { }

    onNoClick(): void {
        this.dialogRef.close();
    }

    onShareClick() {
        this.dialogRef.close(<any>{
            email: this.data.email,
            permission: this.selectedPermission
        });
    }

}