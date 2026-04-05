import { computed, DestroyRef, inject } from "@angular/core";
import { PermissionsService } from "@mylib-shared/services/auth/permission.service";
import { Utils } from "@mylib-shared/utils";
import { MessageService } from "primeng/api";
import Cryptr from 'cryptr';
import { LibHelperService } from "@mylib-shared/services/lib-helper.service";
import { environment } from "@shared/environments/environment";
import { FormApproveComponent, FormRequestComponent } from "projects/my-lib/src/public-api";
import { EIconConfirm } from "@mylib-shared/consts/base.consts";
import { IColumn, IColumnConfig } from "@mylib-shared/interfaces/lib-table.interface";
import { ETableColumnType, ETableFrozen } from "@mylib-shared/consts/lib-table.consts";
import { ApproveModel, RequestModel } from "@mylib-shared/models/base.model";
import { DialogService } from "primeng/dynamicdialog";
import { Observable, of } from "rxjs";

//
type Contructor<T = {}> = new (...args: any[]) => T;
//
export function MixinBase<T extends Contructor>(Base: T) {
	return class extends Base {
		constructor(...args: any []) {
			super(...args)
		}
		
		protected destroyRef: DestroyRef = inject(DestroyRef);
		protected dialogService: DialogService = inject(DialogService);
		//
		protected _permissionService = inject(PermissionsService);
		protected _libHelperService = inject(LibHelperService);

		private cryptr = new Cryptr('encodeId', { encoding: 'hex', pbkdf2Iterations: 1, saltLength: 1 });

		protected Utils = Utils;

		isEdit: boolean = false;
		isLoading: boolean = false;
		observableFake = (): Observable<any> => of(null);

		isGranted (permissioKey: string = '') {
			// return true;
			return this._permissionService.isGrantedRoot(permissioKey);
		}

		protected cryptEncode(id): string {
			try {
				return this.cryptr.encrypt(id.toString());
			} catch (err) {
				return null;
			}
		}
	
		protected cryptDecode(codeId, isRedirect: boolean = true) {
			try {
				return this.cryptr.decrypt(codeId.toString());
			} catch (err) {
				if (isRedirect) this._libHelperService.redirectPageNotFound();
				return null;
			}
		}
	
		protected checkStatusResponse(response, message?: string): boolean {
			return this._libHelperService.checkStatusResponse(response, message);
		}
	
		protected messageError(msg = '', summary = '', life = 3000) {
			return this._libHelperService.messageError(msg, summary, life);
		}
	
		protected messageSuccess(msg = '', summary = '', life = 1000) {
			return this._libHelperService.messageSuccess(msg, summary, life);
		}
	
		protected messageWarn(msg = '', life = 3000) {
			return this._libHelperService.messageWarn(msg, life);
		}
		
		protected viewFile(fileUrl) {
			this.openViewFile(fileUrl)
		}
	
		protected openViewFile(fileUrl) {
			const url = `${environment.api}/${fileUrl}`;
			if (Utils.isPdfFile(fileUrl)) {
				window.open(url, "_blank");
			} else {
				this._libHelperService.downloadFile(url).subscribe((res) => {
					Utils.viewWordConvertToPdf(res);
				});
			}
		}

		protected getConfigDialogServiceRAC(title: string, params?: any) {
			return {
				header: title,
				width: '600px',
				baseZIndex: 10000,
				data: {
					id: params?.id,
					summary: params?.summary,
				},
			};
		}
	
	
		protected async confirmRequest(messageTitle = 'Trình duyệt'): Promise<RequestModel | undefined> {
			const responseConfirm = await this._libHelperService.awaitApi(this._libHelperService.dialogService.open(
				FormRequestComponent,
				this.getConfigDialogServiceRAC(messageTitle)
			).onClose);
			//
			return responseConfirm;
		}
		
		protected async confirmApprove(messageTitle = 'Xử lý phê duyệt'): Promise<ApproveModel | undefined> {
			const responseConfirm = await this._libHelperService.awaitApi(this._libHelperService.dialogService.open(
				FormApproveComponent,
				this.getConfigDialogServiceRAC(messageTitle)
			).onClose);
			//
			return responseConfirm;
		}
	
		protected async confirmAction(message: string, icon?: EIconConfirm): Promise<boolean> {
			const responseConfirm: boolean = await this._libHelperService.awaitApi(this._libHelperService.dialogConfirmRef(
				message, {
				icon: icon || EIconConfirm.QUESTION
			}
			).onClose);
			//
			return !!responseConfirm;
		}
	
		protected async baseUploadFile(accept: string = 'image'): Promise<boolean> {
			const res = await this._libHelperService.awaitApi(this._libHelperService.dialogUploadRef({ accept }).onClose);
			if (res && res?.fileUrls?.length > 0) {
				return res.fileUrls[0]?.url
			}
			return null;
		}
	
		
	}
		
}