import { Component, HostListener, Inject, OnInit, ViewChild } from '@angular/core';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { FileUpload } from 'primeng/fileupload';
import { HandleLinkYoutubePipe } from '../../pipes/handle-link-youtube.pipe';
import { BaseConsts, ContentTypeEView, EAcceptFile, ETypeHandleLinkYoutube } from '../../shared/consts/base.consts';
import { Utils } from '../../shared/utils';
import { IDialogUploadFileConfig } from '../../shared/interfaces/base.interface';
import { IEnvironment } from '../../shared/interfaces/environment.interface';
import { LibHelperService } from '../../shared/services/lib-helper.service';
import { CropImageComponent } from './crop-image/crop-image.component';
import { Observable, Subscription, finalize, forkJoin, pipe } from 'rxjs';
import { FileHelperService } from '../../shared/services/file-helper.service';


@Component({
  selector: 'lib-upload',
  templateUrl: './upload.component.html',
  providers: [HandleLinkYoutubePipe],
})

export class UploadComponent {
    constructor(
        public ref: DynamicDialogRef,
        public config: DynamicDialogConfig,
        private _helperService: LibHelperService,
        private _handleYoutubePipe: HandleLinkYoutubePipe,
        private _fileHelperService: FileHelperService,
        @Inject('env') environment
    ) {
        this.environment = environment;
    }

    BaseConsts = BaseConsts;

    environment: IEnvironment;
    // CONSTS
    EAcceptFile = EAcceptFile;
    ETypeHandleLinkYoutube = ETypeHandleLinkYoutube;
    Utils = Utils;
    //
    isUploading: boolean = false;
    dialogData: IDialogUploadFileConfig = {};
    inputValue: string;
	uploadSub: Subscription;

    @ViewChild('pUpload') pUpload: FileUpload;
    fileInput: HTMLElement;

	@HostListener('window:popstate', ['$event'])
    onPopState(event: Event) {
        this.ref.close();
    }

    ngOnInit(): void {
		const quantityMaxFileUpload = 5;
        let data: IDialogUploadFileConfig = this.config.data;
        const chooseLabel = data?.accept === EAcceptFile.IMAGE ? "Tải lên" : (data?.accept === EAcceptFile.VIDEO ? "Chọn video tải lên" : "Chọn file tải lên");
        this.dialogData  = {
            ...data,
            folderUpload: data.folderUpload || this.getFolderUpload(),
            uploadServer: !(data.uploadServer === false),
            multiple: !!data.multiple,
            accept: data.accept || EAcceptFile.ALL,
            previewBeforeUpload: data.previewBeforeUpload === undefined ? false : data.previewBeforeUpload,
            isChooseNow: data.isChooseNow === undefined ? true : data.isChooseNow,
            inputValue: data.inputValue,
            inputRequired: data.inputRequired,
            chooseLabel: data.chooseLabel || chooseLabel,
            quantity: data.quantity || quantityMaxFileUpload,
        };
        this.inputValue = this.dialogData.inputValue || '';
        // ẨN MODAL KHI CHỌN FILE
        if(this.dialogData.isChooseNow) {
            const elements: any = document.querySelectorAll('.p-dialog-mask-scrollblocker');
            elements[elements.length - 1].style.opacity = 0;
        }
    }

    getFolderUpload(): string {
        return 'files';
    }

    ngAfterViewInit() {
        if(this.dialogData.isChooseNow) {
            this.pUpload.choose();
            // ẨN DIALOG_MODAL KHI CLICK CANCEL KHÔNG CHỌN FILE
            const elementPUpload = document.getElementsByClassName("e-file-upload");
            this.fileInput = elementPUpload[0].getElementsByTagName("input")[0];
            this.fileInput.addEventListener("cancel", this.hideDialogListen);
        }
    }

    // validTypeFile(type, file): boolean {
    //     let extension = file?.name?.split('.')?.pop();
    //     let accept: string;
    //     switch(type) {
    //         case 'image':
    //             accept = BaseConsts.imageExtensionStrings;
    //             break;
    //         case 'video':
    //             accept = BaseConsts.videoExtensionStrings;
    //             break;
    //         case 'media':
    //             accept = BaseConsts.imageExtensionStrings + ',' + BaseConsts.videoExtensionStrings;
    //             break;
    //         default:
    //             accept = type || '';
    //     }

    //     if(!type || !extension) return !!(!type);
    //     return !!(type && extension && accept.includes(extension.toLowerCase()));
    // }

    hideDialogListen = () => {
        this.hideDialog()
    }

    hideDialog(): any {
        this.ref.close();
    }

    eventFile: Event;
    fileUploadErrors:any[] = [];
    onSelectedFiles(event?: any) {
		let messageError = '';
        // if(!this.validTypeFile(this.dialogData?.accept, event.files[0]) || !event?.currentFiles?.length) {
        if(!event?.currentFiles?.length) {
            messageError = 'File không hợp lệ';
        } 
		//
        let isOverloadSize: boolean = false;
        for(let i=0; i <event?.currentFiles?.length; i++){
            if(event?.files[i]?.size > BaseConsts.maxSizeUpdate){
                isOverloadSize = true;
                this.fileUploadErrors.push(event.files[i]);
            } 
        }

        if(isOverloadSize && !messageError) {
			messageError = 'File vượt quá dung lượng cho phép (10MB)';
		}
        
		if(messageError && !this.files.length && event?.currentFiles?.length === this.fileUploadErrors?.length) {
			this._helperService.messageError(messageError);
			this.hideDialog();
		} else {
			if(messageError) this._helperService.messageError(messageError);
			if(this.files.length > 0 || event?.currentFiles?.length > 0) {
				event.currentFiles.forEach(file => {
                    // Kiểm tra nếu mảng file upload nhỏ hơn số lượng tối đa, file không nằm trong mảng file lỗi và chưa tồn tại file đó trong mảng file upload
                    if(this.files.length < this.dialogData.quantity && !this.fileUploadErrors.includes(file) && !this.files.includes(file)) {
						this.files.push(file);
					}
				})
			} else {
				this.files = event.currentFiles.slice(0, this.dialogData.quantity);
			}
			this.eventFile = event.originalEvent;
			if(this.dialogData.accept === EAcceptFile.VIDEO || Utils.checkLinkYoutube(this.inputValue)) {
				// Nếu chọn upload video thì remove đường dẫn
				this.inputValue = '';
			}
			//
			if(this.dialogData.previewBeforeUpload === false) {
				this.config.data.isLoading = true;
				this.onUpload();
			} else {
				// HIỆN MODEL SAU KHI CHỌN XONG FILE
				const elements: any = document.querySelectorAll('.p-dialog-mask-scrollblocker');
				elements[elements.length - 1].style.opacity = 1;
				if(this.fileInput) {
					this.fileInput.removeEventListener("cancel", this.hideDialogListen);
				}
			}
		}
       
    }

    changeValue(inputValue) {
        if(this._handleYoutubePipe.checkLinkYoutube(inputValue)) {
            this.inputValue = this._handleYoutubePipe.getLinkWatchYoutube(inputValue);
            this.files = [];
        }
    }

    fileType = (file: File) => {
        return file.type.split('/')[0] || EAcceptFile.IMAGE;
    }

    getBlobVideo = (file: File) => {
        const blobVideo = this._helperService.sanitizer.bypassSecurityTrustUrl(URL.createObjectURL(file));
        return blobVideo || '';
    }

    files: File[]= [];
    preview(image) {
        this._helperService.dialogViewerRef(
            image,
            ContentTypeEView.IMAGE,
        );
    }

    cropImage(fileImage, index) {
        const ref = this._helperService.dialogService.open(
            CropImageComponent,
            {
                header: 'Chỉnh sửa ảnh',
                width: 'auto',
                style: {'min-width': '500px', 'max-height': '100%'},
                data: {
                    fileImage: fileImage,
                    eventFile: this.eventFile,
                }
            }
        ).onClose.subscribe((fileCrop) => {
            if(fileCrop) {
                this.files[index] = fileCrop;
            }
        })
    }

    removeFile(index: number) {
        this.files.splice(index, 1);
    }

    onUpload() {
        if((this.dialogData.inputRequired && this.inputValue.trim()) || !this.dialogData.inputRequired) {
            if(this.dialogData.uploadServer && this.files.length > 0) {
                 //
                let fileExtensionNotSupports: string[] = [];
                // const getParamFile = (file): string => {
                //     let param: string;
                //     const fileName = file?.name;
                //     const extension = file?.name ? fileName.split('.').pop().toLowerCase() : '';
                //     if(BaseConsts.imageExtensions.includes(extension)) param = 'images';
                //     if(BaseConsts.fileExtensions.includes(extension)) param = 'files';
                //     if(BaseConsts.videoExtensions.includes(extension)) param = 'videos';
                //     //
                //     return param;
                // }

                let formData = new FormData();
                let quantity: number = +this.dialogData.quantity;

                this.files.forEach((file, index) => {
                    if((quantity && index < quantity) || !quantity) {
                        formData.append(`file`, file);
                    }
                    // const paramRequest = getParamFile(file);
                    // if(paramRequest) {
                     
                    // } else {
                    //     const extension = file?.name.split('.').pop();
                    //     extension && !fileExtensionNotSupports.includes(extension)
                    //     && fileExtensionNotSupports.push(extension);
                    // }
                });
                //
                if(this.dialogData?.callback) {
                    // Gọi lại component gốc để tương tác
                    this.dialogData.callback();
                }

				this.isUploading = true;
                if(this.dialogData.service) this.dialogData.service.setUploadMedia(true);
                this.uploadSub = this._fileHelperService.uploadFiles(formData, this.dialogData.folderUpload)
                .pipe(finalize(() => { 
                    if(this.dialogData.service) this.dialogData.service.setUploadMedia(false)
                }))
                .subscribe({
                    next: (res) => {
                        if(res) {
                            if(fileExtensionNotSupports?.length) {
                                this._helperService.messageError('không được hỗ trợ', `Định dạng file: ${fileExtensionNotSupports.join(', ')}`, 3500);
                            }
                  
                            this.ref.close({
                                inputData: this.inputValue,
                                fileUrls: res?.data, 
                            })
                        }
                    },
                    error: (err) => {
                        this.ref.close();
                    }
                })
            } else {
                this.ref.close({
                    inputData: this.inputValue,
                    fileUrls: this.files, 
                })
            }
        } else {
            this._helperService.messageError(`Vui lòng nhập ${this.dialogData.titleInput}`);
        }
    }

	ngOnDestroy() {
		if(this.uploadSub) this.uploadSub.unsubscribe();
	}
}
