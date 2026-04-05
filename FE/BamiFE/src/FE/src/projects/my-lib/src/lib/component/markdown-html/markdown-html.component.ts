import { ChangeDetectorRef, Component, ElementRef, EventEmitter, Inject, Input, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ContentTypeEView } from '../../shared/consts/base.consts';
import { LibHelperService } from '../../shared/services/lib-helper.service';
import { ITempMediaContentImage } from '../../shared/interfaces/markdown-html.interface';
import { DOCUMENT } from '@angular/common';

@Component({
    selector: 'lib-markdown-html',
    templateUrl: './markdown-html.component.html',
})
export class MarkdownHtmlComponent {

    constructor(
        private _helperService: LibHelperService,
        private ref: ChangeDetectorRef,
        private _libHelperService: LibHelperService,
        @Inject('env') environment,
		@Inject(DOCUMENT) private doc: any
    ) {
        this.environment = environment;
    }

    environment: any;
    htmlMarkdownOptions: any = [
        {
            value: ContentTypeEView.MARKDOWN,
            name: ContentTypeEView.MARKDOWN,
        },
        {
            value: ContentTypeEView.HTML,
            name: ContentTypeEView.HTML
        }
    ];

    @Input() disabled: boolean = false;
    @Input() id: string;
    @Input() title: string = 'Loại nội dung';
    @Input() labelContent: string = 'Nội dung';
    @Input() labelContentView: string = 'Nội dung xem trước';
    @Input() isRequired: boolean = false;
    @Input() inValid: boolean = false;
    @Input() height: number;
    @Input() placeholder: string = 'Nhập nội dung';

    @Input() contentType: ContentTypeEView;
    @Output() contentTypeChange = new EventEmitter<string>();

    @Input() content: string;
    @Output() contentChange = new EventEmitter<string>();

    @Input() formGroup: FormGroup;
    @Input() formGroupChange = new EventEmitter<FormGroup>();

    @Input() formControlContent: string;
    @Input() formControlContentType: string;

    listImageContents: ITempMediaContentImage[] = [];
    @Output() imageContentChanges = new EventEmitter<ITempMediaContentImage[]>();


    heightHtml: number;
    heightMarkdown: number;
    textareaRow: number = 13;

	@Input() maxLength: number;

    @Input() baseUrl: string;
    @Input() header: string = 'Chèn hình ảnh';
    @Input() isMoveImage: boolean = true;
    caretPos: number = 0;
	editorId: string;
    isInsertImageHtml: boolean = true;

	editorContentEl: any;

	//
	contentLimit: string;
	editable: boolean = true;
	isFocusEditor: boolean = false;

    ngOnInit(): void {
        if(!this.id) {
            this.id = 'markdown-' + new Date().getTime();
        }
		//
		this.editorId = 'editor-' + new Date().getTime();
        this.setData();
    }

    ngOnChanges(changes: SimpleChanges) {
		if(this.contentType === ContentTypeEView.HTML) this.activeEditorEl();
        this.setData();
    }
	
    setData() {
        if(this.formGroup) {
            this.content = this.formGroup.value?.[this.formControlContent];
            this.contentType = this.formGroup.value?.[this.formControlContentType];
        }
    }

    ngAfterViewInit() {
        this.setHeightContent();
    }

    setHeightContent() {
        setTimeout(() => {
            const elementMarkdownHtml: any = document.getElementById(this.id);
            // TÍNH CHIỀU CAO CỦA MARKDOWN VÀ SET CHO EDITOR_HTML
            // NẾU HIỆN HTML TRƯỚC THÌ SET HEIGHT VÀ DÙNG HEIGHT SET CHO CẢ MARK_DOWN_EDITOR
            let elementTextarea: HTMLElement = elementMarkdownHtml.querySelector(".p-inputtextarea");
            if(elementTextarea) {
                if(this.height) elementTextarea.style.height = this.height+'px';
                this.heightMarkdown = this.height || elementTextarea.offsetHeight;
            }
            const angularEditorToolbar: HTMLElement = elementMarkdownHtml.querySelector(".angular-editor-toolbar");
            if(angularEditorToolbar) {
                this.heightHtml = (this.height || this.heightMarkdown) - angularEditorToolbar.offsetHeight;
            }
            // console.log('heightMarkdown', this.heightHtml, this.heightMarkdown);
            this.heightHtml = this.heightHtml || 250;
            this.ref.detectChanges();
        }, 100);
    }

    onChange(contentType) {
        setTimeout(() => {
            this.isInsertImageHtml = true;
            this.setHeightContent();
            if(this.formGroup) {
                this.formGroup.controls?.[this.formControlContentType].setValue(contentType || this.contentType);
                this.formGroupChange.emit(this.formGroup);
            } else {
                this.contentTypeChange.emit(contentType || this.contentType);
            }
        }, 0);
		//
        if(contentType === ContentTypeEView.MARKDOWN && this.isFocusEditor) this.checkValid();
		if(contentType === ContentTypeEView.HTML) this.activeEditorEl();
    }

	activeEditorEl() {
		if(this.inValid && this.isRequired) this.isFocusEditor = true;
		setTimeout(() => {
			const wrapperEditorEl = document.getElementById(this.editorId);
			this.editorContentEl = wrapperEditorEl?.getElementsByClassName('angular-editor-textarea')[0] as HTMLElement;
			if(this.editorContentEl) {
				this.editorContentEl.addEventListener('focus', this.focusEditor);
			}
		}, 100);
	}

	focusEditor = () => {
		this.isFocusEditor = true;
	}

	isChooseImage: boolean;
    insertImage() {
        const ref = this._libHelperService.dialogUploadRef({
            // folderUpload: 'project-media'
        });
        ref.onClose.subscribe((response) => {
            if (Array.isArray(response.fileUrls)) {
                let imagesUrl = "";
                response.fileUrls.forEach(image => {
                    imagesUrl += `![](${this.environment.apiFile}${image?.url}) \n`;
                });
                let currentContent = this.content || "";
                let newContent = currentContent.slice(0, this.caretPos)
                    + imagesUrl
                    + currentContent.slice(this.caretPos);
                this.content = newContent;
                this.emitContent();
            }
        })
    }

    insertImageInHtml(typeHtml: boolean, listImage) {
        if(typeHtml) {
            if(this.editorContentEl) this.editorContentEl.focus();
            this.doc.execCommand('insertHtml', false, listImage);
        } else {
            // this.imageContentChanges.emit(this.listImageContents);
            let currentContent = this.content || "";
            let newContent = currentContent.slice(0, this.caretPos)
                            + listImage
                            + currentContent.slice(this.caretPos);
            this.content = newContent;
            this.emitContent();
        }
    }

    emitContent(value?: string) {
        if(this.formGroup) {
            // FORM GROUP
            this.formGroup.controls?.[this.formControlContent].setValue(value || this.content);
            this.formGroupChange.emit(this.formGroup);
        } else {
            this.contentChange.emit(value || this.content);
        }
    }

    previewContent() {
        this._libHelperService.dialogViewerRef(
            this.content,
            this.contentType,
        );
    }


    getCaretPos(event) {
        if (event.target.selectionStart || event.target.selectionStart == '0') {
            this.caretPos = event.target.selectionStart;
        }
        this.emitContent(event.target.value);
    }

    checkValid(event?:any){
        if(event){
            this.inValid = !(event.target.value && (event.target.value.trim())) || !event.target.value;
        } else{
            this.inValid = !(this.content && (this.content.trim())) || !this.content;
        }
    }

    changeContentHtml(value) {
		this.ref.detectChanges();
		if(value) this.setValidEditor();
		if(this.editorContentEl && this.maxLength) {
			const contentText = this.editorContentEl.innerHTML.replaceAll('&nbsp;', ' ').replace(/<[^>]+>/g, '');
			const contentTextLength = contentText.split('').length;
			if(contentTextLength >= this.maxLength) {
				this.contentLimit = (contentText.split('').slice(0, this.maxLength)).join('');
				this.editable = false;
				setTimeout(() => {
					this.content = this.contentLimit;
					setTimeout(() => {
						this.editable = true;
						try {
							let range = document.createRange();
							let pos = this.editorContentEl?.lastChild?.textContent?.length;
							let sel = window.getSelection();
							range.setStart(this.editorContentEl?.lastChild, pos);
							range.collapse(true);
							sel.removeAllRanges();
							sel.addRange(range);
						} catch {}
					}, 0);
				}, 0);
			} else {
				this.contentLimit = null;
			} 		
		}
		//
        this.emitContent(value);
    }

	setValidEditor() {
		setTimeout(() => {
			const wrapperEditorEl = document.getElementById(this.editorId);
			const editorContentEl = wrapperEditorEl?.getElementsByClassName('angular-editor-textarea')[0] as HTMLElement;
			try {
				if(((this.isRequired && this.isFocusEditor) || (this.isRequired && this.inValid)) && !this.isChooseImage) {
					let styleBorder: string = '1px solid #EAECF0';
					let contentText = editorContentEl.innerHTML.replaceAll('&nbsp;', ' ');
					if(contentText) contentText = contentText.trim();
					// Lỗi thư viện xóa nội dung vẫn còn thừa thẻ <br> cần remove đi
					if(contentText === '<br>') {
						this.content = null;
						contentText = null;
					}
					if(editorContentEl) {
						const isInvalid = !(contentText);
						if(isInvalid) {
							styleBorder = '1px solid red';
						}
					}
					editorContentEl.style.border = styleBorder;
				} 
				
			} catch {}
		}, 0);
	}

    changeViewMode(isInsertImageHtml: boolean){
        this.isInsertImageHtml = isInsertImageHtml;
        this.isFocusEditor = false;
    }
    
	ngOnDestroy() {
		if(this.editorContentEl) {
			this.editorContentEl.removeEventListener('focus', this.focusEditor);
		}
    }

}
