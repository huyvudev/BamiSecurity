import { DestroyRef, Directive, Inject, Injector, inject } from "@angular/core";
import { AbstractControl, FormArray, FormGroup } from "@angular/forms";
import moment from "moment";
import { FileHelperService, FormApproveComponent, FormRequestComponent, LibHelperService } from "projects/my-lib/src/public-api";
import { TokenService } from "../my-lib/src/lib/shared/services/auth/token.service";
import { environment } from "./environments/environment";
import Cryptr from 'cryptr';
import { PermissionsService } from "projects/my-lib/src/lib/shared/services/auth/permission.service";
import { Page } from "projects/my-lib/src/lib/shared/models/page";
import { Utils } from "projects/my-lib/src/lib/shared/utils";
import { BaseConsts, ErrorMessage, SuccessMessage, EYesNo, DefaultImage, EActiveDeactive, EFormatDate, EIconConfirm, ActiveDeactiveConst, ActiveDeactiveTextConst } from "projects/my-lib/src/lib/shared/consts/base.consts";
import jwt_decode from "jwt-decode";
import { BehaviorSubject, ReplaySubject, Subject } from "rxjs";
import { ApproveModel, RequestModel } from "projects/my-lib/src/lib/shared/models/base.model";
import { ETableColumnType, ETableFrozen } from "@mylib-shared/consts/lib-table.consts";
import { IColumn, IColumnConfig } from "@mylib-shared/interfaces/lib-table.interface";
/**
 * Component base cho tất cả app
 */

@Directive()
export abstract class ComponentBase {

    _libHelperService: LibHelperService;
    _fileHelperService: FileHelperService;
    _permissionService: PermissionsService;
    _tokenService: TokenService;
	destroyRef: DestroyRef;
    utils = Utils;

    // CONST
    BaseConsts = BaseConsts;
    ErrorMessage = ErrorMessage;
    SuccessMessage = SuccessMessage;
    YesNoConst = EYesNo;
    EActiveDeactive = EActiveDeactive;
    ActiveDeactiveConst = ActiveDeactiveConst;
    ActiveDeactiveTextConst = ActiveDeactiveTextConst;

    IconConfirm = EIconConfirm;
    //

    DefaultImage = DefaultImage;

	subject = new Subject<any>();
	behaviorSubject = new BehaviorSubject<any>(null);

    constructor(
        injector: Injector,
    ) {
        this._fileHelperService = injector.get(FileHelperService);
        this._libHelperService = injector.get(LibHelperService);
        this._permissionService = injector.get(PermissionsService);
        this._tokenService = injector.get(TokenService);
        this.destroyRef = injector.get(DestroyRef);
        this._permissionService.getPermissions();
    }

    cryptr = new Cryptr('encodeId', { encoding: 'hex', pbkdf2Iterations: 1, saltLength: 1 });
    screenHeight = window.innerHeight;
    screenWidth = window.innerWidth;
    isLoading: boolean = false;

    public uploadApi: string = `${environment.api}/api/media/upload`


    submitted: boolean = false;
    onClickSubmit: boolean = false;

    isValid: boolean = false;
    isSpinner: boolean = false;

    page: Page = new Page('ComponentBase');

    activeIndex = 0;

    blockText: RegExp = /[0-9,.]/;
    number: RegExp = /[0-9]/;
    numberPercent: RegExp = /[0-9,]/;
    blockSpace: RegExp = /[^\s]/;
    regexTaxcode: RegExp = /[0-9-]/;

    destroyed$: ReplaySubject<boolean> = new ReplaySubject(3);

    dateNow = new Date(); // Ngày hiện tại

    bodyEl: any;
    btnUpdatePageClass: string = 'flex justify-content-end gap-3';

    isGranted(permissionNames: any): boolean {
        return true;
        permissionNames = (typeof permissionNames === 'string') ? [permissionNames] : permissionNames;
        return this._permissionService.isGrantedRoot(permissionNames);
    }

    protected formatCalendarItemSendApi(datetime: Date) {
        if (!(datetime instanceof Date)) {
            return null
        }
        return moment(datetime, EFormatDate.DATE_YMD_Hms).format(EFormatDate.DATE_YMD_Hms);
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

    public inValidForm(formGroup: FormGroup, skipValidationForFields: string[] = []): boolean {
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
        if (isInvalid) {
            this.messageError(ErrorMessage.ERR_INVALID_FORM)
        }
        return isInvalid;
    }

    protected formatCalendar(fields, model) {
        for (let field of fields) {
            if (model[field]) model[field] = this.formatCalendarItem(model[field]);
        }
        return model;
    }

    protected formatCalendarItem(datetime: string) {
        return moment(new Date(datetime)).format("YYYY-MM-DDTHH:mm:ss");
    }

    isInvalidMessage(form: FormGroup, controlName: string): boolean {
        return !!(form?.get(controlName)?.hasError('errorMessage') && form?.get(controlName)?.invalid && form?.get(controlName)?.touched);
    }

    isInvalidMessageInArray(formArray: FormArray, index: number, controlName: string): boolean {
        const control = formArray.at(index).get(controlName);
        return !!(
            control?.hasError('errorMessage') &&
            control?.invalid &&
            control?.touched
        );
    }

    isInvalid(form: FormGroup, controlName: string, onClickSubmit?: boolean): boolean {
        if (onClickSubmit) {
            return onClickSubmit && form?.get(controlName)?.invalid;
        }
        return !!(form?.get(controlName)?.invalid && form?.get(controlName)?.touched);
    }

    convertObjectToFormData(object, formData: FormData = new FormData(), parentKey = null, arrayIndex = null): FormData {
        for (const [key, value] of Object.entries(object)) {
            const finalKey = parentKey ? (arrayIndex !== null ? `${parentKey}[${arrayIndex}].${key}` : `${parentKey}.${key}`) : key;
            if (Array.isArray(value)) {
                for (let i = 0; i < value.length; i++) {
                    const arrayItemKey = `${finalKey}[${i}]`;
                    if (typeof value[i] === 'object' && value[i] !== null) {
                        this.convertObjectToFormData(value[i], formData, finalKey, i);
                    } else {
                        formData.append(arrayItemKey, String(value[i]));
                    }
                }
            } else if (typeof value === 'object' && value !== null) {
                if (value instanceof File) {
                    // Handle File objects separately and append them to FormData
                    formData.append(finalKey, value, value.name);
                } else {
                    this.convertObjectToFormData(value, formData, finalKey);
                }
            } else {
                formData.append(finalKey, String(value));
            }
        }

        return formData;
    }

    getConfigDialogServiceRAC(title: string, params?: any) {
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

    checkValidForm(isValid: boolean) {
        if (!isValid) this.messageError('Vui lòng nhập đủ thông tin!');
        return isValid;
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

    viewFile(fileUrl) {
        this.openViewFile(fileUrl)
    }

    openViewFile(fileUrl) {
        const url = `${environment.api}/${fileUrl}`;
        if (Utils.isPdfFile(fileUrl)) {
            window.open(url, "_blank");
        } else {
            this._libHelperService.downloadFile(url).subscribe((res) => {
                Utils.viewWordConvertToPdf(res);
            });
        }
    }

    async confirmRequest(messageTitle = 'Trình duyệt'): Promise<RequestModel | undefined> {
        const responseConfirm = await this._libHelperService.awaitApi(this._libHelperService.dialogService.open(
            FormRequestComponent,
            this.getConfigDialogServiceRAC(messageTitle)
        ).onClose);
        //
        return responseConfirm;
    }
	
    async confirmApprove(messageTitle = 'Xử lý phê duyệt'): Promise<ApproveModel | undefined> {
        const responseConfirm = await this._libHelperService.awaitApi(this._libHelperService.dialogService.open(
            FormApproveComponent,
            this.getConfigDialogServiceRAC(messageTitle)
        ).onClose);
        //
        return responseConfirm;
    }

    async confirmAction(message: string, icon?: EIconConfirm): Promise<boolean> {
        const responseConfirm = await this._libHelperService.awaitApi(this._libHelperService.dialogConfirmRef(
            message, {
            icon: icon || EIconConfirm.QUESTION
        }
        ).onClose);
        //
        return !!responseConfirm;
    }

    async uploadImage(accept: string = 'image'): Promise<boolean> {
        const res = await this._libHelperService.awaitApi(this._libHelperService.dialogUploadRef({ accept }).onClose);
        if (res && res?.fileUrls?.length > 0) {
            return res.fileUrls[0]?.url
        }
        return null;
    }

    getColumnId(config: IColumnConfig = {}): IColumn {
        const defaultConfig: IColumn = {
            field: 'id',
            header: '#ID',
            width: 5,
            action: undefined,
            isFrozen: true,
            isPin: true,
            alignFrozen: ETableFrozen.LEFT,
            otherType: config.isLink === false ? ETableColumnType.TEXT : ETableColumnType.LINK,
            customValue: (value) => {
                return value = (value?.id ?? value ?? '-').toString().slice(0, 4);
            }
        };
    
        const finalConfig = { ...defaultConfig, ...config };
    
        return finalConfig
    }
    
    getColumnStatus(config: IColumnConfig = {}): IColumn {
        const defaultConfig: IColumn = {
            field: 'status',
            header: 'Trạng thái',
            width: 7.5,
            isFrozen: true,
            alignFrozen: ETableFrozen.RIGHT,
            type: ETableColumnType.STATUS,
            getTagInfo: undefined,
        };
    
        const finalConfig = { ...defaultConfig, ...config };
    
        return finalConfig
    }

    getColumnAction(config: IColumn = {}): IColumn {
        const defaultConfig: IColumn = {
            field: '',
            header: '',
            width: 3,
            displaySettingColumn: false,
            isFrozen: true,
            alignFrozen: ETableFrozen.RIGHT,
            type: ETableColumnType.ACTION_DROPDOWN,
        };
    
        const finalConfig = { ...defaultConfig, ...config };
    
        return finalConfig
    }
    
    formatTimeAgo(inputDate: string): string {
		const date = new Date(inputDate);
		const now = new Date();
		const diffInMs = now.getTime() - date.getTime();
		const diffInSeconds = Math.floor(diffInMs / 1000);
		const diffInMinutes = Math.floor(diffInSeconds / 60);
		const diffInHours = Math.floor(diffInMinutes / 60);
		const diffInDays = Math.floor(diffInHours / 24);
	
		if (diffInSeconds < 60) {
			return `${diffInSeconds} second`;
		} else if (diffInMinutes < 60) {
			return `${diffInMinutes} minute ago`;
		} else if (diffInHours < 24) {
			return `${diffInHours} hours ago`;
		} else {
			return date.toLocaleString('en-US', {
				month: 'short',
				day: 'numeric',
				hour: '2-digit',
				minute: '2-digit',
				hour12: true,
			});
		}
	}
}
