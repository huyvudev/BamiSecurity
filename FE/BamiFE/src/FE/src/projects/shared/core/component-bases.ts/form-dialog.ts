import { Directive, inject } from "@angular/core";
import { MixinFormBasic } from "../minxin-component-base.ts/form-basic";
import { DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";
import { MixinBase } from "../minxin-component-base.ts/base";

@Directive()
export abstract class FormDialog extends MixinFormBasic(MixinBase(class{})) { 

	dialogRef: DynamicDialogRef = inject(DynamicDialogRef);
    dialogConfig: DynamicDialogConfig = inject(DynamicDialogConfig);
	//
    closeDialog(data?: any, delay?: boolean) {
		if(delay) {
			// Fix lỗi theme submit form nhưng không call api. Đóng dialog luôn sẽ bị reload lại trang
			setTimeout(() => {
				this.dialogRef.close(data);
			}, 0);
			return;
		}
		//
		this.dialogRef.close(data);		
    }
}