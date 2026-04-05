import { Injectable } from "@angular/core";
import { DomSanitizer, SafeHtml, SafeUrl } from "@angular/platform-browser";
import { ActivatedRoute, Router } from "@angular/router";
import { MessageService } from "primeng/api";
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";
import { Location } from "@angular/common";
import { ConfirmComponent } from "../../component/confirm/confirm.component";
import { UploadComponent } from "../../component/upload/upload.component";
import { ViewComponent } from "../../component/view/view.component";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { BehaviorSubject, Observable, catchError, finalize, firstValueFrom, forkJoin, map, mergeMap, of, timeout } from "rxjs";
import { BaseConsts, ContentTypeEView, EIconConfirm, StatusResonse } from "../consts/base.consts";
import { IDialogConfirmConfig, IDialogUploadFileConfig } from "../interfaces/base.interface";

@Injectable({
    providedIn: 'root'
})

export class LibHelperService {

    constructor(
        public messageService: MessageService,
        public sanitizer: DomSanitizer,
        public dialogService: DialogService,
        public router: Router,
        public activatedRoute: ActivatedRoute,
        public location: Location,
        public http: HttpClient,
    ) { }

    modulePath = window.location.pathname;

    checkStatusResponse(response: any, message?: string): boolean {
        if (response?.status === StatusResonse.SUCCESS) {
            if (message) {
                this.messageService.add({ severity: 'success', summary: '', detail: message, life: 1000 });
            } else if (response?.successFE && message === undefined) {
                // MESSAGE CHUNG CHO CREATE | UPDATE | DELETE KHI KHÔNG TRUYỀN VÀO PARAM MESSAGE
                this.messageService.add({ severity: 'success', summary: '', detail: response.successFE, life: 1000 });
            }
            return true;
        } else {
			//
            let dataMessage = response?.data;
            if (dataMessage && typeof dataMessage === 'object') {
                const message = dataMessage?.[0]?.value?.[0] || dataMessage?.[Object.keys(dataMessage)?.[0]];
                this.messageService.add({ severity: 'error', summary: '', detail: message, life: 2500 });
            } else {
                let message = response?.message;
                if (response?.code > 1 && response?.code < 1000) {
                    message = BaseConsts.messageError;
                }
                this.messageService.add({ severity: 'error', summary: '', detail: response?.message, life: 2500 });
            }
            return false;
        }
    }

    messageError(msg = '', summary = '', life = 2500) {
        this.messageService.add({ severity: 'error', summary, detail: msg, life: life });
    }

    messageSuccess(msg = '', summary = '', life = 2000) {
        this.messageService.add({ severity: 'success', summary, detail: msg, life: life });
    }

    messageWarn(msg = '', life = 3000) {
        this.messageService.add({ severity: 'warn', summary: '', detail: msg, life: life });
    }

    getAtributionPopupConfirmDialog(message: string, icon: EIconConfirm = EIconConfirm.WARNING, reason: boolean) {
        return {
            header: "Thông báo",
            with: 'auto',
            style: { 'min-width': '350px' },
            data: {
                message: message,
                icon: icon,
                reason: reason,
            },
        } as DynamicDialogConfig;
    }

    getBlobUrlImage(image: File): SafeUrl {
        return this.sanitizer.bypassSecurityTrustUrl(image?.['objectURL']?.['changingThisBreaksApplicationSecurity']);
    }

    getContentHtml(content): SafeHtml {
        return this.sanitizer.bypassSecurityTrustHtml(content);
    }

    checkValidForm(isValid: boolean,) {
        if (!isValid) this.messageError('Vui lòng nhập đủ thông tin');
    }

    dialogConfirmRef(message: string, options?: IDialogConfirmConfig): DynamicDialogRef {
        return this.dialogService.open(
            ConfirmComponent,
            {
                header: options?.header || "Thông báo",
                width: options?.width || '560px',
				styleClass: options?.styleClass || '',
				maskStyleClass: options?.maskStyleClass || '',
                data: {
                    message: message,
                    icon: options?.icon || EIconConfirm.WARNING,
                    ...(options || {}),
                },
            },
        );
    }

    dialogUploadRef(params?: IDialogUploadFileConfig): DynamicDialogRef {
        return this.dialogService.open(
            UploadComponent,
            {
                header: params?.header || 'Preview Media',
                width: params?.width || '650px',
                data: {
                    ...(params || {}),
                },
            }
        )
    }

    dialogViewerRef(content: any, type: ContentTypeEView, params?: DynamicDialogConfig) {
        params = {
            ...(params || new DynamicDialogConfig()),
            contentStyle: params?.contentStyle || {},
        }
        //
        if (type === ContentTypeEView.FILE) {
            params.contentStyle['padding-top'] = 0;
            params.contentStyle['padding-bottom'] = 0;
        } else {
            if (!params.contentStyle['padding-bottom']) {
                params.contentStyle['padding-bottom'] = '10px';
            }
        }
        //
        params.header = params.header || 'Preview';
        if (type === ContentTypeEView.IMAGE) params.header = '';
        //
        return this.dialogService.open(ViewComponent, {
            header: params.header,
            width: params.width || '100%',
            style: { ...(params.style || {}), 'border-radius': 0 },
            contentStyle: { ...(params.contentStyle || {}) },
            styleClass: params.styleClass + ' height-100 ' + (type === ContentTypeEView.IMAGE && ' no-background '),
            data: {
                content: content,
                type: type,
            },
        });
    }

    // Xử lý isLoading khi upload hoặc move ảnh
    private $isUploadMedia = new BehaviorSubject(false);

    public get IsUploadMediaObservable(): Observable<any> {
        return this.$isUploadMedia.asObservable();
    }

    public setUploadMedia(value?: boolean) {
        this.$isUploadMedia.next(value);
    }

    messageTimeout = "Vui lòng kiểm tra lại đường truyền mạng. Thời gian phản hồi tối đa 15s!";
   
    downloadFile(url: string, unAuthorize = false): Observable<any> {
        return this.http.get(url, { responseType: 'blob', params: { unAuthorize: unAuthorize }, observe: 'response' });
    }

    checkDownload(url: string, unAuthorize = false): Observable<any> {
        return this.http.get(url, { params: { unAuthorize: unAuthorize } });
    }

    redirectPageNotFound() {
        this.router.navigate(['/page-not-found'])
    }

    async awaitApi(api: Observable<any>) {
        try {
            const resData = await firstValueFrom(api);
            return resData;
        } catch (error) {
            console.error('Error:', error);
            return null;
        }
    }

    handleForkJoin(apiRequests: Observable<any>[]): Observable<any[]> {
        const handledRequests = apiRequests.map(request =>
            request.pipe(
                catchError(error => {
                    console.log('ForkJoinError: ', error);
                    return of(null); 
                })
            )
        );
        return forkJoin(handledRequests);
    }
}
