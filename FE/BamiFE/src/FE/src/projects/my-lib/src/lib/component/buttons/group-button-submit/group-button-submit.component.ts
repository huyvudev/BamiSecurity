import { ChangeDetectionStrategy, ChangeDetectorRef, Component, DestroyRef, EventEmitter, Input, NgZone, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { AbstractControl, FormArray, FormGroup } from '@angular/forms';
import { Utils } from '../../../shared/utils';
import { LibHelperService } from '../../../shared/services/lib-helper.service';
import { IEventSubmitForm } from '../../../shared/interfaces/base.interface';
import { NavigationConfirmService } from '@mylib-shared/services/navigation-confirm-service';
import { map, skip, takeUntil, takeWhile, timer } from 'rxjs';
import { UtilDestroyService } from '@mylib-shared/services/util-destroy.service';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop'
@Component({
    selector: 'lib-groupButtonSubmit',
    templateUrl: './group-button-submit.component.html',
    styleUrls: ['./group-button-submit.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers:[UtilDestroyService]
})
export class GroupButtonSubmitComponent implements OnChanges {

    constructor(
        private _libHelperService: LibHelperService,
        private destroyRef: DestroyRef,
		private ngZone: NgZone,
		private cd: ChangeDetectorRef,
    ) {

    }

    @Input() actionType: 'create' | 'update' | 'confirm' = 'create';
    @Input() type: 'dialog' | 'page' = 'dialog';

    @Input() permissionUpdate: boolean = false;
    @Input() labelEdit = 'Chỉnh sửa';
    @Input() labelCancel = 'Huỷ';
    @Input() labelClose = 'Đóng';
    @Input() labelSubmit = 'Lưu';
    @Input() postForm: FormGroup;
    @Input() messageConfirmCancel: string = 'Bạn chắc chắn muốn huỷ?';
    @Input() confirmCancel: boolean = true;
    @Input() warningDataChange: boolean = true;
    @Input() disabled: boolean = false;
    @Input() isButtonEditPrimary: boolean = false;
    @Input() alwaysSubmit: boolean = false;
    @Input() setFormDataInit: any;

    @Input() styleClassButtonEdit: string = '';

    @Input() isEdit: boolean = false;
    @Input() navigationService: any;
    @Input() isCreateSuccess: boolean;
    @Output() isEditChange = new EventEmitter<boolean>(false);;


    @Output() cancel = new EventEmitter<{hasChangeData: boolean, originalData: any}>(null);
    @Output() submit = new EventEmitter<any>(null);
    @Output() activeEdit = new EventEmitter<any>(null);

    showButtonEdit: boolean = false;
    labelCancelDeffault = 'Huỷ';
    labelSubmitDeffault = 'Lưu'
    isDialogType: boolean = true;
    buttonClose: boolean = false;
	isInit: boolean = true;

    formDataInit: any;
	id: string;
	classSecondary = 'btn-secondary';

    ngOnInit(): void {
		this.id = `group-button-${new Date().getTime()}`;
		this.taskRunOutsideAngular();
		//
        if(this.navigationService){
            this.isInit = false;
            this.checkNavigation()
        }
    }

	taskRunOutsideAngular() {
		this.ngZone.runOutsideAngular(() => {
			timer(0, 200)
			.pipe(
				map(() => document.getElementById(this.id)),
				takeWhile((element) => !element, true)
			)
			.subscribe((element) => {
				if(element) {
					this.ngZone.run(() => {
						this.isDialogType = !!element.closest('p-dynamicdialog');
						if(this.isDialogType) {
							this.classSecondary = '';
						}
						this.cd.markForCheck();
					})
				}
			})
		})
	}

    ngOnChanges(changes: SimpleChanges): void {
        if(changes['actionType']?.currentValue) {
            switch(this.actionType) {
                case 'confirm':
                    this.isEdit = true;
                    this.labelSubmit = this.labelSubmit !== this.labelSubmitDeffault ? this.labelSubmit : 'Xác nhận';
                    break;
                default:
            }
        }
        //
		if(changes['isEdit'] || (changes['postForm']?.currentValue && changes['postForm']?.previousValue === undefined)) {
			if(changes['isEdit']?.currentValue || changes['postForm']?.currentValue) {
                setTimeout(() => {
                    this.formDataInit = this.postForm?.getRawValue() && Utils.cloneData(this.postForm?.getRawValue());
                }, 200)
			}
		}
    }

    get checkChangeData(): IEventSubmitForm {
        const hasChangeData = !Utils.compareData(this.formDataInit, this.postForm?.getRawValue());
        const dataEmit: IEventSubmitForm = {
            hasChangeData: hasChangeData,
            originalData: this.formDataInit,
            actionType: this.actionType
        }
        //
        return dataEmit;
    }

    checkNavigation(){
        this.navigationService.checkConfirm.pipe(skip(1), takeUntilDestroyed(this.destroyRef)).subscribe((status) => {
            if (status) {
                const isConfirmCreate = this.actionType === 'create' && !this.isCreateSuccess && (this.checkChangeData.hasChangeData);
                const isConfirmUpdate = this.actionType === 'update' && this.isEdit && this.checkChangeData.hasChangeData;
                let isConfirm = !!((isConfirmCreate || isConfirmUpdate) && !this.isInit);
                this.navigationService.pushData(isConfirm, this.messageConfirmCancel);
            }
        });
    }

    _cancel() {
        // console.log(this.formDataInit, this.postForm?.getRawValue());
        const dataChangeInfo = this.checkChangeData;
        if(dataChangeInfo.hasChangeData && this.confirmCancel) {
            this._libHelperService.dialogConfirmRef(this.messageConfirmCancel, {
                maskStyleClass: this.type === 'dialog' ? 'unset-filter' : ''
			}).onClose.subscribe((accept: boolean) => {
                if (accept) {
                    this.cancel.emit(dataChangeInfo);
                }
            })
        } else {
            this.cancel.emit(dataChangeInfo);
        }
    }

    _submit() {
        if(this.postForm) {
            const dataChangeInfo = this.checkChangeData;
            if(!dataChangeInfo.hasChangeData && this.actionType === 'update' && this.warningDataChange && this.postForm) {
                this._libHelperService.messageWarn("Không có thay đổi được cập nhật!");
				setTimeout(() => {
					this.submit.emit(dataChangeInfo);
				}, 0);
				return;
            }
            // Phòng trường hợp thêm mới nhưng không có trường nào invalid thêm được dữ liệu null hoặc data đổ sẵn
            this.submit.emit(dataChangeInfo);
        } else if(!this.postForm) {
            this.submit.emit();
        }
    }

	_activeEdit() {
		this.activeEdit.emit();
	}

    ngOnDestroy(){
       
    }
}
