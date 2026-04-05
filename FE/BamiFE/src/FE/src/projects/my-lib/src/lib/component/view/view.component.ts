import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Image } from 'primeng/image';
import { ContentTypeEView } from '../../shared/consts/base.consts';
import { LibHelperService } from '../../shared/services/lib-helper.service';

@Component({
  selector: 'lib-view',
  templateUrl: './view.component.html',
})

export class ViewComponent {
    constructor(
        public ref: DynamicDialogRef,
        public config: DynamicDialogConfig,
        public _helperService: LibHelperService,
    ) { }

    ContentTypeEView = ContentTypeEView;
    dialogData: {
        content: any;
        type: ContentTypeEView;
    }

    isLoading: boolean = false;
    contentHeight: string = '';

    ngOnInit(): void {
        this.dialogData = this.config.data;
        this.removeImageMaskOther();
        this.showImage();
    }

    @HostListener('window:popstate', ['$event'])
    onPopState(event: Event) {
        this.ref.close();
    }

    hideDialog() {
        this.ref.close();
    }

    ngAfterViewInit() {
        const contentHeight = document.querySelectorAll(".p-dialog-content")[0].clientHeight;
        this.contentHeight = contentHeight ? contentHeight+'px' : '75vh';
        this.removeImageMaskOther();
    }

    // Xóa các thẻ ImageMask khác
    removeImageMaskOther() {
        const elementImageMask: any = document.querySelectorAll(".p-image-mask");
        for(let i=0; i < elementImageMask.length; i++) {
            if(elementImageMask[i]) elementImageMask[i].remove();
        }
    }

    genHtml = () => {
        return this._helperService.getContentHtml(this.dialogData.content);
    }

    src: SafeUrl | string;
    @ViewChild('pImage') pImage: Image;
    showImage() {
        if(this.dialogData.type === ContentTypeEView.IMAGE) {
            setTimeout(() => {
                if(this.dialogData.content instanceof File) {
                    // content dạng file xử lý hiển thị local khi chưa upload sever
                    this.src = this._helperService.getBlobUrlImage(this.dialogData.content);
                    // File Crop
                    if(!this.src?.['changingThisBreaksApplicationSecurity']) {
                        this.src = this._helperService.sanitizer.bypassSecurityTrustUrl(this.dialogData.content?.['objectUrl']);
                    }
                } else {
                    // Đường dẫn ảnh online
                    this.src = this.dialogData.content;
                }
                this.pImage.onImageClick();
            }, 0);
        }
    }
}
