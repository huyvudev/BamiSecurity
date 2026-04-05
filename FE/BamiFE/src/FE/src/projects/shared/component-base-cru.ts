import { ChangeDetectorRef, DestroyRef, Directive, HostListener, Injector, NgZone, inject } from "@angular/core";
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, ValidatorFn } from "@angular/forms";
import moment from "moment";
import { FileHelperService, LibHelperService } from "projects/my-lib/src/public-api";
import { TokenService } from "../my-lib/src/lib/shared/services/auth/token.service";
import { environment } from "./environments/environment";
import Cryptr from 'cryptr';
import { PermissionsService } from "projects/my-lib/src/lib/shared/services/auth/permission.service";
import { Page } from "projects/my-lib/src/lib/shared/models/page";
import { Utils } from "projects/my-lib/src/lib/shared/utils";
import { ActiveDeactiveConst, BaseConsts, ErrorMessage, SuccessMessage, EYesNo, DefaultImage, EActiveDeactive, EFormatDate, EIconConfirm } from "projects/my-lib/src/lib/shared/consts/base.consts";
import jwt_decode from "jwt-decode";
import { IEventButtonForm, IUserDecodeToken } from "projects/my-lib/src/lib/shared/interfaces/base.interface";
import { PermissionCoreConst } from "./consts/permissionWeb/PermissionCoreConfig";
import { Observable, of } from "rxjs";
import { DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";
/**
 * Component base cho tất cả app
 */

@Directive()
export abstract class ComponentBaseCRU {

    _libHelperService: LibHelperService = inject(LibHelperService);
    _fileHelperService: FileHelperService = inject(FileHelperService);
    _permissionService: PermissionsService = inject(PermissionsService);
    _tokenService: TokenService = inject(TokenService);
    //
    fb: FormBuilder = inject(FormBuilder);
    form: FormGroup;

    //
    dialogRef: DynamicDialogRef = inject(DynamicDialogRef);
    dialogConfig: DynamicDialogConfig = inject(DynamicDialogConfig);
    ngZone: NgZone = inject(NgZone);
    cd: ChangeDetectorRef = inject(ChangeDetectorRef);
    destroyRef = inject(DestroyRef);

    //
    utils = Utils;

    // CONST
    BaseConsts = BaseConsts;
    ErrorMessage = ErrorMessage;
    SuccessMessage = SuccessMessage;
    YesNoConst = EYesNo;
    EActiveDeactive = EActiveDeactive;
    ActiveDeactiveConst = ActiveDeactiveConst;
    IconConfirm = EIconConfirm;
    //
    PermissionCoreConst = PermissionCoreConst;
    
    DefaultImage = DefaultImage;

    dialog_footer = "p-dialog-footer e-custom-dialog-footer";
    
    constructor() {
        this._permissionService.getPermissions();
    }

    cryptr = new Cryptr('encodeId', { encoding: 'hex', pbkdf2Iterations: 1, saltLength: 1 });
    screenHeight = window.innerHeight;
    screenWidth = window.innerWidth;
    isLoading: boolean = false;
    isEdit: boolean = false;
    activeIndex: number = 0;

    public uploadApi: string = `${environment.api}/api/file/upload?folder=core/media` 

    page: Page = new Page();
    dateNow = new Date(); // Ngày hiện tại

    @HostListener("window:popstate", ["$event"])
    onPopState(event) {
        if (event) {
            this.dialogCancel();
        }
    }

    isGranted(permissionNames: any): boolean {
        return true;
        permissionNames = (typeof permissionNames === 'string') ? [permissionNames] : permissionNames;
        return this._permissionService.isGrantedRoot(permissionNames);
    }
   
    markControls(formGroup: FormGroup, skipValidationForFields: string[] = []) {
        for (const i in formGroup.controls) {
            if (!skipValidationForFields.includes(i)) {
                formGroup.controls[i].markAsDirty();
            } else {
                formGroup.controls[i].clearValidators();
            }
            formGroup.controls[i].updateValueAndValidity({ emitEvent: false });
        }
    }

    addControl(name: string, value: any = null, validators?: ValidatorFn) {
        this.form.addControl(name, new FormControl(value, validators));
    }

    control(name: string) {
        return this.form?.get(name);
    }

    controlValue(name: string) {
        return this.form?.get(name)?.value;
    }

    formValue(names?: string[]) {
        if(Array.isArray(names) && names?.length) {
            return this.form?.get(names).value;
        }
        return this.form?.value;
    }

    formRawValue() {
        return this.form?.getRawValue();
    }

    getPropertiesForm(form = this.form) {
        return Object.keys(form?.controls);
    }

    public inValidForm(formGroup: FormGroup = this.form, skipValidationForFields: string[] = []): boolean {
        // Duyệt qua tất cả các controls trong formGroup
        this.markControls(formGroup, skipValidationForFields);

        // Duyệt qua các controls trong FormArray
        Object.keys(formGroup.controls).forEach(key => {
            const control = formGroup.controls[key];
            if (control instanceof FormArray) {
                control.controls.forEach((controlItem: FormGroup | AbstractControl) => {
                    // Gọi hàm markControls cho mỗi control trong FormArray
                    if (controlItem instanceof FormGroup) {
                        this.markControls(controlItem, skipValidationForFields);
                    }
                });
            }
        });

        // Kiểm tra xem có bất kỳ control nào INVALID không
        let isInvalid = false;
        Object.keys(formGroup.controls).forEach(key => {
            if (formGroup.controls[key].status === "INVALID") {
                Utils.log(`invalid`, { controlName: key, controls: formGroup.controls[key] });
                isInvalid = true;
            }
        });
        if(isInvalid){
            this.messageError(ErrorMessage.ERR_INVALID_FORM)
        }
        return isInvalid;
    }

    cryptEncode(id): string {
        try {
            return this.cryptr.encrypt(id.toString());
        } catch (err) {
            return null;
        }
    }

    cryptDecode(codeId, isRedirect: boolean = true) {
        try {
            return this.cryptr.decrypt(codeId.toString());
        } catch (err) {
            if (isRedirect) this._libHelperService.redirectPageNotFound();
            return null;
        }
    }

    checkStatusResponse(response, message?: string): boolean {
        return this._libHelperService.checkStatusResponse(response, message);
    }

    messageError(msg = '', summary = '', life = 3000) {
        return this._libHelperService.messageError(msg, summary, life);
    }

    messageSuccess(msg = '', summary = '', life = 1000) {
        return this._libHelperService.messageSuccess(msg, summary, life);
    }

    messageWarn(msg = '', life = 3000) {
        return this._libHelperService.messageWarn(msg, life);
    }

    // lấy giá trị mặc định của obj
    getPropertyObj(obj, path, type: string = '') {
        if (!path) return null
        const value = path.split('.').reduce((o, key) => (o && o[key] !== 'undefined' ? o[key] : null), obj);
        if (type === 'calendar') {
            return value ? new Date(value) : null;
        }
        return value;
    }

    getUser() {
        const token = this._tokenService.getToken();
        let userInfo: IUserDecodeToken;
        if(token) {
            userInfo = jwt_decode(token);
            return userInfo;
        }
        return userInfo;
    }

    timeOutDialog: any;
	observableFake = (): Observable<any> => of(null);
    public dialogCancel(data?: any) {
		if(this.timeOutDialog) clearTimeout(this.timeOutDialog);
		this.timeOutDialog = setTimeout(() => {
			this.dialogRef.close(data);
		}, 50);
    }

    dialogSubmit(data?: any) {
        if(this.timeOutDialog) clearTimeout(this.timeOutDialog);
        this.timeOutDialog = setTimeout(() => {
            this.dialogRef.close(data || true);
        }, 50);
    }

	pageCancel() {
		
	}

    public activeEdit(fieldDisables: string[] = []) {
        this.form?.enable();
        this.isEdit = true;
    }

    // uploadFile() {
	// 	const ref = this._libHelperService.dialogUploadRef({
	// 		accept: '.docx, .pdf',
	// 	});
    //     //
	// 	ref.onClose.subscribe((response) => {
    //         if (response && response.fileUrls.length > 0) {
	// 			this.control('url').setValue(response.fileUrls[0]?.url);
	// 		}
	// 	});
	// }

    getConfigDialogServiceRAC(title: string, params: any) {
        return {
            header: title,
            width: '600px',
            baseZIndex: 10000,
            data: {
                id: params.id,
                summary: params?.summary,
            },
        };
    }

	async confirmAction(message: string, icon?: EIconConfirm): Promise<boolean> {
		const responseConfirm = await this._libHelperService.awaitApi(this._libHelperService.dialogConfirmRef(
			message, {
				icon: icon || EIconConfirm.QUESTION
			}
		).onClose);
		//
		return !!responseConfirm?.accept;
	}

	async getUploadedFileUrl(accept: string = 'image'): Promise<boolean> {
		const res = await this._libHelperService.awaitApi(this._libHelperService.dialogUploadRef({ accept }).onClose);
		if(res && res?.fileUrls?.length > 0) {
			return res.fileUrls[0]?.url
		}
		return null;
	}

    protected save(event?: IEventButtonForm, activeSubmit: boolean = false) {
        //        
        if(!this.inValidForm()) {
            if(event?.hasChangeData || !event || event?.actionType === 'create' || activeSubmit) {
                this.onSubmit(event);
            } else {
                this.dialogCancel();
            }

            if(event && !event?.hasChangeData) {
                this.dialogCancel();
                return;
            }
            //
            this.onSubmit(event);
        } 
    }
    //
    protected abstract onSubmit(event?: any): void;

    protected formatCalendarItemSendApi(datetime: Date) {
        if (!(datetime instanceof Date)) {
            return null
        }
        return moment(datetime, EFormatDate.DATE_YMD_Hms).format(EFormatDate.DATE_YMD_Hms);
    }
}
